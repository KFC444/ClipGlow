# ClipGlow - 赛博朋克风格的复制反馈器

<div align="center">

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![Language](https://img.shields.io/badge/language-C%23-green.svg)

**让每一次复制都充满仪式感**

一个轻量级的Windows托盘应用，当你复制内容时，会在屏幕上显示炫酷的动画图标。

[English](README_EN.md) | 简体中文

</div>

---

## 特性

### 14种内置图标样式
- **赛博朋克风格**: 猫爪、雪花、闪电、太阳、星星、六边形、叶子、月亮、电路、钻石
- **像素风格**: 像素爱心、像素机器人、像素幽灵、像素史莱姆

### 丰富的自定义选项
- 支持导入自定义图标 (PNG/JPG/BMP/GIF/ICO)
- 可调节图标大小 (20-48px)
- 托盘图标闪烁提示
- 开机自启动
- 自动检查更新

### 流畅的动画
- 淡入阶段: 透明度 0→1，缩放 0.6→1.0
- 停留阶段: 轻微呼吸效果
- 淡出阶段: 透明度 1→0，缩放 1.0→0.85
- 60fps 流畅动画

---

## 快速开始

### 方式1: 下载发行版 (推荐)

1. 前往 [GitHub Releases](https://github.com/KFC444/ClipGlow/releases) 或 [Gitee 发行版](https://gitee.com/kfc444/clip-glow/releases) 页面
2. 下载最新版本的 `ClipGlow.exe`
3. 双击运行即可

### 方式2: 从源码编译

```bash
# 克隆仓库
git clone https://github.com/KFC444/ClipGlow.git
cd ClipGlow/src

# 编译发布
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true

# exe文件位于 bin/Release/net8.0-windows/win-x64/ClipGlow.exe
```

---

## 使用方法

1. **启动程序** - 双击 `ClipGlow.exe`，程序会在系统托盘显示图标
2. **测试效果** - 复制任何内容 (Ctrl+C)，屏幕上会弹出动画图标
3. **自定义设置** - 右键托盘图标打开菜单，选择图标样式、大小等
4. **检查更新** - 右键托盘图标 → "检查更新..."

---

## 配置说明

配置文件位置: `%AppData%\CopyRelay\config.json`

```json
{
  "IconStyle": 0,              // 图标样式 (0-13)
  "IconSize": 32,              // 图标大小 (16-64)
  "CustomIconPath": null,      // 自定义图标路径
  "EnableTrayFlash": true,     // 启用托盘闪烁
  "EnableIconFeedback": true,  // 启用图标反馈
  "EnableAutoStart": true,     // 开机自启动
  "EnableAutoUpdate": true     // 自动检查更新
}
```

### 图标样式对照表

| 值 | 样式 | 值 | 样式 |
|:---:|:---:|:---:|:---:|
| 0 | 猫爪 | 7 | 月亮 |
| 1 | 雪花 | 8 | 电路 |
| 2 | 闪电 | 9 | 钻石 |
| 3 | 太阳 | 10 | 像素爱心 |
| 4 | 星星 | 11 | 像素机器人 |
| 5 | 六边形 | 12 | 像素幽灵 |
| 6 | 叶子 | 13 | 像素史莱姆 |

---

## 技术栈

- **框架**: .NET 8.0 (Windows Forms)
- **语言**: C# 12
- **图形**: System.Drawing (GDI+)

### 核心组件

```
src/
├── Program.cs              # 程序入口
├── ClipboardMonitor.cs     # 剪贴板监听
├── FeedbackManager.cs      # 动画反馈管理
├── TrayIconManager.cs      # 托盘图标管理
├── IconGenerator.cs        # 图标生成器
├── ConfigManager.cs        # 配置管理
├── UpdateManager.cs        # 自动更新
├── AutoStartManager.cs     # 自启动管理
├── AppInfo.cs              # 应用信息
├── Logger.cs               # 日志记录
└── GraphicsExtensions.cs   # 绘图扩展
```

---

## 更新日志

### v1.1.0
- 新增 4 种像素风格图标
- 新增自动检查更新功能
- 修复剪贴板监听问题
- 代码结构优化

### v1.0.0
- 10 种赛博朋克风格图标
- 完整的自定义选项
- 开机自启动功能
- 60fps 流畅动画

---

## 许可证

本项目采用 MIT 许可证 - 详见 [LICENSE](LICENSE) 文件

---

## 反馈

- **问题反馈**: [GitHub Issues](https://github.com/KFC444/ClipGlow/issues)
- **Gitee Issues**: [Gitee Issues](https://gitee.com/kfc444/clip-glow/issues)

---

<div align="center">

**如果这个项目对你有帮助，请给个 Star 支持一下！**

</div>
