using System;
using System.Xml.Serialization;

namespace TestJeux.SharedKernel.Enums
{
    [Serializable]
    public enum GroundSprite
    {
        [XmlEnum("1")]
        Grass = 1,
        [XmlEnum("2")]
        Water = 2,
        [XmlEnum("3")]
        WaterSide =3,
        [XmlEnum("4")]
        CaveFloor = 4,
        [XmlEnum("5")]
        CaveWall = 5,
        [XmlEnum("6")]
        CaveWallOneSide = 6,
        [XmlEnum("7")]
        CaveWallOpposedSide = 7,
        [XmlEnum("8")]
        CaveWallOneCorner = 8
    }
}
