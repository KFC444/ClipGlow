using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace CopyRelay;

/// <summary>
/// 自启动管理器 - 管理Windows开机自启动
/// </summary>
public static class AutoStartManager
{
    private const string AppName = "CopyRelay";
    private const string RegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

    /// <summary>
    /// 获取当前程序的可执行文件路径
    /// </summary>
    private static string GetExecutablePath()
    {
        // 使用AppContext.BaseDirectory代替Assembly.Location以支持单文件发布
        var baseDir = AppContext.BaseDirectory;
        var exeName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        if (string.IsNullOrEmpty(exeName))
        {
            exeName = "CopyRelay";
        }
        return Path.Combine(baseDir, exeName + ".exe");
    }

    /// <summary>
    /// 检查是否已设置自启动
    /// </summary>
    public static bool IsAutoStartEnabled()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, false);
            if (key == null) return false;

            var value = key.GetValue(AppName) as string;
            var currentPath = GetExecutablePath();

            return !string.IsNullOrEmpty(value) &&
                   value.Equals(currentPath, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 启用自启动
    /// </summary>
    public static bool EnableAutoStart()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, true);
            if (key == null) return false;

            var exePath = GetExecutablePath();
            if (!File.Exists(exePath))
            {
                return false;
            }

            key.SetValue(AppName, exePath);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"启用自启动失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 禁用自启动
    /// </summary>
    public static bool DisableAutoStart()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, true);
            if (key == null) return false;

            if (key.GetValue(AppName) != null)
            {
                key.DeleteValue(AppName, false);
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"禁用自启动失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 设置自启动状态
    /// </summary>
    public static bool SetAutoStart(bool enable)
    {
        return enable ? EnableAutoStart() : DisableAutoStart();
    }
}
