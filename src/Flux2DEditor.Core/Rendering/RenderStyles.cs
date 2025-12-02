using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Rendering
{
    /// <summary>
    /// Stroke (outline) style for drawing shapes.
    /// </summary>
    public readonly struct StrokeStyle
    {
        public readonly float Thickness;
        public readonly Vector4 Color;
        public readonly bool IsDashed;

        public StrokeStyle(float thinkness, Vector4 color, bool isDashed = false)
        {
            Thickness = thinkness;
            Color = color;
            IsDashed = isDashed;
        }

        public static StrokeStyle Default =>
            new StrokeStyle(1.5f, new Vector4(0, 0, 0, 1));
    }

    /// <summary>
    /// Fill style for closed shapes.
    /// </summary>
    public readonly struct FillStyle
    {
        public readonly Vector4 Color;

        public FillStyle(Vector4 color)
        {
            Color = color;
        }

        public static FillStyle None =>
            new FillStyle(new Vector4(0, 0, 0, 0));
    }

    public struct RenderStyles
    {
        public StrokeStyle NormalStroke;
        public StrokeStyle SelectedStroke;
        public FillStyle Fill;

        public StrokeStyle HandleStroke;
        public FillStyle HandleFill;

        public static RenderStyles Default => new RenderStyles
        {
            NormalStroke = new StrokeStyle(1.5f, new Vector4(0, 0, 0, 1)),
            SelectedStroke = new StrokeStyle(2.0f, new Vector4(0.1f, 0.5f, 1.0f, 1.0f)),
            Fill = new FillStyle(new Vector4(0.95f, 0.95f, 0.95f, 1.0f)),

            HandleFill = new FillStyle(new Vector4(1, 1, 1, 1)),
            HandleStroke = new StrokeStyle(1.2f, new Vector4(0.1f, 0.5f, 1.0f, 1.0f)),
        };
    }
}
