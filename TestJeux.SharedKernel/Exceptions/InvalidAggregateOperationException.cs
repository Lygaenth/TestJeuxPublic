using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJeux.SharedKernel.Exceptions
{
	/// <summary>
	/// Exception class for invalid operation on aggregate
	/// </summary>
	public class InvalidAggregateOperationException : Exception
	{
		public string ElementType { get; }
		public int ID { get; }

		public InvalidAggregateOperationException(string elementType, int iD)
		{
			ElementType = elementType;
			ID = iD;
		}
	}
}
