using System;
using System.Xml.Serialization;

namespace TestJeux.SharedKernel.Enums
{
	[Serializable]
    public enum ShaderType
    {
        [XmlEnum("0")]
        Natural,
        [XmlEnum("1")]
        Evening,
        [XmlEnum("2")]
        Cave
    }
}
