using System.Numerics;

namespace SkiaHelper.Models
{
    public class HybridVec2
    {
        public string VectorIdentification { get; set; }
        public bool IsSelected { get; set; }

        public Vector2 ScreenPos { get; set; }
        public Vector2 CartPlanPos { get; set; }
        public Vector2 LabelCenterScreenPosition { get; set; }

        public HybridVec2(Vector2 screenPos, Vector2 cartPlanPos)
        {
            ScreenPos = screenPos;
            CartPlanPos = cartPlanPos;
        }
    }
}
