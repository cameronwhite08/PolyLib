﻿using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using PolyLib.Animation;
using SkiaSharp;
using SkiaSharp.Views.Android;

namespace PolyLib.Views.Android
{
    public class AnimationUpdateView : SKCanvasView, IAnimationUpdateView
    {
        public AnimationEngine Engine { get; private set; }

        #region Constructors

        public AnimationUpdateView(Context context) : base(context)
        {
            Initialize();
        }

        public AnimationUpdateView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public AnimationUpdateView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize();
        }

        #endregion

        private void Initialize()
        {
            Engine = new AnimationEngine(this);
        }

        protected override void OnDraw(SKSurface surface, SKImageInfo info)
        {
            base.OnDraw(surface, info);

            DrawOnMe(surface);
        }

        public void DrawOnMe(SKSurface surf)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Engine.DrawOnMe(surf);
            Console.WriteLine("Animation Frame drawn in: " + watch.ElapsedMilliseconds + " ms\n");
        }

        public void SignalRedraw()
        {
            Invalidate();
        }

        public void AddAnimation(AnimationBase anim)
        {
            Engine.AddAnimation(anim);
        }
    }
}