using System.Numerics;

namespace SkiaHelper.Converters;

public static class CoordsConverter
{
    /// <summary>
    /// Converts screen/window coordinates to cartesian coordinates (Center of screen is 0,0). Not scaled!
    /// </summary>
    /// <param name="screenX">Screen point X.</param>
    /// <param name="screenY">Screen point Y.</param>
    /// <param name="screenCenter">Center of the screen point.</param>
    /// <returns>Vector2 with the conversion.</returns>
    public static Vector2 ScreenCoordinatesToUnscCartPlanCoordinates(float screenX, float screenY, in Vector2 screenCenter)
    {
        return new Vector2(screenX - screenCenter.X, screenCenter.Y - screenY);
    }

    public static void ScaleCoordinatesToCartPlan(ref Vector2 vector2, in short scale)
    {
        vector2.X /= scale;
        vector2.Y /= scale;
    }
}
