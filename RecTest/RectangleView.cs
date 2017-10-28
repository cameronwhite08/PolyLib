﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using LowPolyLibrary;
using SkiaSharp;

namespace RecTest
{
    public class RectangleView : View
    {
        private cRectangleF.RectangleContainer rectangles;
        private int angle, numFrames, boundsWidth, boundsHeight;
        private float scale;
        Paint recPaint, screenPaint;

        public RectangleView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public RectangleView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            angle = 0;
            numFrames = 12;
            scale = .25f;

            recPaint = new Paint
            {
                AntiAlias = true,
                StrokeWidth = 3,
                Color = Color.Blue
            };
            recPaint.SetStyle(Paint.Style.Stroke);

            screenPaint = new Paint(recPaint);
            screenPaint.Color = Color.Red;

            var m = Resources.DisplayMetrics;
            boundsHeight = m.HeightPixels;
            boundsWidth = m.WidthPixels;

            UpdateRectangles();
        }

        private void UpdateRectangles()
        {
            rectangles = Geometry.createRectangleOverlays(angle, numFrames, boundsWidth, boundsHeight);
        }

        public void SetAngle(int ang)
        {
            angle = ang;
            UpdateRectangles();
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var screen = new cRectangleF();
            screen.A = new SKPoint(0, 0);
            screen.B = new SKPoint(boundsWidth, 0);
            screen.C = new SKPoint(boundsWidth, boundsHeight);
            screen.D = new SKPoint(0, boundsHeight);

            canvas.DrawPath(RecPath(RecScaler(screen, scale)), screenPaint);

            foreach (var rec in rectangles.VisibleRecs)
            {
                canvas.DrawPath(RecPath(RecScaler(rec, scale)), recPaint);
            }

            foreach (var rec in rectangles.WideRecs)
            {
                canvas.DrawPath(RecPath(RecScaler(rec, scale)), recPaint);
            }
        }

        private Path RecPath(cRectangleF rec)
        {
            var path = new Path();
            path.SetFillType(Path.FillType.EvenOdd);
            path.MoveTo(rec.A.X, rec.A.Y);
            path.LineTo(rec.B.X, rec.B.Y);
            path.LineTo(rec.C.X, rec.C.Y);
            path.LineTo(rec.D.X, rec.D.Y);
            path.Close();
            return path;
        }

        private cRectangleF RecScaler(cRectangleF rec, float scal)
        {
            var xShift = boundsWidth / 4;
            var yShift = boundsHeight / 4;
            var A = new SKPoint(rec.A.X * scal + xShift, rec.A.Y * scal + yShift);
            var B = new SKPoint(rec.B.X * scal + xShift, rec.B.Y * scal + yShift);
            var C = new SKPoint(rec.C.X * scal + xShift, rec.C.Y * scal + yShift);
            var D = new SKPoint(rec.D.X * scal + xShift, rec.D.Y * scal + yShift);
            return new cRectangleF(A, B, C, D);
        }
    }
}