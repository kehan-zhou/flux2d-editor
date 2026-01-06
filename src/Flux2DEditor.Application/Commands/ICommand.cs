namespace Flux2DEditor.Application.Commands
{
    public interface ICommand
    {
        void Execute();

        void Undo();
    }
}
