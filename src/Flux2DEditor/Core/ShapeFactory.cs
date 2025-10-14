using Flux2DEditor.Core.Interfaces;
using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Core
{
    /// <summary>
    /// Factory to creating shapes. Supports registration for extensibility.
    /// </summary>
    public static class ShapeFactory
    {
        #region Fields

        private static readonly Dictionary<string, Func<RectangleF, IShape>> _registry = new Dictionary<string, Func<RectangleF, IShape>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Rectangle", bounds => new RectangleShape(bounds) },
            { "Circle", bounds => new CircleShape(bounds) }
        };

        #endregion

        #region API

        /// <summary>
        /// Registers a custom shape constructor for a given type name.
        /// </summary>
        /// <param name="type">The type name of the shape.</param>
        /// <param name="constructor">Constructor function that takes bounds.</param>
        public static void RegisterShape(string type, Func<RectangleF, IShape> constructor)
        {
            _registry[type] = constructor;
        }

        /// <summary>
        /// Creates a shape of the requested type.
        /// </summary>
        /// <param name="type">Type key (e.g. "Rectangle")</param>
        /// <param name="bounds">Initial bounds of the shape.</param>
        /// <returns>New IShape instance.</returns>
        public static IShape CreateShape(string type, RectangleF bounds)
        {
            if (_registry.TryGetValue(type, out var constructor))
            {
                return constructor(bounds);
            }
            throw new ArgumentException($"Unknown shape type: {type}", nameof(type));
        }
    }
    
    #endregion
}
