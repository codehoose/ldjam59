namespace HackThePlanet.Commands
{
    internal interface ICommand
    {
        void Execute();
        void Undo();
    }
}
