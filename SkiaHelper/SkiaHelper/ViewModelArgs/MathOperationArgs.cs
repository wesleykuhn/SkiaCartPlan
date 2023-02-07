using SkiaHelper.Models;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SkiaHelper.ViewModelArgs;

public class MathOperationArgs
{
    public List<CartPlanVec> Vectors;

    public bool Add { get; set; }
    public bool Sub { get; set; }
    public bool Div { get; set; }
    public bool Mult { get; set; }

    public Vector2 ResultVec { get; set; }

    public Action OnCompletedCallback { get; set; }
}
