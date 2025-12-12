# 🎉 ClipGlow v1.0.0 - Initial Release

## ✨ Features

- **10 Emoji-style icon designs** with gradients, highlights and shadows
- **Cyberpunk neon color scheme** (Cyan/Blue/Purple/Green/Orange/Yellow)
- **Smooth 60fps animations** (fade in/stay/fade out)
- **Auto-start on boot** with toggle in tray menu
- **Custom icon support** (PNG/JPG/BMP/GIF/ICO)
- **Adjustable icon size** (20-48px)
- **Tray icon flash notification**
- **Single instance application**

## 🎨 Icon Styles

Cat Paw 🐾 | Snowflake ❄️ | Bolt ⚡ | Sun ☀️ | Star ⭐

Hexagon ⬡ | Leaf 🍃 | Moon 🌙 | Circuit 🔌 | Diamond 💎

## 🛠️ Tech Stack

- .NET 8.0 (Windows Forms)
- C# 12
- System.Drawing (GDI+)
- Event-driven architecture

## 📦 Installation

1. Download `CopyRelay.exe`
2. Double-click to run
3. The program will appear in the system tray
4. Copy anything (Ctrl+C) to see the animated icon!

## ⚙️ Requirements

- Windows 10/11
- .NET 8.0 Runtime (will auto-install if missing)

## 🎯 Usage

- **Right-click tray icon** to access settings
- **Toggle auto-start** in the menu
- **Choose icon style** from 10 built-in options
- **Import custom icons** for personalization
- **Adjust icon size** to your preference

## 📝 Configuration

Config file: `%AppData%\CopyRelay\config.json`

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

## 🐛 Known Issues

None

## 📄 License

MIT License - See LICENSE file for details
