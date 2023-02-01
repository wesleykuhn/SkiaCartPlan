using System.Numerics;

namespace SkiaHelper.Models
{
    public class HybridVec2
    {
        public string VectorIdentification { get; set; }
        public bool IsSelected { get; set; }

        public Vector2 CartPlanPos { get; set; }
        public Vector2 CartPlanCenterPos { get; set; } = new Vector2(0f, 0f);
        public Vector2 LabelCenterScreenPosition { get; set; }

        public HybridVec2(Vector2 cartPlanPos)
        {
            CartPlanPos = cartPlanPos;
        }
    }
}
