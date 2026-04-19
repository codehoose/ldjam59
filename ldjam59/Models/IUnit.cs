namespace HackThePlanet.Models
{
    internal interface IUnit
    {
        int TileIndex { get; set; }
        bool HasActed { get; set; }
        bool IsGhost { get; }
        UnitType Type { get; }
    }
}
