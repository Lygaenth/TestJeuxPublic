using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Services
{
	public class MovementDto
	{
		public int UnitNumber { get; set; }
		public DirectionEnum Direction { get; set; }
	}
}
