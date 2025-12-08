# ✨ ClipGlow - 赛博朋克风格的复制反馈器

<div align="center">

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![Language](https://img.shields.io/badge/language-C%23-green.svg)

**让每一次复制都充满仪式感** 🎨

一个轻量级的Windows托盘应用，当你复制内容时，会在屏幕上显示炫酷的Emoji风格动画图标。

[English](README_EN.md) | 简体中文

</div>

---

## 📸 预览

<div align="center">

### 🎯 10种内置图标样式


|  猫爪 🐾  | 雪花 ❄️ | 闪电 ⚡ | 太阳 ☀️ | 星星 ⭐ |
| :------: | :----: | :----: | :----: | :----: |
| 六边形 ⬡ | 叶子 🍃 | 月亮 🌙 | 电路 🔌 | 钻石 💎 |

</div>

---

## ✨ 特性

### 🎨 **Emoji风格设计**
- **渐变填充**: 每个图标都使用精心设计的渐变色
- **高光阴影**: 立体感十足的光影效果
- **圆润线条**: 柔和的圆角和端点
- **装饰元素**: 星星、闪光等可爱细节

### 🌈 **赛博朋克配色**
- 霓虹青 (Neon Cyan)
- 霓虹蓝 (Neon Blue)
- 霓虹紫 (Neon Purple)
- 霓虹绿 (Neon Green)
- 霓虹橙 (Neon Orange)
- 霓虹黄 (Neon Yellow)

### ⚙️ **丰富的自定义选项**
- ✅ 10种内置图标样式
- ✅ 支持导入自定义图标 (PNG/JPG/BMP/GIF/ICO)
- ✅ 可调节图标大小 (20-48px)
- ✅ 托盘图标闪烁提示
- ✅ 开机自启动
- ✅ 单实例运行

### 🎭 **流畅的动画**
- **淡入阶段**: 透明度 0→1，缩放 0.6→1.0
- **停留阶段**: 轻微呼吸效果 (缩放 1.0±0.03)
- **淡出阶段**: 透明度 1→0，缩放 1.0→0.85
- **60fps**: 流畅丝滑的动画体验

---

## 🚀 快速开始

### 📦 下载安装

#### 方式1: 下载发行版 (推荐)
1. 前往 [Releases](https://github.com/yourusername/ClipGlow/releases) 页面
2. 下载最新版本的 `ClipGlow.exe`
3. 双击运行即可

#### 方式2: 从源码编译
```bash
# 克隆仓库
git clone https://github.com/yourusername/ClipGlow.git
cd ClipGlow/CSharp

# 编译项目
dotnet build -c Release

# 运行程序
dotnet run
```

### 🎯 使用方法

1. **启动程序**
   - 双击 `ClipGlow.exe` 或运行编译后的程序
   - 程序会在系统托盘显示图标

2. **测试效果**
   - 复制任何内容 (Ctrl+C 或右键复制)
   - 屏幕上会弹出动画图标反馈

3. **自定义设置**
   - 右键托盘图标打开菜单
   - 选择图标样式、调整大小、导入自定义图标等

4. **开机自启动**
   - 右键托盘图标 → "开机自启动" (默认已启用)
   - 取消勾选即可禁用自启动

---

## ⚙️ 配置说明

配置文件位置: `%AppData%\CopyRelay\config.json`

```json
{
  "IconStyle": 0,              // 图标样式 (0-9)
  "IconSize": 32,              // 图标大小 (16-64)
  "CustomIconPath": null,      // 自定义图标路径
  "EnableTrayFlash": true,     // 启用托盘闪烁
  "EnableIconFeedback": true,  // 启用图标反馈
  "EnableAutoStart": true      // 开机自启动
}
```

### 图标样式对照表

| 值 | 样式 | 值 | 样式 |
|:---:|:---:|:---:|:---:|
| 0 | 猫爪 | 5 | 六边形 |
| 1 | 雪花 | 6 | 叶子 |
| 2 | 闪电 | 7 | 月亮 |
| 3 | 太阳 | 8 | 电路 |
| 4 | 星星 | 9 | 钻石 |

---

## 🛠️ 技术栈

- **框架**: .NET 8.0 (Windows Forms)
- **语言**: C# 12
- **图形**: System.Drawing (GDI+)
- **架构**: 事件驱动 + 单例模式

### 核心组件

```
ClipGlow/
├── Program.cs              # 程序入口
├── CopyRelayApp.cs         # 主应用逻辑
├── ClipboardMonitor.cs     # 剪贴板监听
├── FeedbackManager.cs      # 动画反馈管理
├── TrayIconManager.cs      # 托盘图标管理
├── IconGenerator.cs        # 图标生成器 (核心)
├── ConfigManager.cs        # 配置管理
└── AutoStartManager.cs     # 自启动管理
```

---

## 🎨 图标设计原理

### Emoji风格的核心要素

1. **渐变系统**
   ```csharp
   // 每种颜色都有亮色和暗色版本
   NeonCyan + NeonCyanDark       // 青色渐变
   NeonBlue + NeonBlueDark       // 蓝色渐变
   NeonPurple + NeonPurpleDark   // 紫色渐变
   // ... 更多颜色
   ```

2. **高光效果**
   ```csharp
   // 半透明白色高光，增加立体感
   Highlight = Color.FromArgb(120, 255, 255, 255)
   ```

3. **阴影效果**
   ```csharp
   // 半透明黑色阴影，增强深度
   Shadow = Color.FromArgb(80, 0, 0, 0)
   ```

4. **绘制流程**
   ```
   阴影层 → 渐变填充 → 高光 → 轮廓 → 装饰元素
   ```

---

## 🤝 贡献指南

欢迎贡献代码、报告问题或提出建议！

### 如何贡献

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启 Pull Request

### 开发环境

- Visual Studio 2022 或 VS Code
- .NET 8.0 SDK
- Windows 10/11

---

## 📝 更新日志

### v1.0.0 (2025-12-08)

#### ✨ 新功能
- 🎨 10种Emoji风格图标
- 🌈 赛博朋克霓虹配色
- ⚙️ 完整的自定义选项
- 🚀 开机自启动功能
- 💫 流畅的60fps动画

#### 🎯 优化
- 渐变填充系统
- 高光阴影效果
- 圆润的线条和端点
- 装饰元素和细节

---

## 📄 许可证

本项目采用 MIT 许可证 - 详见 [LICENSE](LICENSE) 文件

---

## 🙏 致谢

- 灵感来源于各种Emoji设计
- 赛博朋克配色参考了霓虹灯美学
- 感谢所有贡献者和用户的支持

---

## 📮 联系方式

- **问题反馈**: [GitHub Issues](https://github.com/yourusername/ClipGlow/issues)
- **功能建议**: [GitHub Discussions](https://github.com/yourusername/ClipGlow/discussions)

---

<div align="center">

**如果这个项目对你有帮助，请给个 ⭐ Star 支持一下！**

Made with ❤️ by [Your Name]

</div>
