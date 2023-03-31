using System;
using System.Drawing;

namespace TestJeux.Core.Entities
{
	public abstract class GraphicalEntity<T> : Entity where T : Enum
	{
		public T Enumvalue { get; set; }

		public Point TopLeft { get; set; }

		protected GraphicalEntity(int id, T enumValue, Point position) : base(id)
		{
			Enumvalue = enumValue;
			TopLeft = position;
		}
	}
}
