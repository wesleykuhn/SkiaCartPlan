using SkiaHelper.Models;
using System.Numerics;

namespace SkiaHelper.Calcs;

public static class VecMatMaths
{
    public static Vector2 TransformVector2(in Mat2 transfMat, in Vector2 vector)
    {
        return new(vector.X * transfMat.A + vector.Y * transfMat.C, vector.X * transfMat.B + vector.Y * transfMat.D);
    }

    public static sbyte GetVecPointingSide(in Vector2 screenCenter, in float vecScreenX)
    {
        if (screenCenter.X == vecScreenX)
            return 0;

        if (vecScreenX < screenCenter.X)
            return -1;
        else
            return 1;
    }

    public static void SmoothMovVecToGoal(ref CartPlanVec vec, in float deltaTime)
    {
        vec.MovCurrent = Lerp.GetDtimeLerpMovToGoal(vec.MovGoal, vec.MovCurrent, deltaTime, vec.MovVel);
    }
}
