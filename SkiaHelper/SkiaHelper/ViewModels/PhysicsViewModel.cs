using SkiaHelper.Interfaces;
using SkiaHelper.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using System.Numerics;

namespace SkiaHelper.ViewModels;

public class PhysicsViewModel : BaseViewModel
{
    private Vector2 curScreenCenterVec2;
    private List<IGraphicObject> sceneObjs = new();

    private readonly SKGLView _skglView;

    public PhysicsViewModel(SKGLView skglView)
    {
        skglView.PaintSurface += OnSkglView_PaintSurface;
        //skglView.Touch += SkglView_Touch;
        //skglView.EnableTouchEvents = true;

        _skglView = skglView;

        CreateObjs();
    }

    private void CreateObjs()
    {
        BallObj ball = new(0f, 0f, new SKPaint() { Color = SKColors.White });

        sceneObjs.Add(ball);
    }

    private void OnSkglView_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var screenLength = e.Surface.Canvas.DeviceClipBounds.Width;
        var screenHeight = e.Surface.Canvas.DeviceClipBounds.Height;

        curScreenCenterVec2 = new Vector2(screenLength / 2, screenHeight / 2);

        canvas.Clear(new SKColor(33, 33, 33));

        for (int i = 0; i < sceneObjs.Count; i++)
        {
            sceneObjs[i].HandleAttributes(10f);
            sceneObjs[i].Render(in canvas);
        }
    }
}
