namespace HackThePlanet.Models
{
    /// <summary>
    /// The in-game representative of the player. Think TRON to Alan Bradley
    /// </summary>
    internal class Agent : IUnit
    {
        public Player Owner { get; }
        public int TileIndex { get; set; }
        public bool IsAlive { get; set; } = true;
        public bool HasActed { get; set; }
        public bool IsGhost => false;
        public UnitType Type { get; } = UnitType.None;

        public Agent(Player owner, int tileIndex)
        {
            Owner = owner;
            TileIndex = tileIndex;
        }
    }
}
