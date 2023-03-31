using System;
using System.Xml.Serialization;

namespace TestJeux.SharedKernel.Enums
{
	[Serializable]
    public enum Musics
    {
        [XmlEnum("0")]
        None,
        [XmlEnum("1")]
        Menu,
        [XmlEnum("2")]
        LevelEasy,
        [XmlEnum("3")]
        Death,
        [XmlEnum("4")]
        Cave
    }
}
