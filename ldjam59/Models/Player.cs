namespace HackThePlanet.Models
{
    internal class Player
    {
        internal string Name { get; set; }
        internal Agent Agent { get; set; }

        public Player(int tileIndex)
        {
            Agent = new Agent(this, tileIndex);
        }
    }
}
