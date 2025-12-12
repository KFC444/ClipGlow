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
        CatPaw,      // 猫爪
        Snowflake,   // 雪花
        Bolt,        // 闪电
        Sun,         // 太阳
        Star,        // 星星
        Hexagon,     // 六边形
        Leaf,        // 叶子
        Moon,        // 月亮
        Circuit,     // 电路
        Diamond,     // 钻石
        // 像素风图标
        PixelHeart,  // 像素爱心
        PixelRobot,  // 像素机器人
        PixelGhost,  // 像素幽灵
        PixelSlime,  // 像素史莱姆
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
        IconStyle.PixelHeart => "像素爱心",
        IconStyle.PixelRobot => "像素机器人",
        IconStyle.PixelGhost => "像素幽灵",
        IconStyle.PixelSlime => "像素史莱姆",
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
            IconStyle.PixelHeart => DrawPixelHeart(size),
            IconStyle.PixelRobot => DrawPixelRobot(size),
            IconStyle.PixelGhost => DrawPixelGhost(size),
            IconStyle.PixelSlime => DrawPixelSlime(size),
            _ => DrawCatPaw(size)
        };
    }

    #region 配色常量

    private static readonly Color NeonCyan = Color.FromArgb(0, 255, 255);
    private static readonly Color NeonCyanDark = Color.FromArgb(0, 180, 200);
    private static readonly Color NeonBlue = Color.FromArgb(0, 150, 255);
    private static readonly Color NeonPurple = Color.FromArgb(180, 100, 255);
    private static readonly Color NeonPurpleDark = Color.FromArgb(120, 60, 200);
    private static readonly Color NeonGreen = Color.FromArgb(0, 255, 150);
    private static readonly Color NeonGreenDark = Color.FromArgb(0, 180, 100);
    private static readonly Color NeonOrange = Color.FromArgb(255, 150, 0);
    private static readonly Color NeonYellow = Color.FromArgb(255, 255, 100);
    private static readonly Color NeonYellowDark = Color.FromArgb(200, 200, 0);
    private static readonly Color Highlight = Color.FromArgb(120, 255, 255, 255);
    private static readonly Color Shadow = Color.FromArgb(80, 0, 0, 0);

    #endregion

    #region 公共绘制方法

    private static void SetHighQuality(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.Clear(Color.Transparent);
    }

    /// <summary>
    /// 绘制椭圆阴影
    /// </summary>
    private static void DrawEllipseShadow(Graphics g, RectangleF rect, float offset)
    {
        var shadowRect = new RectangleF(rect.X + offset, rect.Y + offset, rect.Width, rect.Height);
        using var shadowBrush = new SolidBrush(Shadow);
        g.FillEllipse(shadowBrush, shadowRect);
    }

    /// <summary>
    /// 绘制多边形阴影
    /// </summary>
    private static void DrawPolygonShadow(Graphics g, PointF[] points, float offset)
    {
        var shadowPoints = new PointF[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            shadowPoints[i] = new PointF(points[i].X + offset, points[i].Y + offset);
        }
        using var shadowBrush = new SolidBrush(Shadow);
        g.FillPolygon(shadowBrush, shadowPoints);
    }

    /// <summary>
    /// 绘制椭圆高光
    /// </summary>
    private static void DrawEllipseHighlight(Graphics g, RectangleF parentRect, float xRatio, float yRatio, float wRatio, float hRatio)
    {
        var highlightRect = new RectangleF(
            parentRect.X + parentRect.Width * xRatio,
            parentRect.Y + parentRect.Height * yRatio,
            parentRect.Width * wRatio,
            parentRect.Height * hRatio
        );
        using var highlightBrush = new SolidBrush(Highlight);
        g.FillEllipse(highlightBrush, highlightRect);
    }

    /// <summary>
    /// 渐变填充椭圆
    /// </summary>
    private static void FillEllipseGradient(Graphics g, RectangleF rect, Color c1, Color c2, float angle = 45f)
    {
        using var gradBrush = new LinearGradientBrush(rect, c1, c2, angle);
        g.FillEllipse(gradBrush, rect);
    }

    /// <summary>
    /// 渐变填充多边形
    /// </summary>
    private static void FillPolygonGradient(Graphics g, PointF[] points, PointF start, PointF end, Color c1, Color c2)
    {
        using var gradBrush = new LinearGradientBrush(start, end, c1, c2);
        g.FillPolygon(gradBrush, points);
    }

    /// <summary>
    /// 绘制椭圆轮廓
    /// </summary>
    private static void DrawEllipseOutline(Graphics g, RectangleF rect, Color color, float width)
    {
        using var pen = new Pen(Color.FromArgb(200, color), width);
        g.DrawEllipse(pen, rect);
    }

    /// <summary>
    /// 绘制多边形轮廓
    /// </summary>
    private static void DrawPolygonOutline(Graphics g, PointF[] points, Color color, float width)
    {
        using var pen = new Pen(Color.FromArgb(220, color), width) { LineJoin = LineJoin.Round };
        g.DrawPolygon(pen, points);
    }

    /// <summary>
    /// 绘制小星星装饰
    /// </summary>
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
    /// 创建渐变画笔线条
    /// </summary>
    private static Pen CreateGradientPen(PointF start, PointF end, Color c1, Color c2, float width)
    {
        var brush = new LinearGradientBrush(start, end, c1, c2);
        return new Pen(brush, width) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    }

    #endregion

    #region 图标绘制方法

    /// <summary>
    /// 猫爪
    /// </summary>
    private static Bitmap DrawCatPaw(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float strokeW = Math.Max(2f, s * 0.065f);
        float shadowOffset = s * 0.02f;

        // 主肉垫
        var mainRect = new RectangleF(s * 0.2f, s * 0.42f, s * 0.6f, s * 0.48f);
        DrawEllipseShadow(g, mainRect, shadowOffset);
        FillEllipseGradient(g, mainRect, NeonCyan, NeonCyanDark);
        DrawEllipseHighlight(g, mainRect, 0.15f, 0.1f, 0.4f, 0.3f);
        DrawEllipseOutline(g, mainRect, NeonCyan, strokeW);

        // 四个小肉垫
        float[,] pads = {
            { 0.13f, 0.15f, 0.22f },
            { 0.32f, 0.05f, 0.20f },
            { 0.52f, 0.05f, 0.20f },
            { 0.69f, 0.15f, 0.22f },
        };

        for (int i = 0; i < 4; i++)
        {
            var padRect = new RectangleF(s * pads[i, 0], s * pads[i, 1], s * pads[i, 2], s * pads[i, 2]);
            DrawEllipseShadow(g, padRect, s * 0.015f);
            FillEllipseGradient(g, padRect, NeonCyan, NeonCyanDark);
            DrawEllipseHighlight(g, padRect, 0.2f, 0.15f, 0.35f, 0.25f);
            DrawEllipseOutline(g, padRect, NeonCyan, strokeW * 0.8f);
        }

        return bmp;
    }

    /// <summary>
    /// 雪花
    /// </summary>
    private static Bitmap DrawSnowflake(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.06f);

        // 6个主分支
        for (int i = 0; i < 6; i++)
        {
            double angle = Math.PI / 3 * i;
            float x1 = center + (float)(Math.Cos(angle) * s * 0.4);
            float y1 = center + (float)(Math.Sin(angle) * s * 0.4);

            using var branchPen = CreateGradientPen(new PointF(center, center), new PointF(x1, y1), NeonCyan, NeonBlue, strokeW);
            g.DrawLine(branchPen, center, center, x1, y1);
            branchPen.Brush.Dispose();

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

                using var sidePen = CreateGradientPen(new PointF(bx, by), new PointF(bx2, by2), NeonCyan, NeonCyanDark, strokeW * 0.6f);
                g.DrawLine(sidePen, bx, by, bx2, by2);
                sidePen.Brush.Dispose();
            }
        }

        // 中心
        float cr = s * 0.06f;
        var centerRect = new RectangleF(center - cr, center - cr, cr * 2, cr * 2);
        FillEllipseGradient(g, centerRect, NeonCyan, NeonBlue);
        DrawEllipseHighlight(g, centerRect, 0.15f, 0.1f, 0.5f, 0.35f);

        return bmp;
    }

    /// <summary>
    /// 闪电
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

        DrawPolygonShadow(g, points, s * 0.03f);
        FillPolygonGradient(g, points, new PointF(s * 0.55f, s * 0.05f), new PointF(s * 0.32f, s * 0.95f), NeonYellow, NeonOrange);

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

        DrawPolygonOutline(g, points, NeonYellow, strokeW);
        DrawSparkle(g, s * 0.2f, s * 0.3f, s * 0.06f, NeonYellow);
        DrawSparkle(g, s * 0.75f, s * 0.6f, s * 0.07f, NeonYellow);

        return bmp;
    }

    /// <summary>
    /// 太阳
    /// </summary>
    private static Bitmap DrawSun(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2.2f, s * 0.065f);

        // 8条光芒
        for (int i = 0; i < 8; i++)
        {
            double angle = Math.PI / 4 * i;
            float x1 = center + (float)(Math.Cos(angle) * s * 0.28);
            float y1 = center + (float)(Math.Sin(angle) * s * 0.28);
            float x2 = center + (float)(Math.Cos(angle) * s * 0.44);
            float y2 = center + (float)(Math.Sin(angle) * s * 0.44);

            using var rayPen = CreateGradientPen(new PointF(x1, y1), new PointF(x2, y2), NeonOrange, NeonYellow, strokeW);
            g.DrawLine(rayPen, x1, y1, x2, y2);
            rayPen.Brush.Dispose();
        }

        float sr = s * 0.22f;
        var sunRect = new RectangleF(center - sr, center - sr, sr * 2, sr * 2);

        DrawEllipseShadow(g, sunRect, s * 0.02f);
        FillEllipseGradient(g, sunRect, NeonOrange, NeonYellow, 135f);
        DrawEllipseHighlight(g, sunRect, 0.15f, 0.1f, 0.45f, 0.35f);
        DrawEllipseOutline(g, sunRect, NeonOrange, strokeW * 0.8f);

        return bmp;
    }

    /// <summary>
    /// 星星
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
            points[i] = new PointF(center + (float)(Math.Cos(angle) * r), center + (float)(Math.Sin(angle) * r));
        }

        DrawPolygonShadow(g, points, s * 0.025f);
        FillPolygonGradient(g, points, new PointF(center, center - s * 0.42f), new PointF(center, center + s * 0.42f), NeonYellow, NeonYellowDark);

        // 中心高光
        var highlightSize = s * 0.15f;
        var highlightRect = new RectangleF(center - highlightSize / 2, center - s * 0.15f, highlightSize, highlightSize);
        using (var highlightBrush = new SolidBrush(Highlight))
        {
            g.FillEllipse(highlightBrush, highlightRect);
        }

        DrawPolygonOutline(g, points, NeonYellow, strokeW);
        DrawSparkle(g, center + s * 0.35f, center - s * 0.35f, s * 0.08f, NeonYellow);
        DrawSparkle(g, center - s * 0.38f, center + s * 0.32f, s * 0.06f, NeonYellow);

        return bmp;
    }

    /// <summary>
    /// 六边形
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
        var outerPoints = CreateHexagonPoints(center, center, s * 0.42f);

        DrawPolygonShadow(g, outerPoints, s * 0.025f);
        FillPolygonGradient(g, outerPoints, new PointF(center, center - s * 0.42f), new PointF(center, center + s * 0.42f), NeonPurple, NeonPurpleDark);

        // 顶部高光
        var highlightPoints = new PointF[]
        {
            outerPoints[0], outerPoints[1],
            new PointF(center + s * 0.15f, center - s * 0.05f),
            new PointF(center - s * 0.15f, center - s * 0.05f),
            outerPoints[5],
        };
        using (var highlightBrush = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
        {
            g.FillPolygon(highlightBrush, highlightPoints);
        }

        DrawPolygonOutline(g, outerPoints, NeonPurple, strokeW);

        // 内六边形
        var innerPoints = CreateHexagonPoints(center, center, s * 0.22f);
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

    private static PointF[] CreateHexagonPoints(float cx, float cy, float radius)
    {
        var points = new PointF[6];
        for (int i = 0; i < 6; i++)
        {
            double angle = Math.PI / 3 * i - Math.PI / 2;
            points[i] = new PointF(cx + (float)(Math.Cos(angle) * radius), cy + (float)(Math.Sin(angle) * radius));
        }
        return points;
    }

    /// <summary>
    /// 叶子
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
            var shadowMatrix = new Matrix();
            shadowMatrix.Translate(s * 0.025f, s * 0.025f);
            shadowPath?.Transform(shadowMatrix);
            using var shadowBrush = new SolidBrush(Shadow);
            if (shadowPath != null) g.FillPath(shadowBrush, shadowPath);
        }

        // 渐变填充
        var leafBounds = path.GetBounds();
        using (var gradBrush = new LinearGradientBrush(new PointF(center, leafBounds.Top), new PointF(center, leafBounds.Bottom), NeonGreen, NeonGreenDark))
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

        // 叶脉
        using (var veinPen = new Pen(Color.FromArgb(180, NeonCyan), strokeW * 0.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
        {
            g.DrawLine(veinPen, center, s * 0.18f, center, s * 0.82f);

            // 侧叶脉
            using var sideVeinPen = new Pen(Color.FromArgb(120, NeonCyan), strokeW * 0.4f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            for (float t = 0.3f; t <= 0.7f; t += 0.15f)
            {
                float y = s * (0.18f + t * 0.64f);
                float offset = s * 0.15f;
                g.DrawLine(sideVeinPen, center, y, center + offset, y + offset * 0.5f);
                g.DrawLine(sideVeinPen, center, y, center - offset, y + offset * 0.5f);
            }
        }

        return bmp;
    }

    /// <summary>
    /// 月亮
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
            var shadowMatrix = new Matrix();
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

        DrawSparkle(g, s * 0.75f, s * 0.25f, s * 0.06f, NeonCyan);
        DrawSparkle(g, s * 0.82f, s * 0.65f, s * 0.05f, NeonBlue);

        return bmp;
    }

    /// <summary>
    /// 电路
    /// </summary>
    private static Bitmap DrawCircuit(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.055f);

        // 电路线条
        using (var linePen = new Pen(Color.FromArgb(200, NeonCyan), strokeW) { StartCap = LineCap.Round, EndCap = LineCap.Round })
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

        // 节点
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
                g.FillEllipse(glowBrush, pos.X - nr * 1.5f, pos.Y - nr * 1.5f, nr * 3, nr * 3);
            }

            FillEllipseGradient(g, nodeRect, NeonGreen, NeonCyan);
            DrawEllipseHighlight(g, nodeRect, 0.1f, 0.05f, 0.4f, 0.3f);
        }

        return bmp;
    }

    /// <summary>
    /// 钻石
    /// </summary>
    private static Bitmap DrawDiamond(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        SetHighQuality(g);

        float s = size;
        float center = s / 2;
        float strokeW = Math.Max(2f, s * 0.06f);

        var points = new PointF[]
        {
            new(center, s * 0.05f),
            new(s * 0.85f, s * 0.35f),
            new(s * 0.75f, s * 0.95f),
            new(s * 0.25f, s * 0.95f),
            new(s * 0.15f, s * 0.35f),
        };

        DrawPolygonShadow(g, points, s * 0.03f);
        FillPolygonGradient(g, points, new PointF(center, s * 0.05f), new PointF(center, s * 0.95f), NeonPurple, NeonPurpleDark);

        // 切面高光
        using (var facetBrush = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
        {
            g.FillPolygon(facetBrush, new PointF[] { new(center, s * 0.05f), new(s * 0.15f, s * 0.35f), new(s * 0.3f, s * 0.35f) });
            g.FillPolygon(facetBrush, new PointF[] { new(center, s * 0.05f), new(s * 0.7f, s * 0.35f), new(center, s * 0.25f) });
        }

        // 内部切面线
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

        DrawPolygonOutline(g, points, NeonPurple, strokeW);

        // 顶部高光
        var highlightRect = new RectangleF(center - s * 0.08f, s * 0.08f, s * 0.16f, s * 0.12f);
        using (var highlightBrush = new SolidBrush(Highlight))
        {
            g.FillEllipse(highlightBrush, highlightRect);
        }

        return bmp;
    }

    #endregion

    #region 像素风图标绘制方法

    // 像素风专用颜色
    private static readonly Color PixelPink = Color.FromArgb(255, 105, 180);
    private static readonly Color PixelPinkDark = Color.FromArgb(200, 60, 140);
    private static readonly Color PixelBlue = Color.FromArgb(100, 180, 255);
    private static readonly Color PixelBlueDark = Color.FromArgb(60, 120, 200);
    private static readonly Color PixelGreen = Color.FromArgb(100, 255, 150);
    private static readonly Color PixelGreenDark = Color.FromArgb(50, 200, 100);
    private static readonly Color PixelOrange = Color.FromArgb(255, 180, 100);

    /// <summary>
    /// 绘制像素块
    /// </summary>
    private static void DrawPixel(Graphics g, float x, float y, float pixelSize, Color color)
    {
        using var brush = new SolidBrush(color);
        g.FillRectangle(brush, x, y, pixelSize, pixelSize);
    }

    /// <summary>
    /// 像素爱心 - 经典像素艺术风格
    /// </summary>
    private static Bitmap DrawPixelHeart(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);

        // 像素风关闭抗锯齿
        g.SmoothingMode = SmoothingMode.None;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.Clear(Color.Transparent);

        float s = size;
        float p = s / 12f; // 像素单位大小

        // 爱心像素图案 (12x12网格)
        // 使用相对坐标定义爱心形状
        var heartPixels = new (int x, int y)[]
        {
            // 第2行
            (2, 2), (3, 2), (8, 2), (9, 2),
            // 第3行
            (1, 3), (2, 3), (3, 3), (4, 3), (7, 3), (8, 3), (9, 3), (10, 3),
            // 第4行
            (1, 4), (2, 4), (3, 4), (4, 4), (5, 4), (6, 4), (7, 4), (8, 4), (9, 4), (10, 4),
            // 第5行
            (1, 5), (2, 5), (3, 5), (4, 5), (5, 5), (6, 5), (7, 5), (8, 5), (9, 5), (10, 5),
            // 第6行
            (2, 6), (3, 6), (4, 6), (5, 6), (6, 6), (7, 6), (8, 6), (9, 6),
            // 第7行
            (3, 7), (4, 7), (5, 7), (6, 7), (7, 7), (8, 7),
            // 第8行
            (4, 8), (5, 8), (6, 8), (7, 8),
            // 第9行
            (5, 9), (6, 9),
        };

        // 高光像素
        var highlightPixels = new (int x, int y)[]
        {
            (2, 3), (3, 3), (8, 3), (9, 3),
            (2, 4), (8, 4), (9, 4),
        };

        // 绘制爱心主体
        foreach (var (x, y) in heartPixels)
        {
            DrawPixel(g, x * p, y * p, p, PixelPink);
        }

        // 绘制高光
        foreach (var (x, y) in highlightPixels)
        {
            DrawPixel(g, x * p, y * p, p, Color.FromArgb(180, 255, 255, 255));
        }

        return bmp;
    }

    /// <summary>
    /// 像素机器人 - 可爱的小机器人
    /// </summary>
    private static Bitmap DrawPixelRobot(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);

        g.SmoothingMode = SmoothingMode.None;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.Clear(Color.Transparent);

        float s = size;
        float p = s / 12f;

        // 天线
        DrawPixel(g, 5 * p, 0 * p, p, PixelBlueDark);
        DrawPixel(g, 6 * p, 0 * p, p, PixelBlueDark);
        DrawPixel(g, 5 * p, 1 * p, p, PixelBlue);
        DrawPixel(g, 6 * p, 1 * p, p, PixelBlue);

        // 头部
        for (int x = 3; x <= 8; x++)
        {
            DrawPixel(g, x * p, 2 * p, p, PixelBlueDark); // 顶边
            DrawPixel(g, x * p, 6 * p, p, PixelBlueDark); // 底边
        }
        for (int y = 3; y <= 5; y++)
        {
            DrawPixel(g, 3 * p, y * p, p, PixelBlueDark); // 左边
            DrawPixel(g, 8 * p, y * p, p, PixelBlueDark); // 右边
        }

        // 头部填充
        for (int x = 4; x <= 7; x++)
        {
            for (int y = 3; y <= 5; y++)
            {
                DrawPixel(g, x * p, y * p, p, PixelBlue);
            }
        }

        // 眼睛
        DrawPixel(g, 4 * p, 4 * p, p, Color.White);
        DrawPixel(g, 7 * p, 4 * p, p, Color.White);
        DrawPixel(g, 4 * p, 4 * p, p * 0.6f, Color.Black); // 眼珠
        DrawPixel(g, 7 * p, 4 * p, p * 0.6f, Color.Black);

        // 身体
        for (int x = 4; x <= 7; x++)
        {
            DrawPixel(g, x * p, 7 * p, p, PixelBlueDark);
            DrawPixel(g, x * p, 10 * p, p, PixelBlueDark);
        }
        for (int y = 8; y <= 9; y++)
        {
            DrawPixel(g, 4 * p, y * p, p, PixelBlueDark);
            DrawPixel(g, 7 * p, y * p, p, PixelBlueDark);
        }

        // 身体填充
        for (int x = 5; x <= 6; x++)
        {
            for (int y = 8; y <= 9; y++)
            {
                DrawPixel(g, x * p, y * p, p, PixelBlue);
            }
        }

        // 胸口灯
        DrawPixel(g, 5 * p, 8 * p, p, PixelOrange);
        DrawPixel(g, 6 * p, 8 * p, p, PixelOrange);

        // 手臂
        DrawPixel(g, 2 * p, 8 * p, p, PixelBlueDark);
        DrawPixel(g, 3 * p, 8 * p, p, PixelBlue);
        DrawPixel(g, 9 * p, 8 * p, p, PixelBlueDark);
        DrawPixel(g, 8 * p, 8 * p, p, PixelBlue);

        // 腿
        DrawPixel(g, 4 * p, 11 * p, p, PixelBlueDark);
        DrawPixel(g, 5 * p, 11 * p, p, PixelBlueDark);
        DrawPixel(g, 6 * p, 11 * p, p, PixelBlueDark);
        DrawPixel(g, 7 * p, 11 * p, p, PixelBlueDark);

        return bmp;
    }

    /// <summary>
    /// 像素幽灵 - 吃豆人风格的可爱幽灵
    /// </summary>
    private static Bitmap DrawPixelGhost(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);

        g.SmoothingMode = SmoothingMode.None;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.Clear(Color.Transparent);

        float s = size;
        float p = s / 12f;

        var bodyColor = NeonCyan;
        var darkColor = NeonCyanDark;

        // 幽灵身体像素
        var ghostBody = new (int x, int y)[]
        {
            // 第1行 (顶部)
            (4, 1), (5, 1), (6, 1), (7, 1),
            // 第2行
            (3, 2), (4, 2), (5, 2), (6, 2), (7, 2), (8, 2),
            // 第3-8行 (主体)
            (2, 3), (3, 3), (4, 3), (5, 3), (6, 3), (7, 3), (8, 3), (9, 3),
            (2, 4), (3, 4), (4, 4), (5, 4), (6, 4), (7, 4), (8, 4), (9, 4),
            (2, 5), (3, 5), (4, 5), (5, 5), (6, 5), (7, 5), (8, 5), (9, 5),
            (2, 6), (3, 6), (4, 6), (5, 6), (6, 6), (7, 6), (8, 6), (9, 6),
            (2, 7), (3, 7), (4, 7), (5, 7), (6, 7), (7, 7), (8, 7), (9, 7),
            (2, 8), (3, 8), (4, 8), (5, 8), (6, 8), (7, 8), (8, 8), (9, 8),
            // 第9行 (底部波浪)
            (2, 9), (3, 9), (5, 9), (6, 9), (8, 9), (9, 9),
            // 第10行 (底部波浪)
            (2, 10), (5, 10), (6, 10), (9, 10),
        };

        // 绘制幽灵身体
        foreach (var (x, y) in ghostBody)
        {
            DrawPixel(g, x * p, y * p, p, bodyColor);
        }

        // 眼睛 (白色背景)
        DrawPixel(g, 3 * p, 4 * p, p * 2, Color.White);
        DrawPixel(g, 7 * p, 4 * p, p * 2, Color.White);

        // 眼珠 (蓝色)
        DrawPixel(g, 4 * p, 5 * p, p, Color.FromArgb(50, 50, 150));
        DrawPixel(g, 8 * p, 5 * p, p, Color.FromArgb(50, 50, 150));

        // 高光
        DrawPixel(g, 3 * p, 3 * p, p, Color.FromArgb(150, 255, 255, 255));
        DrawPixel(g, 4 * p, 3 * p, p, Color.FromArgb(100, 255, 255, 255));

        return bmp;
    }

    /// <summary>
    /// 像素史莱姆 - 可爱的果冻状小生物
    /// </summary>
    private static Bitmap DrawPixelSlime(int size)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);

        g.SmoothingMode = SmoothingMode.None;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.Clear(Color.Transparent);

        float s = size;
        float p = s / 12f;

        // 史莱姆身体像素
        var slimeBody = new (int x, int y)[]
        {
            // 第3行 (顶部)
            (4, 3), (5, 3), (6, 3), (7, 3),
            // 第4行
            (3, 4), (4, 4), (5, 4), (6, 4), (7, 4), (8, 4),
            // 第5-8行 (主体)
            (2, 5), (3, 5), (4, 5), (5, 5), (6, 5), (7, 5), (8, 5), (9, 5),
            (2, 6), (3, 6), (4, 6), (5, 6), (6, 6), (7, 6), (8, 6), (9, 6),
            (2, 7), (3, 7), (4, 7), (5, 7), (6, 7), (7, 7), (8, 7), (9, 7),
            (2, 8), (3, 8), (4, 8), (5, 8), (6, 8), (7, 8), (8, 8), (9, 8),
            // 第9行 (底部)
            (3, 9), (4, 9), (5, 9), (6, 9), (7, 9), (8, 9),
            // 第10行 (底部)
            (4, 10), (5, 10), (6, 10), (7, 10),
        };

        // 绘制史莱姆身体
        foreach (var (x, y) in slimeBody)
        {
            DrawPixel(g, x * p, y * p, p, PixelGreen);
        }

        // 边缘阴影
        var shadowPixels = new (int x, int y)[]
        {
            (2, 8), (9, 8), (3, 9), (8, 9), (4, 10), (7, 10),
        };
        foreach (var (x, y) in shadowPixels)
        {
            DrawPixel(g, x * p, y * p, p, PixelGreenDark);
        }

        // 眼睛 (可爱的大眼睛)
        DrawPixel(g, 4 * p, 6 * p, p, Color.White);
        DrawPixel(g, 5 * p, 6 * p, p, Color.White);
        DrawPixel(g, 7 * p, 6 * p, p, Color.White);
        DrawPixel(g, 8 * p, 6 * p, p, Color.White);

        // 眼珠
        DrawPixel(g, 5 * p, 6 * p, p * 0.7f, Color.Black);
        DrawPixel(g, 8 * p, 6 * p, p * 0.7f, Color.Black);

        // 眼睛高光
        DrawPixel(g, 4 * p, 6 * p, p * 0.4f, Color.White);
        DrawPixel(g, 7 * p, 6 * p, p * 0.4f, Color.White);

        // 嘴巴 (微笑)
        DrawPixel(g, 5 * p, 8 * p, p * 0.6f, PixelGreenDark);
        DrawPixel(g, 6 * p, 8 * p, p * 0.6f, PixelGreenDark);

        // 头顶高光
        DrawPixel(g, 4 * p, 4 * p, p, Color.FromArgb(180, 255, 255, 255));
        DrawPixel(g, 5 * p, 4 * p, p, Color.FromArgb(120, 255, 255, 255));

        return bmp;
    }

    #endregion
}
