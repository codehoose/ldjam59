namespace HackThePlanet.Commands
{
    internal abstract class BaseCommand : ICommand
    {
        public abstract void Execute();

        public abstract void Undo();
    }
}
