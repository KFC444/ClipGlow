using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CopyRelay;

/// <summary>
/// 全局鼠标钩子 - 检测鼠标左键状态（用于判断预复制）
/// </summary>
public class MouseHook : IDisposable
{
    private const int WH_MOUSE_LL = 14;
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_LBUTTONUP = 0x0202;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string? lpModuleName);

    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    private readonly LowLevelMouseProc _proc;
    private IntPtr _hookId = IntPtr.Zero;
    private bool _disposed;

    /// <summary>
    /// 鼠标左键是否正在按住（正在拖动选中）
    /// </summary>
    public bool IsLeftButtonDown { get; private set; }

    /// <summary>
    /// 鼠标左键释放的时间戳
    /// </summary>
    public DateTime LeftButtonUpTime { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// 预复制检测阈值（毫秒）- 左键释放后这个时间内的剪贴板变化视为预复制
    /// </summary>
    public int PreCopyThresholdMs { get; set; } = 50;

    /// <summary>
    /// 判断当前是否处于可能的预复制状态
    /// </summary>
    public bool IsPossiblePreCopy()
    {
        // 情况1：鼠标左键正在按住（正在拖动选中）
        if (IsLeftButtonDown)
        {
            return true;
        }

        // 情况2：鼠标左键刚释放不久（选中刚完成）
        var timeSinceLeftUp = (DateTime.Now - LeftButtonUpTime).TotalMilliseconds;
        if (timeSinceLeftUp < PreCopyThresholdMs)
        {
            return true;
        }

        return false;
    }

    public MouseHook()
    {
        _proc = HookCallback;
    }

    public void Start()
    {
        if (_hookId != IntPtr.Zero) return;

        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;

        if (curModule != null)
        {
            _hookId = SetWindowsHookEx(WH_MOUSE_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
            if (_hookId == IntPtr.Zero)
            {
                Logger.Debug($"鼠标钩子安装失败: {Marshal.GetLastWin32Error()}");
            }
        }
    }

    public void Stop()
    {
        if (_hookId != IntPtr.Zero)
        {
            UnhookWindowsHookEx(_hookId);
            _hookId = IntPtr.Zero;
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            int msg = wParam.ToInt32();

            if (msg == WM_LBUTTONDOWN)
            {
                IsLeftButtonDown = true;
            }
            else if (msg == WM_LBUTTONUP)
            {
                IsLeftButtonDown = false;
                LeftButtonUpTime = DateTime.Now;
            }
        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Stop();
    }
}
