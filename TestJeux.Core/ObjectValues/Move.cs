using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.ObjectValues
{
	public class Movement
    {
        public int UnitNumber { get; }
        public DirectionEnum Direction { get; }

        public Movement(int unitNumber, DirectionEnum direction)
        {
            UnitNumber = unitNumber;
            Direction = direction;
        }
    }
}
