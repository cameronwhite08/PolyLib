﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DelaunayTriangulator;

namespace LowPolyLibrary.Animation
{
    class Sweep : AnimationBase
    {
		private int Direction = -1;

        internal Sweep(Triangulation triangulation): base(triangulation) 
		{
			AnimationType = AnimationTypes.Type.Sweep;
		}

        //necessary to prevent animationbase from "setting up" multiple times
        internal override void SetupAnimation()
        {
            base.SetupAnimation();
            IsSetup = true;
        }

        internal override List<AnimatedPoint> RenderFrame()
        {
			var animatedPoints = new List<AnimatedPoint>();
            //all the points will move within 15 degrees of the same direction
            var localDirection = Geometry.getAngleInRange(Direction, 15);

            foreach (var point in FramedPoints[CurrentFrame])
            {
                var distCanMove = shortestDistanceFromPoints(point);
                var xComponent = Geometry.getXComponent(localDirection, distCanMove);
                var yComponent = Geometry.getYComponent(localDirection, distCanMove);
				var p = new AnimatedPoint(point, (float)xComponent, (float)yComponent);
                animatedPoints.Add(p);
            }
			return animatedPoints;
        }
    }
}