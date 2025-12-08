using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CopyRelay;

/// <summary>
/// 图标生成器 - 赛博朋克风格
/// 霓虹色 + 深色 + 科技感
/// </summary>
public static class IconGenerator
{
    public enum IconStyle
    {
        CatPaw,     // 猫爪
        Snowflake,  // 雪花
        Bolt,       // 闪电
        Sun,        // 太阳
        Star,       // 星星
        Hexagon,    // 六边形
        Leaf,       // 叶子
        Moon,       // 月亮
        Circuit,    // 电路
        Diamond,    // 钻石
    }

    public static string GetStyleName(IconStyle style) => style switch
    {
        IconStyle.CatPaw => "猫爪",
        IconStyle.Snowflake => "雪花",
        IconStyle.Bolt => "闪电",
        IconStyle.Sun => "太阳",
        IconStyle.Star => "星星",
        IconStyle.Hexagon => "六边形",
        IconStyle.Leaf => "叶子",
        IconStyle.Moon => "月亮",
        IconStyle.Circuit => "电路",
        IconStyle.Diamond => "钻石",
        _ => "未知"
    };

    public static Bitmap GenerateIcon(IconStyle style, int size)
    {
        return style switch
        {
            IconStyle.CatPaw => DrawCatPaw(size),
            IconStyle.Snowflake => DrawSnowflake(size),
            IconStyle.Bolt => DrawBolt(size),
            IconStyle.Sun => DrawSun(size),
            IconStyle.Star => DrawStar(size),
            IconStyle.Hexagon => DrawHexagon(size),
            IconStyle.Leaf => DrawLeaf(size),
            IconStyle.Moon => DrawMoon(size),
            IconStyle.Circuit => DrawCircuit(size),
            IconStyle.Diamond => DrawDiamond(size),
            _ => DrawCatPaw(size)
        };
    }

    // ==================== 赛博朋克配色 - Emoji风格增强版 ====================
    private static readonly Color NeonCyan = Color.FromArgb(0, 255, 255);        // 霓虹青
    private static readonly Color NeonCyanDark = Color.FromArgb(0, 180, 200);    // 霓虹青(暗)
    private static readonly Color NeonBlue = Color.FromArgb(0, 150, 255);        // 霓虹蓝
    private static readonly Color NeonBlueDark = Color.FromArgb(0, 100, 200);    // 霓虹蓝(暗)
    private static readonly Color NeonPurple = Color.FromArgb(180, 100, 255);    // 霓虹紫
    private static readonly Color NeonPurpleDark = Color.FromArgb(120, 60, 200); // 霓虹紫(暗)
    private static readonly Color NeonGreen = Color.FromArgb(0, 255, 150);       // 霓虹绿
    private static readonly Color NeonGreenDark = Color.FromArgb(0, 180, 100);   // 霓虹绿(暗)
    private static readonly Color NeonOrange = Color.FromArgb(255, 150, 0);      // 霓虹橙
    private static readonly Color NeonOrangeDark = Color.FromArgb(200, 100, 0);  // 霓虹橙(暗)
    private static readonly Color NeonYellow = Color.FromArgb(255, 255, 100);    // 霓虹黄
    private static readonly Color NeonYellowDark = Color.FromArgb(200, 200, 0);  // 霓虹黄(暗)
    private static readonly Color DarkBg = Color.FromArgb(20, 25, 35);           // 深色背景
    private static readonly Color DarkFill = Color.FromArgb(30, 40, 55);         // 深色填充
    private static readonly Color Highlight = Color.FromArgb(120, 255, 255, 255); // 高光
    private static readonly Color Shadow = Color.FromArgb(80, 0, 0, 0);          // 阴影

    /// <summary>
    /// 猫爪 - Emoji风格(渐变+高光+阴影)
    /// </summary>
    private static Bitmap DrawCatPaw(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float strokeW = Math.Max(2f, s * 0.065f);

        // 主肉垫 - 渐变填充
        var mainRect = new RectangleF(s * 0.2f, s * 0.42f, s * 0.6f, s * 0.48f);

        // 阴影层
        var shadowRect = new RectangleF(mainRect.X + s * 0.02f, mainRect.Y + s * 0.02f, mainRect.Width, mainRect.Height);
        using (var shadowBrush = new SolidBrush(Shadow))
        {
            g.FillEllipse(shadowBrush, shadowRect);
        }

        // 渐变填充
        using (var gradBrush = new LinearGradientBrush(mainRect, NeonCyan, NeonCyanDark, 45f))
        {
            g.FillEllipse(gradBrush, mainRect);
        }

        // 高光
        var highlightRect = new RectangleF(mainRect.X + mainRect.Width * 0.15f, mainRect.Y + mainRect.Height * 0.1f,
                                           mainRect.Width * 0.4f, mainRect.Height * 0.3f);
        using (var highlightBrush = new SolidBrush(Highlight))
        {
            g.FillEllipse(highlightBrush, highlightRect);
        }

        // 轮廓
        using (var glowPen = new Pen(Color.FromArgb(200, NeonCyan), strokeW))
        {
            g.DrawEllipse(glowPen, mainRect);
        }

        // 四个小肉垫 - 同样的渐变处理
        float[,] pads = {
            { 0.13f, 0.15f, 0.22f },
            { 0.32f, 0.05f, 0.20f },
            { 0.52f, 0.05f, 0.20f },
            { 0.69f, 0.15f, 0.22f },
        };

        for (int i = 0; i < 4; i++)
        {
            var padRect = new RectangleF(s * pads[i, 0], s * pads[i, 1], s * pads[i, 2], s * pads[i, 2]);

            // 阴影
            var padShadow = new RectangleF(padRect.X + s * 0.015f, padRect.Y + s * 0.015f, padRect.Width, padRect.Height);
            using (var shadowBrush = new SolidBrush(Shadow))
            {
                g.FillEllipse(shadowBrush, padShadow);
            }

            // 渐变
            using (var gradBrush = new LinearGradientBrush(padRect, NeonCyan, NeonCyanDark, 45f))
            {
                g.FillEllipse(gradBrush, padRect);
            }

            // 小高光
            var padHighlight = new RectangleF(padRect.X + padRect.Width * 0.2f, padRect.Y + padRect.Height * 0.15f,
                                              padRect.Width * 0.35f, padRect.Height * 0.25f);
            using (var highlightBrush = new SolidBrush(Highlight))
            {
                g.FillEllipse(highlightBrush, padHighlight);
            }

            // 轮廓
            using (var glowPen = new Pen(Color.FromArgb(200, NeonCyan), strokeW * 0.8f))
            {
                g.DrawEllipse(glowPen, padRect);
            }
        }

        return bmp;
    }

    /// <summary>
    /// 雪花 - Emoji风格(渐变分支+冰晶效果)
    /// </summary>
    private static Bitmap DrawSnowflake(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.06f);

        // 主分支 - 渐变效果
        for (int i = 0; i < 6; i++)
        {
            double angle = Math.PI / 3 * i;
            float x1 = center + (float)(Math.Cos(angle) * s * 0.4);
            float y1 = center + (float)(Math.Sin(angle) * s * 0.4);

            using var branchBrush = new LinearGradientBrush(
                new PointF(center, center), new PointF(x1, y1),
                NeonCyan, NeonBlue);
            using var branchPen = new Pen(branchBrush, strokeW)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            g.DrawLine(branchPen, center, center, x1, y1);

            // 分支末端装饰
            float tipSize = s * 0.04f;
            using var tipBrush = new SolidBrush(Color.FromArgb(180, NeonCyan));
            g.FillEllipse(tipBrush, x1 - tipSize, y1 - tipSize, tipSize * 2, tipSize * 2);

            // 侧分支
            float bx = center + (float)(Math.Cos(angle) * s * 0.27);
            float by = center + (float)(Math.Sin(angle) * s * 0.27);
            foreach (int offset in new[] { 55, -55 })
            {
                double branchAngle = angle + offset * Math.PI / 180;
                float bx2 = bx + (float)(Math.Cos(branchAngle) * s * 0.13);
                float by2 = by + (float)(Math.Sin(branchAngle) * s * 0.13);

                using var sideBrush = new LinearGradientBrush(
                    new PointF(bx, by), new PointF(bx2, by2),
                    NeonCyan, NeonCyanDark);
                using var sidePen = new Pen(sideBrush, strokeW * 0.6f)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                };
                g.DrawLine(sidePen, bx, by, bx2, by2);
            }
        }

        // 中心 - 渐变+高光
        float cr = s * 0.06f;
        var centerRect = new RectangleF(center - cr, center - cr, cr * 2, cr * 2);

        using (var centerGrad = new LinearGradientBrush(centerRect, NeonCyan, NeonBlue, 45f))
        {
            g.FillEllipse(centerGrad, centerRect);
        }

        // 中心高光
        var centerHighlight = new RectangleF(center - cr * 0.5f, center - cr * 0.6f, cr, cr * 0.7f);
        using (var highlightBrush = new SolidBrush(Highlight))
        {
            g.FillEllipse(highlightBrush, centerHighlight);
        }

        return bmp;
    }

    /// <summary>
    /// 闪电 - Emoji风格(能量渐变+电光效果)
    /// </summary>
    private static Bitmap DrawBolt(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float strokeW = Math.Max(2.2f, s * 0.07f);

        var points = new PointF[]
        {
            new(s * 0.55f, s * 0.05f),
            new(s * 0.22f, s * 0.48f),
            new(s * 0.45f, s * 0.48f),
            new(s * 0.32f, s * 0.95f),
            new(s * 0.78f, s * 0.42f),
            new(s * 0.55f, s * 0.42f),
        };

        // 阴影
        using (var shadowPath = new GraphicsPath())
        {
            var shadowPoints = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                shadowPoints[i] = new PointF(points[i].X + s * 0.03f, points[i].Y + s * 0.03f);
            }
            shadowPath.AddPolygon(shadowPoints);
            using var shadowBrush = new SolidBrush(Shadow);
            g.FillPath(shadowBrush, shadowPath);
        }

        // 渐变填充 - 从黄到橙
        using (var gradBrush = new LinearGradientBrush(
            new PointF(s * 0.55f, s * 0.05f),
            new PointF(s * 0.32f, s * 0.95f),
            NeonYellow, NeonOrange))
        {
            g.FillPolygon(gradBrush, points);
        }

        // 中心高光条
        using (var highlightBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
        {
            var highlightPoints = new PointF[]
            {
                new(s * 0.52f, s * 0.15f),
                new(s * 0.35f, s * 0.48f),
                new(s * 0.42f, s * 0.48f),
                new(s * 0.38f, s * 0.75f),
            };
            g.FillPolygon(highlightBrush, highlightPoints);
        }

        // 轮廓
        using (var glowPen = new Pen(Color.FromArgb(230, NeonYellow), strokeW) { LineJoin = LineJoin.Round })
        {
            g.DrawPolygon(glowPen, points);
        }

        // 电光装饰
        DrawSparkle(g, s * 0.2f, s * 0.3f, s * 0.06f, NeonYellow);
        DrawSparkle(g, s * 0.75f, s * 0.6f, s * 0.07f, NeonYellow);

        return bmp;
    }

    /// <summary>
    /// 太阳 - Emoji风格(渐变光芒+立体感)
    /// </summary>
    private static Bitmap DrawSun(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2.2f, s * 0.065f);

        // 光芒 - 渐变效果
        for (int i = 0; i < 8; i++)
        {
            double angle = Math.PI / 4 * i;
            float x1 = center + (float)(Math.Cos(angle) * s * 0.28);
            float y1 = center + (float)(Math.Sin(angle) * s * 0.28);
            float x2 = center + (float)(Math.Cos(angle) * s * 0.44);
            float y2 = center + (float)(Math.Sin(angle) * s * 0.44);

            using var rayBrush = new LinearGradientBrush(
                new PointF(x1, y1), new PointF(x2, y2),
                NeonOrange, NeonYellow);
            using var rayPen = new Pen(rayBrush, strokeW)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            g.DrawLine(rayPen, x1, y1, x2, y2);
        }

        float sr = s * 0.22f;
        var sunRect = new RectangleF(center - sr, center - sr, sr * 2, sr * 2);

        // 阴影
        var shadowRect = new RectangleF(sunRect.X + s * 0.02f, sunRect.Y + s * 0.02f, sunRect.Width, sunRect.Height);
        using (var shadowBrush = new SolidBrush(Shadow))
        {
            g.FillEllipse(shadowBrush, shadowRect);
        }

        // 渐变填充
        using (var gradBrush = new LinearGradientBrush(sunRect, NeonOrange, NeonYellow, 135f))
        {
            g.FillEllipse(gradBrush, sunRect);
        }

        // 高光
        var highlightRect = new RectangleF(center - sr * 0.5f, center - sr * 0.7f, sr * 0.8f, sr * 0.6f);
        using (var highlightBrush = new SolidBrush(Highlight))
        {
            g.FillEllipse(highlightBrush, highlightRect);
        }

        // 轮廓
        using (var glowPen = new Pen(Color.FromArgb(220, NeonOrange), strokeW * 0.8f))
        {
            g.DrawEllipse(glowPen, sunRect);
        }

        return bmp;
    }

    /// <summary>
    /// 星星 - Emoji风格(渐变+光芒+可爱)
    /// </summary>
    private static Bitmap DrawStar(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.06f);

        var points = new PointF[10];
        for (int i = 0; i < 10; i++)
        {
            double angle = Math.PI / 5 * i - Math.PI / 2;
            float r = (i % 2 == 0) ? s * 0.42f : s * 0.18f;
            points[i] = new PointF(
                center + (float)(Math.Cos(angle) * r),
                center + (float)(Math.Sin(angle) * r)
            );
        }

        // 阴影
        using (var shadowPath = new GraphicsPath())
        {
            var shadowPoints = new PointF[10];
            for (int i = 0; i < 10; i++)
            {
                shadowPoints[i] = new PointF(points[i].X + s * 0.025f, points[i].Y + s * 0.025f);
            }
            shadowPath.AddPolygon(shadowPoints);
            using var shadowBrush = new SolidBrush(Shadow);
            g.FillPath(shadowBrush, shadowPath);
        }

        // 渐变填充
        using (var gradBrush = new LinearGradientBrush(
            new PointF(center, center - s * 0.42f),
            new PointF(center, center + s * 0.42f),
            NeonYellow, NeonYellowDark))
        {
            g.FillPolygon(gradBrush, points);
        }

        // 中心高光
        var highlightSize = s * 0.15f;
        var highlightRect = new RectangleF(center - highlightSize / 2, center - s * 0.15f, highlightSize, highlightSize);
        using (var highlightBrush = new SolidBrush(Highlight))
        {
            g.FillEllipse(highlightBrush, highlightRect);
        }

        // 轮廓
        using (var glowPen = new Pen(Color.FromArgb(220, NeonYellow), strokeW) { LineJoin = LineJoin.Round })
        {
            g.DrawPolygon(glowPen, points);
        }

        // 可爱的小星星装饰
        DrawSparkle(g, center + s * 0.35f, center - s * 0.35f, s * 0.08f, NeonYellow);
        DrawSparkle(g, center - s * 0.38f, center + s * 0.32f, s * 0.06f, NeonYellow);

        return bmp;
    }

    // 辅助方法:绘制小星星装饰
    private static void DrawSparkle(Graphics g, float x, float y, float size, Color color)
    {
        using var pen = new Pen(Color.FromArgb(180, color), Math.Max(1.5f, size * 0.3f))
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        g.DrawLine(pen, x - size, y, x + size, y);
        g.DrawLine(pen, x, y - size, x, y + size);
    }

    /// <summary>
    /// 六边形 - Emoji风格(科技渐变+内发光)
    /// </summary>
    private static Bitmap DrawHexagon(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.06f);

        // 外六边形
        var outerPoints = new PointF[6];
        for (int i = 0; i < 6; i++)
        {
            double angle = Math.PI / 3 * i - Math.PI / 2;
            outerPoints[i] = new PointF(
                center + (float)(Math.Cos(angle) * s * 0.42),
                center + (float)(Math.Sin(angle) * s * 0.42)
            );
        }

        // 阴影
        using (var shadowPath = new GraphicsPath())
        {
            var shadowPoints = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                shadowPoints[i] = new PointF(outerPoints[i].X + s * 0.025f, outerPoints[i].Y + s * 0.025f);
            }
            shadowPath.AddPolygon(shadowPoints);
            using var shadowBrush = new SolidBrush(Shadow);
            g.FillPath(shadowBrush, shadowPath);
        }

        // 渐变填充
        using (var gradBrush = new LinearGradientBrush(
            new PointF(center, center - s * 0.42f),
            new PointF(center, center + s * 0.42f),
            NeonPurple, NeonPurpleDark))
        {
            g.FillPolygon(gradBrush, outerPoints);
        }

        // 顶部高光
        var highlightPoints = new PointF[]
        {
            outerPoints[0],
            outerPoints[1],
            new PointF(center + s * 0.15f, center - s * 0.05f),
            new PointF(center - s * 0.15f, center - s * 0.05f),
            outerPoints[5],
        };
        using (var highlightBrush = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
        {
            g.FillPolygon(highlightBrush, highlightPoints);
        }

        // 轮廓
        using (var glowPen = new Pen(Color.FromArgb(220, NeonPurple), strokeW) { LineJoin = LineJoin.Round })
        {
            g.DrawPolygon(glowPen, outerPoints);
        }

        // 内六边形 - 发光效果
        var innerPoints = new PointF[6];
        for (int i = 0; i < 6; i++)
        {
            double angle = Math.PI / 3 * i - Math.PI / 2;
            innerPoints[i] = new PointF(
                center + (float)(Math.Cos(angle) * s * 0.22),
                center + (float)(Math.Sin(angle) * s * 0.22)
            );
        }
        using (var innerPen = new Pen(Color.FromArgb(200, NeonCyan), strokeW * 0.6f) { LineJoin = LineJoin.Round })
        {
            g.DrawPolygon(innerPen, innerPoints);
        }

        // 中心点
        float cr = s * 0.04f;
        using (var centerBrush = new SolidBrush(NeonCyan))
        {
            g.FillEllipse(centerBrush, center - cr, center - cr, cr * 2, cr * 2);
        }

        return bmp;
    }

    /// <summary>
    /// 叶子 - Emoji风格(自然渐变+叶脉细节)
    /// </summary>
    private static Bitmap DrawLeaf(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.06f);

        using var path = new GraphicsPath();
        path.AddBezier(center, s * 0.08f, s * 0.88f, s * 0.2f, s * 0.88f, s * 0.72f, center, s * 0.92f);
        path.AddBezier(center, s * 0.92f, s * 0.12f, s * 0.72f, s * 0.12f, s * 0.2f, center, s * 0.08f);

        // 阴影
        using (var shadowPath = path.Clone() as GraphicsPath)
        {
            var shadowMatrix = new System.Drawing.Drawing2D.Matrix();
            shadowMatrix.Translate(s * 0.025f, s * 0.025f);
            shadowPath?.Transform(shadowMatrix);
            using var shadowBrush = new SolidBrush(Shadow);
            if (shadowPath != null) g.FillPath(shadowBrush, shadowPath);
        }

        // 渐变填充 - 从亮绿到深绿
        var leafBounds = path.GetBounds();
        using (var gradBrush = new LinearGradientBrush(
            new PointF(center, leafBounds.Top),
            new PointF(center, leafBounds.Bottom),
            NeonGreen, NeonGreenDark))
        {
            g.FillPath(gradBrush, path);
        }

        // 左侧高光
        using (var highlightPath = new GraphicsPath())
        {
            highlightPath.AddBezier(center, s * 0.15f, s * 0.65f, s * 0.25f, s * 0.55f, s * 0.5f, center, s * 0.7f);
            highlightPath.AddLine(center, s * 0.7f, center, s * 0.15f);
            using var highlightBrush = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
            g.FillPath(highlightBrush, highlightPath);
        }

        // 轮廓
        using (var glowPen = new Pen(Color.FromArgb(220, NeonGreen), strokeW) { LineJoin = LineJoin.Round })
        {
            g.DrawPath(glowPen, path);
        }

        // 主叶脉
        using (var veinPen = new Pen(Color.FromArgb(180, NeonCyan), strokeW * 0.6f)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        })
        {
            g.DrawLine(veinPen, center, s * 0.18f, center, s * 0.82f);

            // 侧叶脉
            for (float t = 0.3f; t <= 0.7f; t += 0.15f)
            {
                float y = s * (0.18f + t * 0.64f);
                float offset = s * 0.15f;
                using var sideVeinPen = new Pen(Color.FromArgb(120, NeonCyan), strokeW * 0.4f)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                };
                g.DrawLine(sideVeinPen, center, y, center + offset, y + offset * 0.5f);
                g.DrawLine(sideVeinPen, center, y, center - offset, y + offset * 0.5f);
            }
        }

        return bmp;
    }

    /// <summary>
    /// 月亮 - Emoji风格(渐变弯月+柔和光晕)
    /// </summary>
    private static Bitmap DrawMoon(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float strokeW = Math.Max(2f, s * 0.06f);

        using var moonPath = new GraphicsPath();
        moonPath.AddEllipse(s * 0.12f, s * 0.08f, s * 0.72f, s * 0.84f);

        using var cutPath = new GraphicsPath();
        cutPath.AddEllipse(s * 0.35f, s * 0.02f, s * 0.62f, s * 0.72f);

        using var region = new Region(moonPath);
        region.Exclude(cutPath);

        // 阴影
        using (var shadowRegion = region.Clone())
        {
            var shadowMatrix = new System.Drawing.Drawing2D.Matrix();
            shadowMatrix.Translate(s * 0.025f, s * 0.025f);
            shadowRegion.Transform(shadowMatrix);
            using var shadowBrush = new SolidBrush(Shadow);
            g.FillRegion(shadowBrush, shadowRegion);
        }

        // 渐变填充
        var moonRect = new RectangleF(s * 0.12f, s * 0.08f, s * 0.72f, s * 0.84f);
        using (var gradBrush = new LinearGradientBrush(moonRect, NeonBlue, NeonCyan, 45f))
        {
            g.FillRegion(gradBrush, region);
        }

        // 高光
        var highlightRect = new RectangleF(s * 0.18f, s * 0.2f, s * 0.25f, s * 0.35f);
        using (var highlightBrush = new SolidBrush(Highlight))
        {
            using var highlightRegion = new Region(highlightRect);
            highlightRegion.Intersect(region);
            g.FillRegion(highlightBrush, highlightRegion);
        }

        // 轮廓
        using (var glowPen = new Pen(Color.FromArgb(220, NeonBlue), strokeW))
        {
            g.DrawArc(glowPen, s * 0.12f, s * 0.08f, s * 0.72f, s * 0.84f, 45, 270);
        }

        // 小星星装饰
        DrawSparkle(g, s * 0.75f, s * 0.25f, s * 0.06f, NeonCyan);
        DrawSparkle(g, s * 0.82f, s * 0.65f, s * 0.05f, NeonBlue);

        return bmp;
    }

    /// <summary>
    /// 电路 - Emoji风格(渐变线路+发光节点)
    /// </summary>
    private static Bitmap DrawCircuit(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.055f);

        // 电路线条 - 使用渐变
        using (var linePen = new Pen(Color.FromArgb(200, NeonCyan), strokeW)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        })
        {
            g.DrawLine(linePen, s * 0.15f, center, s * 0.35f, center);
            g.DrawLine(linePen, s * 0.35f, center, s * 0.35f, s * 0.25f);
            g.DrawLine(linePen, s * 0.35f, s * 0.25f, s * 0.65f, s * 0.25f);
            g.DrawLine(linePen, s * 0.65f, s * 0.25f, s * 0.65f, center);
            g.DrawLine(linePen, s * 0.65f, center, s * 0.85f, center);

            g.DrawLine(linePen, s * 0.35f, center, s * 0.35f, s * 0.75f);
            g.DrawLine(linePen, s * 0.35f, s * 0.75f, s * 0.65f, s * 0.75f);
            g.DrawLine(linePen, s * 0.65f, s * 0.75f, s * 0.65f, center);

            g.DrawLine(linePen, center, s * 0.25f, center, s * 0.75f);
        }

        // 节点 - 渐变+发光
        float nr = s * 0.055f;
        var nodePositions = new PointF[]
        {
            new(s * 0.35f, center),
            new(s * 0.65f, center),
            new(center, s * 0.25f),
            new(center, s * 0.75f),
            new(center, center),
        };

        foreach (var pos in nodePositions)
        {
            var nodeRect = new RectangleF(pos.X - nr, pos.Y - nr, nr * 2, nr * 2);

            // 外发光
            using (var glowBrush = new SolidBrush(Color.FromArgb(80, NeonGreen)))
            {
                var glowRect = new RectangleF(pos.X - nr * 1.5f, pos.Y - nr * 1.5f, nr * 3, nr * 3);
                g.FillEllipse(glowBrush, glowRect);
            }

            // 渐变节点
            using (var nodeBrush = new LinearGradientBrush(nodeRect, NeonGreen, NeonCyan, 45f))
            {
                g.FillEllipse(nodeBrush, nodeRect);
            }

            // 高光
            var highlightRect = new RectangleF(pos.X - nr * 0.4f, pos.Y - nr * 0.6f, nr * 0.8f, nr * 0.6f);
            using (var highlightBrush = new SolidBrush(Highlight))
            {
                g.FillEllipse(highlightBrush, highlightRect);
            }
        }

        return bmp;
    }

    /// <summary>
    /// 钻石 - Emoji风格(多层渐变+切面高光)
    /// </summary>
    private static Bitmap DrawDiamond(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.06f);

        // 钻石外形
        var points = new PointF[]
        {
            new(center, s * 0.05f),      // 顶
            new(s * 0.85f, s * 0.35f),   // 右上
            new(s * 0.75f, s * 0.95f),   // 右下
            new(s * 0.25f, s * 0.95f),   // 左下
            new(s * 0.15f, s * 0.35f),   // 左上
        };

        // 阴影
        using (var shadowPath = new GraphicsPath())
        {
            var shadowPoints = new PointF[5];
            for (int i = 0; i < 5; i++)
            {
                shadowPoints[i] = new PointF(points[i].X + s * 0.03f, points[i].Y + s * 0.03f);
            }
            shadowPath.AddPolygon(shadowPoints);
            using var shadowBrush = new SolidBrush(Shadow);
            g.FillPath(shadowBrush, shadowPath);
        }

        // 渐变填充
        using (var gradBrush = new LinearGradientBrush(
            new PointF(center, s * 0.05f),
            new PointF(center, s * 0.95f),
            NeonPurple, NeonPurpleDark))
        {
            g.FillPolygon(gradBrush, points);
        }

        // 切面高光效果
        using (var facetBrush = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
        {
            // 左侧切面
            var leftFacet = new PointF[]
            {
                new(center, s * 0.05f),
                new(s * 0.15f, s * 0.35f),
                new(s * 0.3f, s * 0.35f),
            };
            g.FillPolygon(facetBrush, leftFacet);

            // 顶部切面
            var topFacet = new PointF[]
            {
                new(center, s * 0.05f),
                new(s * 0.7f, s * 0.35f),
                new(center, s * 0.25f),
            };
            g.FillPolygon(facetBrush, topFacet);
        }

        // 内部切面线 - 使用渐变
        using (var innerPen = new Pen(Color.FromArgb(150, NeonCyan), strokeW * 0.5f))
        {
            g.DrawLine(innerPen, center, s * 0.05f, s * 0.3f, s * 0.35f);
            g.DrawLine(innerPen, center, s * 0.05f, s * 0.7f, s * 0.35f);
            g.DrawLine(innerPen, s * 0.15f, s * 0.35f, s * 0.85f, s * 0.35f);
            g.DrawLine(innerPen, s * 0.3f, s * 0.35f, s * 0.25f, s * 0.95f);
            g.DrawLine(innerPen, s * 0.7f, s * 0.35f, s * 0.75f, s * 0.95f);
            g.DrawLine(innerPen, s * 0.3f, s * 0.35f, center, s * 0.65f);
            g.DrawLine(innerPen, s * 0.7f, s * 0.35f, center, s * 0.65f);
            g.DrawLine(innerPen, center, s * 0.65f, s * 0.25f, s * 0.95f);
            g.DrawLine(innerPen, center, s * 0.65f, s * 0.75f, s * 0.95f);
        }

        // 轮廓
        using (var glowPen = new Pen(Color.FromArgb(220, NeonPurple), strokeW) { LineJoin = LineJoin.Round })
        {
            g.DrawPolygon(glowPen, points);
        }

        // 顶部高光
        var highlightRect = new RectangleF(center - s * 0.08f, s * 0.08f, s * 0.16f, s * 0.12f);
        using (var highlightBrush = new SolidBrush(Highlight))
        {
            g.FillEllipse(highlightBrush, highlightRect);
        }

        return bmp;
    }

    private static void SetHighQuality(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.Clear(Color.Transparent);
    }
}
