using System;
using System.Windows.Forms;

namespace CopyRelay;

static class Program
{
    [STAThread]
    static void Main()
    {
        // 确保单实例运行
        using var mutex = new System.Threading.Mutex(true, "CopyRelay_SingleInstance", out bool createdNew);
        if (!createdNew)
        {
            MessageBox.Show("CopyRelay 已在运行中!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        // 启动主应用
        using var app = new CopyRelayApp();
        app.Run();
    }
}

/// <summary>
/// CopyRelay 主应用
/// </summary>
public class CopyRelayApp : IDisposable
{
    private readonly ClipboardMonitor _clipboardMonitor;
    private readonly FeedbackManager _feedbackManager;
    private readonly TrayIconManager _trayIcon;
    private bool _disposed;

    public CopyRelayApp()
    {
        _feedbackManager = new FeedbackManager();
        _clipboardMonitor = new ClipboardMonitor(OnClipboardChanged);
        _trayIcon = new TrayIconManager(OnExit, OnSettingsChanged);

        // 初始化自启动设置
        InitializeAutoStart();
    }

    private void InitializeAutoStart()
    {
        var config = ConfigManager.Instance;

        // 如果配置启用自启动，但注册表中未设置，则设置自启动
        if (config.EnableAutoStart && !AutoStartManager.IsAutoStartEnabled())
        {
            AutoStartManager.EnableAutoStart();
        }
        // 如果配置禁用自启动，但注册表中已设置，则移除自启动
        else if (!config.EnableAutoStart && AutoStartManager.IsAutoStartEnabled())
        {
            AutoStartManager.DisableAutoStart();
        }
    }

    private void OnClipboardChanged(string content)
    {
        _feedbackManager.ShowFeedback();
        _trayIcon.Flash();
    }

    private void OnSettingsChanged()
    {
        // 配置变更时刷新图标
        _feedbackManager.RefreshIcon();
    }

    private void OnExit()
    {
        Application.Exit();
    }

    public void Run()
    {
        _clipboardMonitor.Start();
        Application.Run();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _clipboardMonitor?.Dispose();
        _feedbackManager?.Dispose();
        _trayIcon?.Dispose();
    }
}
