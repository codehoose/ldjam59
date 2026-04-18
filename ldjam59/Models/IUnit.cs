namespace HackThePlanet.Models
{
    internal interface IUnit
    {
        int TileIndex { get; }
        bool HasActed { get; }
        UnitType Type { get; }
    }
}
