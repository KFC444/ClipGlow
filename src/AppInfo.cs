using System.Reflection;

namespace CopyRelay;

/// <summary>
/// 应用程序信息 - 统一获取版本等信息
/// </summary>
public static class AppInfo
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public const string Name = "ClipGlow";

    /// <summary>
    /// 应用描述
    /// </summary>
    public const string Description = "复制反馈器 - 复制时显示可爱图标动画";

    /// <summary>
    /// 获取版本号（从程序集读取）
    /// </summary>
    public static string Version
    {
        get
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "1.0.0";
        }
    }

    /// <summary>
    /// 获取完整版本信息
    /// </summary>
    public static string FullVersion
    {
        get
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version?.ToString() ?? "1.0.0.0";
        }
    }
}
