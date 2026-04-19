namespace HackThePlanet.Models
{
    internal interface IUnit
    {
        int TileIndex { get; }
        bool HasActed { get; }
        bool IsGhost { get; }
        UnitType Type { get; }
    }
}
