using CoreAnimation;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace XGallery.iOS.Inherit.Tools
{
    public class FPSHelper
    {
        CADisplayLink displayLink;
        double lastTime =0;
        int count = 0;
        Action<int> complete;
        bool isRunning = false;
       public  void start(Action<int> success)
        {
            if (isRunning)
                stop();
            isRunning = true;
            complete = success;
            //displayLink = CADisplayLink.Create(this,new ObjCRuntime.Selector("tick:"));
            displayLink = CADisplayLink.Create(()=>tick());
            displayLink.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
        }

        public void stop()
        {
            lastTime = 0;
            count = 0;
            isRunning = false;
            displayLink?.Invalidate();
        }

        [Export("tick:")]
        void tick()
        {
            if (lastTime == 0)
            {
                lastTime = displayLink.Timestamp;
                return;
            }
              
            count++;
            var delta = displayLink.Timestamp - lastTime;
            if (delta < 1) return;
            lastTime = displayLink.Timestamp;
            var fps = count / delta;
            count = 0;
            complete((int)fps);
        }
    }
}