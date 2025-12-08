# ✨ ClipGlow - Cyberpunk Style Clipboard Feedback

<div align="center">

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![Language](https://img.shields.io/badge/language-C%23-green.svg)

**Make every copy a ritual** 🎨

A lightweight Windows tray application that displays cool Emoji-style animated icons when you copy content.

English | [简体中文](README.md)

</div>

---

## ✨ Features

### 🎨 **Emoji-Style Design**
- **Gradient Fill**: Each icon uses carefully designed gradient colors
- **Highlights & Shadows**: Three-dimensional lighting effects
- **Rounded Lines**: Soft rounded corners and endpoints
- **Decorative Elements**: Cute details like stars and sparkles

### 🌈 **Cyberpunk Color Scheme**
- Neon Cyan
- Neon Blue
- Neon Purple
- Neon Green
- Neon Orange
- Neon Yellow

### ⚙️ **Rich Customization Options**
- ✅ 10 built-in icon styles
- ✅ Support for custom icons (PNG/JPG/BMP/GIF/ICO)
- ✅ Adjustable icon size (20-48px)
- ✅ Tray icon flash notification
- ✅ Auto-start on boot
- ✅ Single instance

### 🎭 **Smooth Animations**
- **Fade In**: Opacity 0→1, Scale 0.6→1.0
- **Stay**: Subtle breathing effect (Scale 1.0±0.03)
- **Fade Out**: Opacity 1→0, Scale 1.0→0.85
- **60fps**: Smooth animation experience

---

## 🚀 Quick Start

### 📦 Download & Install

#### Method 1: Download Release (Recommended)
1. Go to [Releases](https://github.com/yourusername/ClipGlow/releases) page
2. Download the latest `ClipGlow.exe`
3. Double-click to run

#### Method 2: Build from Source
```bash
# Clone repository
git clone https://github.com/yourusername/ClipGlow.git
cd ClipGlow/CSharp

# Build project
dotnet build -c Release

# Run program
dotnet run
```

### 🎯 Usage

1. **Launch Program**
   - Double-click `ClipGlow.exe` or run the compiled program
   - The program will display an icon in the system tray

2. **Test Effect**
   - Copy any content (Ctrl+C or right-click copy)
   - An animated icon will appear on the screen

3. **Customize Settings**
   - Right-click the tray icon to open the menu
   - Select icon style, adjust size, import custom icons, etc.

4. **Auto-Start**
   - Right-click tray icon → "Auto-Start" (enabled by default)
   - Uncheck to disable auto-start

---

## ⚙️ Configuration

Config file location: `%AppData%\CopyRelay\config.json`

```json
{
  "IconStyle": 0,              // Icon style (0-9)
  "IconSize": 32,              // Icon size (16-64)
  "CustomIconPath": null,      // Custom icon path
  "EnableTrayFlash": true,     // Enable tray flash
  "EnableIconFeedback": true,  // Enable icon feedback
  "EnableAutoStart": true      // Auto-start on boot
}
```

---

## 🛠️ Tech Stack

- **Framework**: .NET 8.0 (Windows Forms)
- **Language**: C# 12
- **Graphics**: System.Drawing (GDI+)
- **Architecture**: Event-driven + Singleton pattern

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

---

<div align="center">

**If this project helps you, please give it a ⭐ Star!**

Made with ❤️ by [Your Name]

</div>
