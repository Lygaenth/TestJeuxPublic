﻿using System;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.Helper
{
	public static class DirectionHelper
	{
		public static DirectionEnum GetOppositeDirection(DirectionEnum direction)
		{
			switch (direction)
			{
				case DirectionEnum.Left:
					return DirectionEnum.Right;
				case DirectionEnum.Right:
					return DirectionEnum.Left;
				case DirectionEnum.Bottom:
					return DirectionEnum.Top;
				case DirectionEnum.Top:
					return DirectionEnum.Bottom;
			}
			throw new NotImplementedException();
		}
	}
}
