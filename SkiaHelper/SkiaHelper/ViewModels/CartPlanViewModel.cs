using SkiaHelper.Calcs;
using SkiaHelper.Converters;
using SkiaHelper.Extensions;
using SkiaHelper.Models;
using SkiaHelper.Pages;
using SkiaHelper.ViewModelArgs;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace SkiaHelper.ViewModels;

public  class CartPlanViewModel : BaseViewModel
{
    #region [ CONSTS ]

    public const short MaxCartPlanScale = 500;
    public const byte MinCartPlanScale = 8;

    #endregion

    #region [ BIND PROPS ]

    private double curZoomPerc = 10;
    public double CurZoomPerc
    {
        get => curZoomPerc;
        set => SetProperty(ref curZoomPerc, value);
    }

    private string typedYPos;
    public string TypedYPos
    {
        get => typedYPos;
        set => SetProperty(ref typedYPos, value);
    }

    private string typedXPos;
    public string TypedXPos
    {
        get => typedXPos;
        set => SetProperty(ref typedXPos, value);
    }

    private bool showAddVecEntries;
    public bool ShowAddVecEntries
    {
        get => showAddVecEntries;
        set => SetProperty(ref showAddVecEntries, value);
    }

    #endregion

    #region [ FIELDS ]

    private MathOperationArgs mathOperationArgs;

    private short curCartPlanScale;
    private float curVecsHeadScale = 0.4f;
    private float curVecsLabelRadProp = 0.3f;

    private Vector2 curScreenCenterVec2;

    private List<HybridVec2> curCartPlanVectors = new();

    #endregion

    #region [ STATIC FIELDS ]

    private static readonly SKColor[] vectorColors = new SKColor[]
    {
        SKColors.Gold,
        SKColors.Turquoise,
        SKColors.DarkViolet,
        SKColors.Violet,
        SKColors.Coral,
        SKColors.BlueViolet,
        SKColors.Cyan,
        SKColors.DarkOrange,
        SKColors.CadetBlue,
        SKColors.LawnGreen,
        SKColors.Magenta,
        SKColors.OrangeRed,
        SKColors.Sienna,
        SKColors.Salmon,
        SKColors.Purple,
        SKColors.Blue,
        SKColors.Orange,
        SKColors.AliceBlue,
        SKColors.Tomato,
        SKColors.Silver,
        SKColors.Chocolate,
        SKColors.CornflowerBlue,
        SKColors.DarkKhaki,
        SKColors.DarkSeaGreen,
        SKColors.DodgerBlue,
        SKColors.Olive,
        SKColors.RosyBrown,
        SKColors.Tan
    };

    private static readonly char[] letters = new char[]
    {
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z'
    };

    private static readonly Mat2 _inverseTm = new(-1f, 0f, 0f, -1f);
    private static readonly Mat2 _cWise40degTm = new(.766f, -.6428f, .6428f, .766f);
    private static readonly Mat2 _counterCWise40degTm = new(.766f, .6428f, -.6428f, .766f);

    #endregion

    #region [ CMDS ]

    private ICommand plusButtonCommand;
    public ICommand PlusButtonCommand =>
        plusButtonCommand ??= new AsyncCommand(PlusButtonCommandExecute);

    private ICommand zoomBarReleasedCommand;
    public ICommand ZoomBarReleasedCommand =>
        zoomBarReleasedCommand ??= new Command(ZoomBarReleasedCommandExecute);

    private ICommand openCalcButnsPopupCommand;
    public ICommand OpenCalcButnsPopupCommand =>
        openCalcButnsPopupCommand ??= new AsyncCommand(OpenCalcButnsPopupCommandExecute, allowsMultipleExecutions: false);

    #endregion

    #region [ CTOR ]

    private readonly SKGLView _skglView;

    public CartPlanViewModel(SKGLView skglView)
    {
        skglView.PaintSurface += OnSkgl_PaintSurface;
        skglView.Touch += SkglView_Touch;
        skglView.EnableTouchEvents = true;

        _skglView = skglView;

        ZoomBarReleasedCommandExecute();
    }

    #endregion

    #region [ EVENTS ]

    private void OnSkgl_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintGLSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var screenLength = e.Surface.Canvas.DeviceClipBounds.Width;
        var screenHeight = e.Surface.Canvas.DeviceClipBounds.Height;

        curScreenCenterVec2 = new Vector2(screenLength / 2, screenHeight / 2);

        canvas.Clear(new SKColor(33, 33, 33));

        DrawCartPlane(in canvas, in screenLength, in screenHeight, in curScreenCenterVec2, in curCartPlanScale);

        if (!curCartPlanVectors.Any())
            return;

        DrawVectors(in curCartPlanVectors, in canvas, in curScreenCenterVec2, in curVecsHeadScale, in curCartPlanScale, in curVecsLabelRadProp);
    }

    #endregion

    #region [ CMDS Exes ]

    private async Task PlusButtonCommandExecute()
    {
        if (!ShowAddVecEntries)
        {
            ShowAddVecEntries = true;
        }
        else
        {
            if (TypedXPos.IsNullEmptyOrWhiteSpace() || TypedYPos.IsNullEmptyOrWhiteSpace())
            {
                await DisplayAlert("Error", "Coordinates fields are blank! Please, check.", "Ok");
                return;
            }

            var successX = float.TryParse(TypedXPos, out var parsedX);
            var successY = float.TryParse(TypedYPos, out var parsedY);

            if (!successX || !successY)
            {
                await DisplayAlert("Error", "Invalid typed coordinates! Please, check.", "Ok");
                return;
            }

            HybridVec2 newVec = new(new Vector2(parsedX, parsedY));

            curCartPlanVectors.Add(newVec);

            ShowAddVecEntries = false;

            TypedXPos = null;
            TypedYPos = null;

            _skglView.InvalidateSurface();
        }
    }

    private void ZoomBarReleasedCommandExecute()
    {
        var newScale = Convert.ToInt16((MaxCartPlanScale - MinCartPlanScale) / (100 / CurZoomPerc) + MinCartPlanScale);

        curCartPlanScale = newScale;

        _skglView.InvalidateSurface();
    }

    private async Task OpenCalcButnsPopupCommandExecute()
    {
        var selected = curCartPlanVectors.Where(w => w.IsSelected).ToList();

        if (selected.Count < 2)
            return;

        mathOperationArgs = new();
        mathOperationArgs.Vectors = selected;
        mathOperationArgs.OnCompletedCallback += OnMathOpsPageClosed;

        await Shell.Current.Navigation.PushModalAsync(new MathOperationsPage(mathOperationArgs));
    }

    #endregion

    #region [ TOUCH EVENT ]

    private void SkglView_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
    {
        if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Released)
        {
            var touched = HandleVectorTouchIfAny(in curCartPlanVectors, e.Location.X, e.Location.Y, in curVecsLabelRadProp, in curCartPlanScale);
            if (touched)
                _skglView.InvalidateSurface();
        }
        else if (e.WheelDelta != 0) //FOR PC
        {
            float newScale;
            if (e.WheelDelta < -100)
                newScale = -100;
            else if (e.WheelDelta > 100)
                newScale = 100;
            else
                newScale = e.WheelDelta;

            newScale = (float)(newScale * 0.1f);

            newScale += curCartPlanScale;

            if (newScale > MaxCartPlanScale)
                newScale = MaxCartPlanScale;
            else if (newScale < MinCartPlanScale)
                newScale = MinCartPlanScale;

            curCartPlanScale = Convert.ToInt16(newScale);

            _skglView.InvalidateSurface();
        }

        e.Handled = true;
    }

    private static bool HandleVectorTouchIfAny(
        in List<HybridVec2> vecs,
        in float touchedScreenX,
        in float touchedScreenY,
        in float curVecsLabelRadProp,
        in short curCartPlanScale)
    {
        for (int i = 0; i < vecs.Count; i++)
        {
            if (touchedScreenX > vecs[i].LabelCenterScreenPosition.X - curVecsLabelRadProp * curCartPlanScale &&
                touchedScreenX < vecs[i].LabelCenterScreenPosition.X + curVecsLabelRadProp * curCartPlanScale &&
                touchedScreenY > vecs[i].LabelCenterScreenPosition.Y - curVecsLabelRadProp * curCartPlanScale &&
                touchedScreenY < vecs[i].LabelCenterScreenPosition.Y + curVecsLabelRadProp * curCartPlanScale)
            {
                vecs[i].IsSelected = !vecs[i].IsSelected;
                return true;
            }
        }

        return false;
    }

    #endregion

    #region [ CART PLAN DRAWING ]

    private static void DrawCartPlane(in SKCanvas canvas, in int screenLength, in int screenHeight, in Vector2 screenCenter, in short curCartPlanScale)
    {
        using var paint = new SKPaint()
        {
            Color = new SKColor(255, 255, 255, 60),
            IsAntialias = true
        };

        using var gPaint = new SKPaint()
        {
            Color = SKColors.LimeGreen,
            IsAntialias = true
        };

        using var rPaint = new SKPaint()
        {
            Color = SKColors.Crimson,
            IsAntialias = true
        };

        //X axis line
        canvas.DrawLine(0f, screenCenter.Y, screenLength, screenCenter.Y, rPaint);

        //Y axis line
        canvas.DrawLine(screenCenter.X, 0f, screenCenter.X, screenHeight, gPaint);

        //From center to bottom
        for (float i = screenCenter.Y + curCartPlanScale; i <= screenHeight; i += curCartPlanScale)
            canvas.DrawLine(0f, i, screenLength, i, paint);

        //From center to top
        for (float i = screenCenter.Y - curCartPlanScale; i >= 0f; i -= curCartPlanScale)
            canvas.DrawLine(0f, i, screenLength, i, paint);

        //From center to left
        for (float i = screenCenter.X - curCartPlanScale; i >= 0f; i -= curCartPlanScale)
            canvas.DrawLine(i, 0f, i, screenHeight, paint);

        //From center to right
        for (float i = screenCenter.X + curCartPlanScale; i <= screenLength; i += curCartPlanScale)
            canvas.DrawLine(i, 0f, i, screenHeight, paint);
    }

    #endregion

    #region [ VECTORS DRAWING ]

    private static void DrawVectors(
        in List<HybridVec2> vectors,
        in SKCanvas canvas,
        in Vector2 screenCenter,
        in float curVectorsHeadScale,
        in short curCartPlanScale,
        in float curVecsLabelRadProp)
    {
        for (int i = 0; i < vectors.Count; i++)
        {
            var screenPosX = vectors[i].CartPlanPos.X * (curCartPlanScale - 1);
            var screenPosY = vectors[i].CartPlanPos.Y * (curCartPlanScale - 1) * -1;

            canvas.Save();

            canvas.Translate(screenCenter.X, screenCenter.Y);

            using var paint = new SKPaint()
            {
                Color = vectorColors[i],
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2,
            };

            canvas.DrawLine(vectors[i].CartPlanCenterPos.X, vectors[i].CartPlanCenterPos.Y, screenPosX, screenPosY, paint);

            if (vectors[i].IsSelected)
            {
                using var shPaint = new SKPaint()
                {
                    Color = vectorColors[i],
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 6,
                    MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 5f)
                };

                canvas.DrawLine(0f, 0f, screenPosX, screenPosY, shPaint);
            }

            canvas.Restore();

            DrawVectorHeadAndLabel(vectors[i], paint.Color, canvas, screenCenter, curVectorsHeadScale, curCartPlanScale, in i, curVecsLabelRadProp);
        }
    }

    #endregion

    #region [ MATHS ]

    private void OnMathOpsPageClosed()
    {
        var newVec = new HybridVec2(mathOperationArgs.ResultVec);

        curCartPlanVectors.Add(newVec);

        _skglView.InvalidateSurface();
    }

    #endregion

    #region [ OTHER DRAWINGS ]

    private static void DrawVectorHeadAndLabel(
        HybridVec2 vec,
        in SKColor vecColor,
        in SKCanvas canvas,
        in Vector2 screenCenter,
        in float curVectorsHeadScale,
        in short curCartPlanScale,
        in int vecIndex,
        in float curVecsLabelRadProp)
    {
        var isFacingDownDir = vec.CartPlanPos.Y < 0;

        var nVec = Vector2.Normalize(vec.CartPlanPos);
        var invNVec = VecMatMaths.TransformVector2(_inverseTm, nVec);

        var cWise40N = VecMatMaths.TransformVector2(_cWise40degTm, in invNVec);
        var counterCWise40N = VecMatMaths.TransformVector2(_counterCWise40degTm, in invNVec);

        var screenCWise40N = Vector2.Multiply(cWise40N, curCartPlanScale);
        var screenCounterCWise40N = Vector2.Multiply(counterCWise40N, curCartPlanScale);

        var regVecScreenPos = Vector2.Multiply(vec.CartPlanPos, curCartPlanScale);

        screenCounterCWise40N = Vector2.Multiply(screenCounterCWise40N, curVectorsHeadScale);
        var cWiseAddedToVecScreePos = Vector2.Add(regVecScreenPos, Vector2.Multiply(screenCWise40N, curVectorsHeadScale));

        var labelDistToVec = isFacingDownDir ? -0.45f * curCartPlanScale : 0.45f * curCartPlanScale;

        var labelCenterScrPos = CoordsConverter.CartPlanCoordToScreenPos(vec.CartPlanPos, screenCenter, curCartPlanScale);
        vec.LabelCenterScreenPosition = new(labelCenterScrPos.X, labelCenterScrPos.Y - labelDistToVec);

        var regLabelScreenPos = Vector2.Add(regVecScreenPos, new Vector2(0f, labelDistToVec));

        vec.VectorIdentification = CreateVecIndentification(vecIndex);

        canvas.Save();

        canvas.Translate(screenCenter.X, screenCenter.Y);

        using var paint = new SKPaint()
        {
            Color = vecColor,
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        using var fontPaint = new SKPaint();
        fontPaint.Color = vecColor.Blue + vecColor.Red + vecColor.Green > 340 ? SKColors.Black : SKColors.White;
        fontPaint.IsAntialias = true;
        fontPaint.TextAlign = SKTextAlign.Center;
        fontPaint.TextSize = curCartPlanScale * 0.4f;
        fontPaint.Typeface = SKTypeface.FromFamilyName("Arial");

        var textMeas = new SKRect();
        fontPaint.MeasureText(vec.VectorIdentification, ref textMeas);

        using var path = new SKPath();
        path.MoveTo(regVecScreenPos.X, regVecScreenPos.Y * -1);
        path.RLineTo(screenCounterCWise40N.X, screenCounterCWise40N.Y * -1);
        path.LineTo(cWiseAddedToVecScreePos.X, cWiseAddedToVecScreePos.Y * -1);
        path.Close();

        canvas.DrawPath(path, paint);

        if (vec.IsSelected)
        {
            using var shPaint = new SKPaint()
            {
                Color = vectorColors[vecIndex],
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 5f)
            };

            canvas.DrawPath(path, shPaint);
        }

        canvas.DrawCircle(regLabelScreenPos.X, regLabelScreenPos.Y * -1, curCartPlanScale * curVecsLabelRadProp, paint);

        canvas.DrawText(vec.VectorIdentification, regLabelScreenPos.X, (regLabelScreenPos.Y * -1) + curCartPlanScale * 0.15f, fontPaint);

        canvas.Restore();
    }

    private static string CreateVecIndentification(in int vecIndex)
    {
        return letters[vecIndex].ToString();
    }

    #endregion
}
