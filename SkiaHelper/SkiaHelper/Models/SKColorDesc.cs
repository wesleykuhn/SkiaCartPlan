using Xamarin.Forms;

namespace SkiaHelper.Models;

public class SKColorDesc
{
    public string Name { get; set; }
    public Color ForeColor { get; set; }
    public Color Color { get; set; }

    public SKColorDesc(string name, Color color)
    {
        Name = name;
        Color = color;

        ForeColor = color.R + color.G + color.B > 1.3 ? Color.Black : Color.White;
    }
}
