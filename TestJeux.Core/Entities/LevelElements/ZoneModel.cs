using System.Drawing;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.Entities.LevelElements
{
    public class ZoneModel
    {
        public ZoneModel(int id, Point topLeft, Point bottomRight, GroundType groundType)
        {
            ID = id;
            TopLeft = topLeft;
            BottomRight = bottomRight;
            GroundType = groundType;
        }

        public int ID { get; private set; }
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }
        public GroundType GroundType { get; set; }
    }
}
