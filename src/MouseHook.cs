using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CopyRelay;

/// <summary>
/// 全局鼠标钩子 - 检测鼠标拖动选择行为（用于判断预复制）
/// </summary>
public class MouseHook : IDisposable
{
    private const int WH_MOUSE_LL = 14;
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_LBUTTONUP = 0x0202;
    private const int WM_MOUSEMOVE = 0x0200;

    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT
    {
        public Point pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

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

    // 鼠标状态
    private bool _isLeftButtonDown;
    private Point _leftButtonDownPos;
    private Point _leftButtonUpPos;
    private DateTime _leftButtonUpTime = DateTime.MinValue;
    private bool _wasDragging;

    /// <summary>
    /// 预复制检测阈值（毫秒）- 拖动选择后这个时间内的剪贴板变化视为预复制
    /// </summary>
    public int PreCopyThresholdMs { get; set; } = 80;

    /// <summary>
    /// 拖动距离阈值（像素）- 超过此距离认为是拖动选择而非点击
    /// </summary>
    public int DragDistanceThreshold { get; set; } = 10;

    /// <summary>
    /// 判断当前是否处于可能的预复制状态
    /// 只有拖动选择（而非点击）后的短时间内才认为是预复制
    /// </summary>
    public bool IsPossiblePreCopy()
    {
        // 情况1：鼠标左键正在按住且已经拖动了一定距离（正在拖动选中）
        if (_isLeftButtonDown && _wasDragging)
        {
            return true;
        }

        // 情况2：刚完成拖动选择（不是点击）
        if (_wasDragging)
        {
            var timeSinceLeftUp = (DateTime.Now - _leftButtonUpTime).TotalMilliseconds;
            if (timeSinceLeftUp < PreCopyThresholdMs)
            {
                return true;
            }
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
            var hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);

            if (msg == WM_LBUTTONDOWN)
            {
                _isLeftButtonDown = true;
                _leftButtonDownPos = hookStruct.pt;
                _wasDragging = false;
            }
            else if (msg == WM_MOUSEMOVE && _isLeftButtonDown)
            {
                // 检测是否已经拖动了足够距离
                if (!_wasDragging)
                {
                    int dx = Math.Abs(hookStruct.pt.X - _leftButtonDownPos.X);
                    int dy = Math.Abs(hookStruct.pt.Y - _leftButtonDownPos.Y);
                    if (dx > DragDistanceThreshold || dy > DragDistanceThreshold)
                    {
                        _wasDragging = true;
                    }
                }
            }
            else if (msg == WM_LBUTTONUP)
            {
                _isLeftButtonDown = false;
                _leftButtonUpPos = hookStruct.pt;
                _leftButtonUpTime = DateTime.Now;

                // 最终确认是否为拖动（再次检查距离）
                if (!_wasDragging)
                {
                    int dx = Math.Abs(_leftButtonUpPos.X - _leftButtonDownPos.X);
                    int dy = Math.Abs(_leftButtonUpPos.Y - _leftButtonDownPos.Y);
                    _wasDragging = (dx > DragDistanceThreshold || dy > DragDistanceThreshold);
                }
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
