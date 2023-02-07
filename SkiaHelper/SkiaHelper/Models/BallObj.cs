using SkiaHelper.Interfaces;
using SkiaSharp;
using System;
using System.Numerics;

namespace SkiaHelper.Models;

public class BallObj : BaseObj, IGraphicObject
{
    public BallObj(float xPos, float yPos, SKPaint paint)
    {
        Position = new(xPos, yPos);
        Paint = paint;
    }

    public void HandleAttributes(in float sceneGravity)
    {
        throw new NotImplementedException();
    }

    public void Render(in SKCanvas canvas)
    {
        throw new NotImplementedException();
    }
}
