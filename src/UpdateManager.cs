using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyRelay;

/// <summary>
/// 自动更新管理器 - 通过 GitHub Releases 检查更新
/// </summary>
public class UpdateManager : IDisposable
{
    private const string GITHUB_OWNER = "KFC444";
    private const string GITHUB_REPO = "ClipGlow";
    private const string RELEASES_API = $"https://api.github.com/repos/{GITHUB_OWNER}/{GITHUB_REPO}/releases/latest";

    private readonly HttpClient _httpClient;
    private readonly System.Windows.Forms.Timer _checkTimer;
    private bool _disposed;
    private bool _isChecking;

    /// <summary>
    /// 发现新版本时触发
    /// </summary>
    public event Action<UpdateInfo>? UpdateAvailable;

    public UpdateManager()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", $"CopyRelay/{AppInfo.Version}");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

        // 每6小时检查一次更新
        _checkTimer = new System.Windows.Forms.Timer { Interval = 6 * 60 * 60 * 1000 };
        _checkTimer.Tick += async (s, e) => await CheckForUpdatesAsync(silent: true);
    }

    /// <summary>
    /// 启动自动更新检查
    /// </summary>
    public void StartAutoCheck()
    {
        if (!ConfigManager.Instance.EnableAutoUpdate) return;

        _checkTimer.Start();

        // 启动后延迟30秒检查一次
        Task.Run(async () =>
        {
            await Task.Delay(30000);
            if (!_disposed)
            {
                await CheckForUpdatesAsync(silent: true);
            }
        });

        Logger.Debug("自动更新检查已启动");
    }

    /// <summary>
    /// 停止自动更新检查
    /// </summary>
    public void StopAutoCheck()
    {
        _checkTimer.Stop();
        Logger.Debug("自动更新检查已停止");
    }

    /// <summary>
    /// 检查更新
    /// </summary>
    /// <param name="silent">静默模式：无更新时不提示</param>
    public async Task CheckForUpdatesAsync(bool silent = false)
    {
        if (_isChecking) return;
        _isChecking = true;

        try
        {
            Logger.Debug("正在检查更新...");

            var response = await _httpClient.GetAsync(RELEASES_API);

            if (!response.IsSuccessStatusCode)
            {
                if (!silent)
                {
                    Logger.Warn($"检查更新失败: HTTP {response.StatusCode}");
                    ShowMessage("检查更新失败，请稍后重试。", "更新检查", MessageBoxIcon.Warning);
                }
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var release = JsonSerializer.Deserialize<GitHubRelease>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (release == null)
            {
                if (!silent)
                {
                    ShowMessage("无法解析更新信息。", "更新检查", MessageBoxIcon.Warning);
                }
                return;
            }

            var latestVersion = ParseVersion(release.TagName);
            var currentVersion = ParseVersion(AppInfo.Version);

            if (latestVersion > currentVersion)
            {
                var updateInfo = new UpdateInfo
                {
                    Version = release.TagName ?? "unknown",
                    ReleaseNotes = release.Body ?? "暂无更新说明",
                    DownloadUrl = FindDownloadUrl(release),
                    HtmlUrl = release.HtmlUrl ?? ""
                };

                Logger.Info($"发现新版本: {updateInfo.Version}");
                UpdateAvailable?.Invoke(updateInfo);

                // 显示更新对话框
                ShowUpdateDialog(updateInfo);
            }
            else if (!silent)
            {
                ShowMessage($"当前已是最新版本 v{AppInfo.Version}", "更新检查", MessageBoxIcon.Information);
            }
        }
        catch (HttpRequestException ex)
        {
            Logger.Error("网络请求失败", ex);
            if (!silent)
            {
                ShowMessage("网络连接失败，请检查网络后重试。", "更新检查", MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Logger.Error("检查更新时出错", ex);
            if (!silent)
            {
                ShowMessage($"检查更新时出错: {ex.Message}", "更新检查", MessageBoxIcon.Error);
            }
        }
        finally
        {
            _isChecking = false;
        }
    }

    /// <summary>
    /// 显示更新对话框
    /// </summary>
    private void ShowUpdateDialog(UpdateInfo info)
    {
        var message = $"发现新版本 {info.Version}\n\n" +
                      $"当前版本: v{AppInfo.Version}\n\n" +
                      $"更新说明:\n{TruncateText(info.ReleaseNotes, 300)}\n\n" +
                      "是否立即前往下载？";

        var result = MessageBox.Show(
            message,
            "发现新版本",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information
        );

        if (result == DialogResult.Yes)
        {
            OpenDownloadPage(info);
        }
    }

    /// <summary>
    /// 打开下载页面
    /// </summary>
    public void OpenDownloadPage(UpdateInfo info)
    {
        try
        {
            var url = !string.IsNullOrEmpty(info.DownloadUrl) ? info.DownloadUrl : info.HtmlUrl;
            if (string.IsNullOrEmpty(url))
            {
                url = $"https://github.com/{GITHUB_OWNER}/{GITHUB_REPO}/releases/latest";
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
            Logger.Debug($"打开下载页面: {url}");
        }
        catch (Exception ex)
        {
            Logger.Error("打开下载页面失败", ex);
            ShowMessage("无法打开浏览器，请手动访问 GitHub 下载最新版本。", "错误", MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// 解析版本号 (移除v前缀)
    /// </summary>
    private static Version ParseVersion(string? versionStr)
    {
        if (string.IsNullOrEmpty(versionStr)) return new Version(0, 0, 0);

        var cleanVersion = versionStr.TrimStart('v', 'V');

        // 处理只有两位版本号的情况 (1.0 -> 1.0.0)
        var parts = cleanVersion.Split('.');
        if (parts.Length == 2)
        {
            cleanVersion += ".0";
        }

        return Version.TryParse(cleanVersion, out var version) ? version : new Version(0, 0, 0);
    }

    /// <summary>
    /// 查找下载链接 (优先exe，其次zip)
    /// </summary>
    private static string? FindDownloadUrl(GitHubRelease release)
    {
        if (release.Assets == null || release.Assets.Length == 0)
            return release.HtmlUrl;

        // 优先找 .exe 文件
        foreach (var asset in release.Assets)
        {
            if (asset.Name?.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) == true)
            {
                return asset.BrowserDownloadUrl;
            }
        }

        // 其次找 .zip 文件
        foreach (var asset in release.Assets)
        {
            if (asset.Name?.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) == true)
            {
                return asset.BrowserDownloadUrl;
            }
        }

        return release.HtmlUrl;
    }

    /// <summary>
    /// 截断文本
    /// </summary>
    private static string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return "";
        if (text.Length <= maxLength) return text;
        return text.Substring(0, maxLength) + "...";
    }

    /// <summary>
    /// 显示消息框 (线程安全)
    /// </summary>
    private static void ShowMessage(string message, string title, MessageBoxIcon icon)
    {
        if (Application.OpenForms.Count > 0)
        {
            var form = Application.OpenForms[0];
            if (form != null && form.InvokeRequired)
            {
                form.Invoke(() => MessageBox.Show(form, message, title, MessageBoxButtons.OK, icon));
                return;
            }
        }
        MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _checkTimer?.Stop();
        _checkTimer?.Dispose();
        _httpClient?.Dispose();
    }

    #region JSON 数据模型

    private class GitHubRelease
    {
        public string? TagName { get; set; }
        public string? Name { get; set; }
        public string? Body { get; set; }
        public string? HtmlUrl { get; set; }
        public GitHubAsset[]? Assets { get; set; }
    }

    private class GitHubAsset
    {
        public string? Name { get; set; }
        public string? BrowserDownloadUrl { get; set; }
    }

    #endregion
}

/// <summary>
/// 更新信息
/// </summary>
public class UpdateInfo
{
    public string Version { get; set; } = "";
    public string ReleaseNotes { get; set; } = "";
    public string? DownloadUrl { get; set; }
    public string HtmlUrl { get; set; } = "";
}
