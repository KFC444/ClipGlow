using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CopyRelay;

/// <summary>
/// 系统托盘图标管理器
/// </summary>
public class TrayIconManager : IDisposable
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool DestroyIcon(IntPtr handle);

    private readonly NotifyIcon _notifyIcon;
    private readonly System.Windows.Forms.Timer _flashTimer;
    private readonly Icon _normalIcon;
    private readonly Icon _flashIcon;
    private int _flashCount;
    private bool _disposed;
    private readonly Action _onExit;
    private readonly Action _onSettingsChanged;
    private readonly UpdateManager? _updateManager;

    public TrayIconManager(Action onExit, Action onSettingsChanged, UpdateManager? updateManager = null)
    {
        _onExit = onExit;
        _onSettingsChanged = onSettingsChanged;
        _updateManager = updateManager;

        // 生成图标
        _normalIcon = CreateTrayIcon(Color.FromArgb(100, 180, 255));
        _flashIcon = CreateTrayIcon(Color.FromArgb(255, 200, 100));

        // 创建托盘图标
        _notifyIcon = new NotifyIcon
        {
            Icon = _normalIcon,
            Text = "ClipGlow - 复制反馈器",
            Visible = true,
            ContextMenuStrip = CreateContextMenu()
        };

        // 闪烁定时器
        _flashTimer = new System.Windows.Forms.Timer { Interval = 150 };
        _flashTimer.Tick += OnFlashTick;
    }

    public void Flash()
    {
        if (!ConfigManager.Instance.EnableTrayFlash) return;
        if (_flashTimer.Enabled) return;

        _flashCount = 0;
        _flashTimer.Start();
    }

    private void OnFlashTick(object? sender, EventArgs e)
    {
        _flashCount++;

        if (_flashCount >= 6)
        {
            _flashTimer.Stop();
            _notifyIcon.Icon = _normalIcon;
            return;
        }

        _notifyIcon.Icon = (_flashCount % 2 == 1) ? _flashIcon : _normalIcon;
    }

    private ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();
        var config = ConfigManager.Instance;

        // 标题
        var titleItem = new ToolStripMenuItem(AppInfo.Name) { Enabled = false };
        menu.Items.Add(titleItem);
        menu.Items.Add(new ToolStripSeparator());

        // ===== 图标样式子菜单 =====
        var iconStyleMenu = new ToolStripMenuItem("图标样式");
        foreach (IconGenerator.IconStyle style in Enum.GetValues<IconGenerator.IconStyle>())
        {
            var item = new ToolStripMenuItem(IconGenerator.GetStyleName(style))
            {
                Checked = config.IconStyle == style,
                Tag = style
            };
            item.Click += (s, e) =>
            {
                config.IconStyle = style;
                config.CustomIconPath = null; // 清除自定义图标
                config.Save();
                _onSettingsChanged?.Invoke();
                RefreshMenu();
            };
            iconStyleMenu.DropDownItems.Add(item);
        }

        // 自定义图标选项
        iconStyleMenu.DropDownItems.Add(new ToolStripSeparator());
        var customIconItem = new ToolStripMenuItem("导入自定义图标...")
        {
            Checked = !string.IsNullOrEmpty(config.CustomIconPath)
        };
        customIconItem.Click += OnImportCustomIcon;
        iconStyleMenu.DropDownItems.Add(customIconItem);

        // 清除自定义图标
        if (!string.IsNullOrEmpty(config.CustomIconPath))
        {
            var clearCustomItem = new ToolStripMenuItem("清除自定义图标");
            clearCustomItem.Click += (s, e) =>
            {
                config.CustomIconPath = null;
                config.Save();
                _onSettingsChanged?.Invoke();
                RefreshMenu();
            };
            iconStyleMenu.DropDownItems.Add(clearCustomItem);
        }

        menu.Items.Add(iconStyleMenu);

        // ===== 图标大小子菜单 =====
        var sizeMenu = new ToolStripMenuItem("图标大小");
        int[] sizes = { 20, 24, 28, 32, 40, 48 };
        foreach (int size in sizes)
        {
            var item = new ToolStripMenuItem($"{size} px")
            {
                Checked = config.IconSize == size,
                Tag = size
            };
            item.Click += (s, e) =>
            {
                config.IconSize = size;
                config.Save();
                _onSettingsChanged?.Invoke();
                RefreshMenu();
            };
            sizeMenu.DropDownItems.Add(item);
        }
        menu.Items.Add(sizeMenu);

        menu.Items.Add(new ToolStripSeparator());

        // ===== 开关选项 =====
        var feedbackItem = new ToolStripMenuItem("启用图标反馈")
        {
            Checked = config.EnableIconFeedback,
            CheckOnClick = true
        };
        feedbackItem.CheckedChanged += (s, e) =>
        {
            config.EnableIconFeedback = feedbackItem.Checked;
            config.Save();
        };
        menu.Items.Add(feedbackItem);

        var flashItem = new ToolStripMenuItem("启用托盘闪烁")
        {
            Checked = config.EnableTrayFlash,
            CheckOnClick = true
        };
        flashItem.CheckedChanged += (s, e) =>
        {
            config.EnableTrayFlash = flashItem.Checked;
            config.Save();
        };
        menu.Items.Add(flashItem);

        var autoStartItem = new ToolStripMenuItem("开机自启动")
        {
            Checked = config.EnableAutoStart,
            CheckOnClick = true
        };
        autoStartItem.CheckedChanged += (s, e) =>
        {
            config.EnableAutoStart = autoStartItem.Checked;
            config.Save();

            // 应用自启动设置
            if (AutoStartManager.SetAutoStart(config.EnableAutoStart))
            {
                // 成功
            }
            else
            {
                // 失败时恢复状态
                autoStartItem.Checked = AutoStartManager.IsAutoStartEnabled();
                config.EnableAutoStart = autoStartItem.Checked;
                config.Save();

                MessageBox.Show(
                    "设置开机自启动失败，请检查权限。",
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        };
        menu.Items.Add(autoStartItem);

        // 自动更新
        var autoUpdateItem = new ToolStripMenuItem("自动检查更新")
        {
            Checked = config.EnableAutoUpdate,
            CheckOnClick = true
        };
        autoUpdateItem.CheckedChanged += (s, e) =>
        {
            config.EnableAutoUpdate = autoUpdateItem.Checked;
            config.Save();

            if (autoUpdateItem.Checked)
            {
                _updateManager?.StartAutoCheck();
            }
            else
            {
                _updateManager?.StopAutoCheck();
            }
        };
        menu.Items.Add(autoUpdateItem);

        menu.Items.Add(new ToolStripSeparator());

        // 检查更新
        var checkUpdateItem = new ToolStripMenuItem("检查更新...");
        checkUpdateItem.Click += async (s, e) =>
        {
            if (_updateManager != null)
            {
                await _updateManager.CheckForUpdatesAsync(silent: false);
            }
            else
            {
                MessageBox.Show("更新功能未启用。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };
        menu.Items.Add(checkUpdateItem);

        // 关于
        var aboutItem = new ToolStripMenuItem("关于");
        aboutItem.Click += (s, e) =>
        {
            MessageBox.Show(
                $"{AppInfo.Name} - {AppInfo.Description}\n\n" +
                $"版本: {AppInfo.Version}\n\n" +
                "功能:\n" +
                "• 14种内置图标样式\n" +
                "• 4种像素风可爱图标\n" +
                "• 支持自定义图标\n" +
                "• 可调节图标大小\n" +
                "• 托盘闪烁提示\n" +
                "• 自动检查更新",
                $"关于 {AppInfo.Name}",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        };
        menu.Items.Add(aboutItem);

        menu.Items.Add(new ToolStripSeparator());

        // 退出
        var exitItem = new ToolStripMenuItem("退出");
        exitItem.Click += (s, e) =>
        {
            _notifyIcon.Visible = false;
            _onExit?.Invoke();
        };
        menu.Items.Add(exitItem);

        return menu;
    }

    private void OnImportCustomIcon(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "选择图标图片",
            Filter = "图片文件|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.ico|所有文件|*.*",
            CheckFileExists = true
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // 验证图片可以加载
                using var test = new Bitmap(dialog.FileName);

                ConfigManager.Instance.CustomIconPath = dialog.FileName;
                ConfigManager.Instance.Save();
                _onSettingsChanged?.Invoke();
                RefreshMenu();

                MessageBox.Show(
                    "自定义图标设置成功！",
                    "成功",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"无法加载图片: {ex.Message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }

    private void RefreshMenu()
    {
        _notifyIcon.ContextMenuStrip?.Dispose();
        _notifyIcon.ContextMenuStrip = CreateContextMenu();
    }

    private static Icon CreateTrayIcon(Color mainColor)
    {
        int size = 32;
        using var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);

        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);

        var darkerColor = Color.FromArgb(
            Math.Max(0, mainColor.R - 30),
            Math.Max(0, mainColor.G - 40),
            Math.Max(0, mainColor.B - 55)
        );

        using var fillBrush = new SolidBrush(mainColor);
        using var outlinePen = new Pen(darkerColor, 1.5f);

        // 剪贴板主体
        g.FillRoundedRectangle(fillBrush, 5, 6, 22, 23, 3);
        g.DrawRoundedRectangle(outlinePen, 5, 6, 22, 23, 3);

        // 夹子
        using var clipBrush = new SolidBrush(Color.FromArgb(80, 80, 80));
        g.FillRoundedRectangle(clipBrush, 10, 2, 12, 7, 2);

        // 内容线条
        using var linePen = new Pen(Color.FromArgb(200, 255, 255, 255), 2);
        g.DrawLine(linePen, 9, 14, 22, 14);
        g.DrawLine(linePen, 9, 19, 19, 19);
        g.DrawLine(linePen, 9, 24, 17, 24);

        // 小星星
        using var starBrush = new SolidBrush(Color.FromArgb(255, 220, 100));
        g.FillEllipse(starBrush, 22, 3, 7, 7);

        // 创建 Icon 并正确管理句柄
        IntPtr hIcon = bmp.GetHicon();
        try
        {
            return Icon.FromHandle(hIcon).Clone() as Icon ?? throw new InvalidOperationException("Failed to create icon");
        }
        finally
        {
            DestroyIcon(hIcon);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _flashTimer?.Stop();
        _flashTimer?.Dispose();
        _notifyIcon?.Dispose();
        _normalIcon?.Dispose();
        _flashIcon?.Dispose();
    }
}
