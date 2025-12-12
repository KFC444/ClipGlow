using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace CopyRelay;

/// <summary>
/// 反馈动画管理器 - 显示可爱的图标动画
/// </summary>
public class FeedbackManager : IDisposable
{
    private readonly FeedbackForm _form;
    private readonly System.Windows.Forms.Timer _animationTimer;
    private int _animationFrame;
    private bool _isAnimating;
    private bool _disposed;

    // 动画参数
    private const int TotalFrames = 50;
    private const int FadeInFrames = 8;
    private const int StayFrames = 30;
    private const int FadeOutFrames = 12;

    public FeedbackManager()
    {
        _form = new FeedbackForm();
        _animationTimer = new System.Windows.Forms.Timer { Interval = 16 }; // ~60fps
        _animationTimer.Tick += OnAnimationTick;
    }

    /// <summary>
    /// 刷新图标（配置变更后调用）
    /// </summary>
    public void RefreshIcon()
    {
        _form.RefreshIcon();
    }

    public void ShowFeedback()
    {
        if (!ConfigManager.Instance.EnableIconFeedback) return;
        if (_isAnimating) return;

        _isAnimating = true;
        _animationFrame = 0;

        // 获取鼠标位置
        var mousePos = Cursor.Position;
        int iconSize = ConfigManager.Instance.IconSize;
        _form.SetPosition(mousePos.X + 15, mousePos.Y + 15);
        _form.Show();

        _animationTimer.Start();
    }

    private void OnAnimationTick(object? sender, EventArgs e)
    {
        _animationFrame++;

        float alpha;
        float scale;

        if (_animationFrame <= FadeInFrames)
        {
            // 淡入阶段
            float progress = (float)_animationFrame / FadeInFrames;
            alpha = progress;
            scale = 0.6f + 0.4f * progress;
        }
        else if (_animationFrame <= FadeInFrames + StayFrames)
        {
            // 停留阶段 - 轻微呼吸效果
            alpha = 1f;
            int stayFrame = _animationFrame - FadeInFrames;
            scale = 1f + 0.03f * (float)Math.Sin(stayFrame * 0.25);
        }
        else if (_animationFrame <= TotalFrames)
        {
            // 淡出阶段
            float progress = (float)(_animationFrame - FadeInFrames - StayFrames) / FadeOutFrames;
            alpha = 1f - progress;
            scale = 1f - 0.15f * progress;
        }
        else
        {
            // 动画结束
            _animationTimer.Stop();
            _form.Hide();
            _isAnimating = false;
            return;
        }

        _form.UpdateAnimation(alpha, scale);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _animationTimer?.Stop();
        _animationTimer?.Dispose();
        _form?.Dispose();
    }

    /// <summary>
    /// 透明动画窗口
    /// </summary>
    private class FeedbackForm : Form
    {
        private float _alpha = 1f;
        private float _scale = 1f;
        private Bitmap? _iconBitmap;

        public FeedbackForm()
        {
            // 窗口设置
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.Magenta;
            TransparencyKey = Color.Magenta;

            // 初始化图标
            RefreshIcon();

            // 双缓冲
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;       // WS_EX_TOOLWINDOW
                cp.ExStyle |= 0x20;       // WS_EX_TRANSPARENT (点击穿透)
                cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE
                return cp;
            }
        }

        /// <summary>
        /// 刷新图标
        /// </summary>
        public void RefreshIcon()
        {
            // 先释放旧图标
            _iconBitmap?.Dispose();
            _iconBitmap = null;

            var config = ConfigManager.Instance;
            int size = config.IconSize;

            // 尝试加载自定义图标
            if (!string.IsNullOrEmpty(config.CustomIconPath) && File.Exists(config.CustomIconPath))
            {
                try
                {
                    using var original = new Bitmap(config.CustomIconPath);
                    _iconBitmap = new Bitmap(original, size, size);
                    Logger.Debug($"加载自定义图标: {config.CustomIconPath}");
                }
                catch (Exception ex)
                {
                    Logger.Warn($"加载自定义图标失败: {ex.Message}，使用默认图标");
                    _iconBitmap = IconGenerator.GenerateIcon(config.IconStyle, size);
                }
            }
            else
            {
                _iconBitmap = IconGenerator.GenerateIcon(config.IconStyle, size);
            }

            // 更新窗口大小
            Size = new Size(size + 10, size + 10);
        }

        public void SetPosition(int x, int y)
        {
            Location = new Point(x, y);
        }

        public void UpdateAnimation(float alpha, float scale)
        {
            _alpha = alpha;
            _scale = scale;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_iconBitmap == null) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.Clear(Color.Magenta);

            int baseSize = ConfigManager.Instance.IconSize;
            int size = (int)(baseSize * _scale);
            int offset = (Width - size) / 2;

            // 创建带透明度的图像
            var colorMatrix = new System.Drawing.Imaging.ColorMatrix
            {
                Matrix33 = _alpha
            };
            using var attributes = new System.Drawing.Imaging.ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(
                _iconBitmap,
                new Rectangle(offset, offset, size, size),
                0, 0, _iconBitmap.Width, _iconBitmap.Height,
                GraphicsUnit.Pixel,
                attributes
            );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _iconBitmap?.Dispose();
                _iconBitmap = null;
            }
            base.Dispose(disposing);
        }
    }
}
