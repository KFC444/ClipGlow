using System;
using System.Diagnostics;
using System.IO;

namespace CopyRelay;

/// <summary>
/// 简单日志记录器 - 记录到文件和调试输出
/// </summary>
public static class Logger
{
    private static readonly string LogDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CopyRelay"
    );

    private static readonly string LogFile = Path.Combine(LogDir, "app.log");
    private static readonly object _lock = new();

    /// <summary>
    /// 是否启用文件日志（默认仅调试输出）
    /// </summary>
    public static bool EnableFileLog { get; set; } = false;

    /// <summary>
    /// 记录调试信息
    /// </summary>
    public static void Debug(string message)
    {
        Log("DEBUG", message);
    }

    /// <summary>
    /// 记录一般信息
    /// </summary>
    public static void Info(string message)
    {
        Log("INFO", message);
    }

    /// <summary>
    /// 记录警告
    /// </summary>
    public static void Warn(string message)
    {
        Log("WARN", message);
    }

    /// <summary>
    /// 记录错误
    /// </summary>
    public static void Error(string message, Exception? ex = null)
    {
        var fullMessage = ex != null ? $"{message}: {ex.Message}" : message;
        Log("ERROR", fullMessage);

        if (ex != null)
        {
            System.Diagnostics.Debug.WriteLine($"[CopyRelay] Exception details: {ex}");
        }
    }

    private static void Log(string level, string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var logEntry = $"[{timestamp}] [{level}] {message}";

        // 始终输出到调试窗口
        System.Diagnostics.Debug.WriteLine($"[CopyRelay] {logEntry}");

        // 可选：写入文件
        if (EnableFileLog)
        {
            WriteToFile(logEntry);
        }
    }

    private static void WriteToFile(string logEntry)
    {
        try
        {
            lock (_lock)
            {
                Directory.CreateDirectory(LogDir);

                // 限制日志文件大小（超过 1MB 时清空）
                if (File.Exists(LogFile))
                {
                    var fileInfo = new FileInfo(LogFile);
                    if (fileInfo.Length > 1024 * 1024)
                    {
                        File.Delete(LogFile);
                    }
                }

                File.AppendAllText(LogFile, logEntry + Environment.NewLine);
            }
        }
        catch
        {
            // 日志写入失败时静默忽略
        }
    }
}
