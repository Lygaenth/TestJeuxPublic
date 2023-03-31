using System;
using System.Xml.Serialization;

namespace TestJeux.SharedKernel.Enums
{
    /// <summary>
    /// Direction enum
    /// </summary>
    [Serializable]
    public enum DirectionEnum
    {
        [XmlEnum("0")]
        Left = 0,
        [XmlEnum("1")]
        Top =1,
        [XmlEnum("2")]
        Right =2,
        [XmlEnum("3")]
        Bottom = 3
    }
}
