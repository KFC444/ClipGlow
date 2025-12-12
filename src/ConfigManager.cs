using System;
using System.IO;
using System.Text.Json;

namespace CopyRelay;

/// <summary>
/// 配置管理器 - 保存用户设置
/// </summary>
public class ConfigManager
{
    private static readonly string ConfigDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ClipGlow"
    );

    private static readonly string ConfigFile = Path.Combine(ConfigDir, "config.json");

    public static ConfigManager Instance { get; } = new();

    /// <summary>
    /// 当前图标样式
    /// </summary>
    public IconGenerator.IconStyle IconStyle { get; set; } = IconGenerator.IconStyle.CatPaw;

    /// <summary>
    /// 图标大小 (16-64)
    /// </summary>
    public int IconSize { get; set; } = 32;

    /// <summary>
    /// 自定义图标路径 (为空则使用内置图标)
    /// </summary>
    public string? CustomIconPath { get; set; }

    /// <summary>
    /// 启用托盘闪烁
    /// </summary>
    public bool EnableTrayFlash { get; set; } = true;

    /// <summary>
    /// 启用图标反馈
    /// </summary>
    public bool EnableIconFeedback { get; set; } = true;

    /// <summary>
    /// 开机自启动
    /// </summary>
    public bool EnableAutoStart { get; set; } = true;

    /// <summary>
    /// 启用自动更新检查
    /// </summary>
    public bool EnableAutoUpdate { get; set; } = true;

    private ConfigManager()
    {
        Load();
    }

    /// <summary>
    /// 加载配置
    /// </summary>
    public void Load()
    {
        try
        {
            if (File.Exists(ConfigFile))
            {
                var json = File.ReadAllText(ConfigFile);
                var data = JsonSerializer.Deserialize<ConfigData>(json);
                if (data != null)
                {
                    IconStyle = (IconGenerator.IconStyle)data.IconStyle;
                    IconSize = Math.Clamp(data.IconSize, 16, 64);
                    CustomIconPath = data.CustomIconPath;
                    EnableTrayFlash = data.EnableTrayFlash;
                    EnableIconFeedback = data.EnableIconFeedback;
                    EnableAutoStart = data.EnableAutoStart;
                    EnableAutoUpdate = data.EnableAutoUpdate;
                    Logger.Debug("配置加载成功");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("加载配置失败，使用默认值", ex);
        }
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    public void Save()
    {
        try
        {
            Directory.CreateDirectory(ConfigDir);

            var data = new ConfigData
            {
                IconStyle = (int)IconStyle,
                IconSize = IconSize,
                CustomIconPath = CustomIconPath,
                EnableTrayFlash = EnableTrayFlash,
                EnableIconFeedback = EnableIconFeedback,
                EnableAutoStart = EnableAutoStart,
                EnableAutoUpdate = EnableAutoUpdate
            };

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFile, json);
            Logger.Debug("配置保存成功");
        }
        catch (Exception ex)
        {
            Logger.Error("保存配置失败", ex);
        }
    }

    private class ConfigData
    {
        public int IconStyle { get; set; }
        public int IconSize { get; set; } = 32;
        public string? CustomIconPath { get; set; }
        public bool EnableTrayFlash { get; set; } = true;
        public bool EnableIconFeedback { get; set; } = true;
        public bool EnableAutoStart { get; set; } = true;
        public bool EnableAutoUpdate { get; set; } = true;
    }
}
