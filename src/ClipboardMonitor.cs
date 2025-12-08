using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CopyRelay;

/// <summary>
/// 剪贴板监听器 - 使用 Windows 原生 API
/// </summary>
public class ClipboardMonitor : IDisposable
{
    private readonly Action<string> _onClipboardChanged;
    private readonly ClipboardNotificationForm _form;
    private string? _lastContent;
    private bool _disposed;

    public ClipboardMonitor(Action<string> onClipboardChanged)
    {
        _onClipboardChanged = onClipboardChanged;
        _form = new ClipboardNotificationForm(HandleClipboardChange);
    }

    public void Start()
    {
        // 表单已在构造时启动监听
    }

    private void HandleClipboardChange()
    {
        try
        {
            if (!Clipboard.ContainsText()) return;

            string content = Clipboard.GetText();
            if (string.IsNullOrEmpty(content)) return;

            // 过滤重复
            if (content == _lastContent) return;
            _lastContent = content;

            _onClipboardChanged?.Invoke(content);
        }
        catch
        {
            // 忽略剪贴板访问错误
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _form?.Dispose();
    }

    /// <summary>
    /// 隐藏窗口用于接收剪贴板通知
    /// </summary>
    private class ClipboardNotificationForm : Form
    {
        private readonly Action _onClipboardChange;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        private const int WM_CLIPBOARDUPDATE = 0x031D;

        public ClipboardNotificationForm(Action onClipboardChange)
        {
            _onClipboardChange = onClipboardChange;

            // 隐藏窗口
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            Size = new System.Drawing.Size(0, 0);
            Opacity = 0;

            // 注册剪贴板监听
            Load += (s, e) => AddClipboardFormatListener(Handle);
            FormClosing += (s, e) => RemoveClipboardFormatListener(Handle);

            // 在后台创建窗口
            Show();
            Hide();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CLIPBOARDUPDATE)
            {
                _onClipboardChange?.Invoke();
            }
            base.WndProc(ref m);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80; // WS_EX_TOOLWINDOW - 不在任务栏显示
                return cp;
            }
        }
    }
}
