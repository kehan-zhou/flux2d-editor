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

        public bool IsModified { get; set; }
        public string? CurrentFilePath { get; set; }
        public List<EditorRectangle> Rectangles { get; } = new();
        public List<RectangleObject> Objects { get; set; } = new();
        public RectangleObject? SelectedObject { get; set; } = null;

        private EditorState() { }
    }
}
