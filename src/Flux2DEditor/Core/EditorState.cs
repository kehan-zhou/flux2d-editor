using Flux2DEditor.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core
{
    public class EditorState
    {
        // Singleton instance
        private static readonly EditorState _instance = new EditorState();
        public static EditorState Instance => _instance;

        public  List<IShape> Shapes { get; private set; } = [];
        public IShape? SelectedShape { get; private set; }

        private EditorState() { }

        public void SelectShape(IShape? shape)
        {
            foreach (var s in Shapes)
            {
                s.IsSelected = (s == shape);
            }
            SelectedShape = shape;
        }

        public void AddShape(IShape shape)
        {
            Shapes.Add(shape);
            SelectShape(shape);
        }

        public void DeleteSelectedShape()
        {
            if (SelectedShape != null)
            {
                Shapes.Remove(SelectedShape);
                SelectedShape = null;
            }
        }

        public IShape? FindShapeAt(PointF point)
        {
            return Shapes.AsEnumerable().Reverse().FirstOrDefault(s => s.HitTest(point));
        }
    }
}
