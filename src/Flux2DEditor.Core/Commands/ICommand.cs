namespace Flux2DEditor.Core.Commands
{
    /// <summary>
    /// Represents a reversible editing command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        void Execute();

        /// <summary>
        /// Reverts the command.
        /// </summary>
        void Undo();

        /// <summary>
        /// Optional description of the command for UI display.
        /// </summary>
        string Description { get; }
    }
}