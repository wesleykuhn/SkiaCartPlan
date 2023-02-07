using SkiaSharp;
using System.Numerics;

namespace SkiaHelper.Models;

public abstract class BaseObj
{
    public Vector2 Position { get; protected set; }
    public SKPaint Paint { get; protected set; }
}
