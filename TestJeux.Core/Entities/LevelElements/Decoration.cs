using System.Drawing;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.LevelElements
{
	public class Decoration : GraphicalEntity<Decorations>
    {
        public Decoration(int id, Decorations enumValue, Point topLeft)
            : base(id, enumValue, topLeft)
        {

        }
    }
}
