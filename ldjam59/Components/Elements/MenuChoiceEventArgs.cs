using System;

namespace HackThePlanet.Components.Elements
{
    internal class MenuChoiceEventArgs : EventArgs
    {
        public MenuChoice MenuChoice { get; }

        public MenuChoiceEventArgs(MenuChoice menuChoice)
        {
            MenuChoice = menuChoice;
        }
    }
}
