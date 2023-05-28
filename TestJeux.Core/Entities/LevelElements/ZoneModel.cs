using System.Drawing;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.Entities.LevelElements
{
    public class ZoneModel : Entity
    {
        public ZoneModel(int id, Point topLeft, Point bottomRight, GroundType groundType)
            : base(id)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
            GroundType = groundType;
        }

        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }
        public GroundType GroundType { get; set; }

        public override void Copy(Entity entity)
        {
            if (!(entity is ZoneModel zoneModel))
                return;

            TopLeft = zoneModel.TopLeft;
            BottomRight = zoneModel.BottomRight;
            GroundType = zoneModel.GroundType;
        }
    }
}
