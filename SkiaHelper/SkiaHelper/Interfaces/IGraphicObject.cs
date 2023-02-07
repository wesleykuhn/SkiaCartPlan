using SkiaSharp;

namespace SkiaHelper.Interfaces;

public interface IGraphicObject
{
    /// <summary>
    /// Handles moviment, speed, colour, direction, etc.
    /// </summary>
    /// <param name="sceneGravity">The current gravity of the scene.</param>
    void HandleAttributes(in float sceneGravity);

    /// <summary>
    /// Drawn the object on the canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    void Render(in SKCanvas canvas);
}
