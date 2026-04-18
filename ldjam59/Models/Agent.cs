namespace HackThePlanet.Models
{
    /// <summary>
    /// The in-game representative of the player. Think TRON to Alan Bradley
    /// </summary>
    internal class Agent : IUnit
    {
        public Player Owner { get; }
        public int TileIndex { get; set; }
        public bool IsAlive { get; set; }
        public bool HasActed { get; set; }

        public Agent(Player owner, int tileIndex)
        {
            Owner = owner;
            TileIndex = tileIndex;
        }
    }
}
