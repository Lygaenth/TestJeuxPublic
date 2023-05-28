using System;
using System.Drawing;

namespace TestJeux.Core.Entities
{
	public abstract class GraphicalEntity<T> : Entity where T : Enum
	{
		public T Enumvalue { get; set; }

		public Point TopLeft { get; set; }

		/// <summary>
		/// Angle for tile rotation
		/// </summary>
		public int Angle { get; set; }

		protected GraphicalEntity(int id, T enumValue, Point position, int angle) : base(id)
		{
			Enumvalue = enumValue;
			TopLeft = position;
			Angle = angle;
		}

		public override void Copy(Entity entity)
		{
			if (!(entity is GraphicalEntity<T> graphicalEntity))
				return;

			TopLeft = graphicalEntity.TopLeft;
			Angle = graphicalEntity.Angle;
			Enumvalue = graphicalEntity.Enumvalue;
		}
	}
}
