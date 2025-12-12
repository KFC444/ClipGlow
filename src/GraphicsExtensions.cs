using System.Drawing;
using System.Drawing.Drawing2D;

namespace CopyRelay;

/// <summary>
/// Graphics 扩展方法 - 圆角矩形等绘图辅助
/// </summary>
public static class GraphicsExtensions
{
    public static void FillRoundedRectangle(this Graphics g, Brush brush, float x, float y, float width, float height, float radius)
    {
        using var path = CreateRoundedRectangle(x, y, width, height, radius);
        g.FillPath(brush, path);
    }

    public static void DrawRoundedRectangle(this Graphics g, Pen pen, float x, float y, float width, float height, float radius)
    {
        using var path = CreateRoundedRectangle(x, y, width, height, radius);
        g.DrawPath(pen, path);
    }

    private static GraphicsPath CreateRoundedRectangle(float x, float y, float width, float height, float radius)
    {
        var path = new GraphicsPath();
        float diameter = radius * 2;

        path.AddArc(x, y, diameter, diameter, 180, 90);
        path.AddArc(x + width - diameter, y, diameter, diameter, 270, 90);
        path.AddArc(x + width - diameter, y + height - diameter, diameter, diameter, 0, 90);
        path.AddArc(x, y + height - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();

        return path;
    }
}
