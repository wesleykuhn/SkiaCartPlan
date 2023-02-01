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

    /// <summary>
    /// Gets a vector that contains cart plan coordinates and converts to screen position.
    /// </summary>
    /// <param name="targetVec">The vector that will be converted.</param>
    /// <param name="screenCenter">The current center position of the screen.</param>
    /// <param name="curCartPlanScale">The current scale of the cart plan.</param>
    /// <returns>The converted position. Note that is the absolute position of the screen, wich has no negative values!</returns>
    public static Vector2 CartPlanCoordToScreenPos(in Vector2 targetVec, in Vector2 screenCenter, in short curCartPlanScale)
    {
        return new(screenCenter.X + targetVec.X * curCartPlanScale, screenCenter.Y + targetVec.Y * curCartPlanScale * -1);
    }
}
