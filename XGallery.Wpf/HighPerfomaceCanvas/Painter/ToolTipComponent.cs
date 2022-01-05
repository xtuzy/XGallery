using CanvasDemo.Canvas;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDemo.Painter
{
    public class ToolTipComponent : IToolTip
    {
        TimCanvas Canvas;

        public ToolTipComponent(TimCanvas canvas)
        {
            Canvas = canvas;
        }

        public void Drawing(SKCanvas g)
        {

        }

        public void DrawingAfter(SKCanvas g)
        {

        }

        public void Hide()
        {
            if (LastCube != null)
            {
                LastCube.IsHover = false;
                LastCube = null;
                Canvas.Refresh();
            }
        }

        CubeElement LastCube;

        public void Show(IToolTipElement element)
        {
            if (element is CubeElement cube && LastCube != cube)
            {
                if (LastCube != null) LastCube.IsHover = false;


                LastCube = cube;
                LastCube.IsHover = true;

                Canvas.Refresh();
            }


        }
    }
}
