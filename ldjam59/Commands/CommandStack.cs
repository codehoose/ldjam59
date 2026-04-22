using System.Collections.Generic;

namespace HackThePlanet.Commands
{
    internal class CommandStack
    {
        private static CommandStack _instance;

        public static CommandStack Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommandStack();
                }

                return _instance;
            }
        }

        private readonly Stack<ICommand> _commands = [];
        private readonly Stack<ICommand> _redo = [];

        public void Execute(ICommand command)
        {
            command.Execute();
            _commands.Push(command);
        }

        public void Undo()
        {
            if (_commands.Count==0) return;

            var command = _commands.Pop();
            command.Undo();
        }

    }
}
