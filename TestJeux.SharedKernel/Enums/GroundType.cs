using System;
using System.Xml.Serialization;

namespace TestJeux.SharedKernel.Enums
{
    /// <summary>
    /// Ground type enum
    /// </summary>
    [Serializable]
    public enum GroundType
    {
        [XmlEnum("0")]
        Neutral =0,
        [XmlEnum("1")]
        OutOfBound =1,
        [XmlEnum("2")]
        Water =2,
        [XmlEnum("3")]
        Bridge=3,
        [XmlEnum("4")]
        Block=4,
        [XmlEnum("5")]
        GoUp=5,
        [XmlEnum("6")]
        GoDown=6,
        [XmlEnum("7")]
        WinLevel =7
    }
}
