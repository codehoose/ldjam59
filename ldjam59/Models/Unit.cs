namespace HackThePlanet.Models
{
    internal class Unit : IUnit
    {
        public Agent Agent { get; }
        public UnitType Type { get; }
        public bool IsIllusion { get; }
        public int TileIndex { get; set; }
        public bool HasActed { get; set; }

        public Unit(Agent agent, UnitType type, bool isIllusion, int tileIndex)
        {
            Agent = agent;
            Type = type;
            IsIllusion = isIllusion;
            TileIndex = tileIndex;
        }
    }
}
