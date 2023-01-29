using System.Numerics;

namespace SkiaHelper.Models;

public struct Mat2
{
    public float A { get; set; }
    public float B { get; set; }
    public float C { get; set; }
    public float D { get; set; }

    public Mat2(float a, float b, float c, float d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public Mat2(Vector2 ab, Vector2 cd)
    {
        A = ab.X;
        B = ab.Y;
        C = cd.X;
        D = cd.Y;
    }
}
