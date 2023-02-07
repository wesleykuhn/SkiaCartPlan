using System.Numerics;

namespace SkiaHelper.Calcs;

public static class Lerp
{
    public static Vector2 GetDtimeLerpMovToGoal(in Vector2 goal, in Vector2 current, in float deltaTime, in float velocity)
    {
        var diff = Vector2.Subtract(goal, current);
        var proj = Vector2.Add(current, Vector2.Multiply(Vector2.Normalize(goal), deltaTime * velocity));

        if (diff.LengthSquared() > proj.LengthSquared())
            return proj;
        else
            return goal;
    }
}
