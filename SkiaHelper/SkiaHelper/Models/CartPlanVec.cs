using System.Numerics;

namespace SkiaHelper.Models;

public class CartPlanVec
{
    public string VectorIdentification { get; set; }
    public bool IsSelected { get; set; }

    public Vector2 CartPlanPos { get; set; }
    public Vector2 CartPlanCenterPos { get; set; } = new Vector2(0f, 0f);
    public Vector2 LabelCenterScreenPosition { get; set; }

    //Below is used to handle moviment
    public Vector2 MovGoal { get; set; }
    public Vector2 MovCurrent { get; set; }
    public float MovVel { get; set; }

    public CartPlanVec(Vector2 cartPlanPos)
    {
        CartPlanPos = cartPlanPos;
    }
}
