namespace TestJeux.Core.Entities
{
	public abstract class Entity
	{
		public int ID { get; }

		public Entity(int id)
		{
			ID = id;
		}
	}
}
