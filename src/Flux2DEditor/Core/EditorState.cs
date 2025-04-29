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
        private static EditorState? _instance;
        
        public static EditorState? Instance => _instance ?? (_instance = new EditorState());

        private EditorState() { }

        public bool IsModified { get; set; }
        public string? CurrentFilePath { get; set; }
        public List<EditorRectangle> Rectangles { get; } = new();
    }
}
