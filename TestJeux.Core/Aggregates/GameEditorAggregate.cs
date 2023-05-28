using System.Drawing;
using System.Linq;
using TestJeux.Core.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;
using TestJeux.SharedKernel.Exceptions;

namespace TestJeux.Core.Aggregates
{
	public class GameEditorAggregate : GameAggregateBase
	{
		/// <summary>
		/// Udapte zone
		/// </summary>
		/// <param name="zoneId"></param>
		/// <param name="topLeft"></param>
		/// <param name="bottomRight"></param>
		/// <param name="groundType"></param>
		public void UpdateZone(int zoneId, Point topLeft, Point bottomRight, GroundType groundType)
		{
			CheckZoneExistence(zoneId);

			var zone = _currentLevel.Zones.FirstOrDefault(z => z.ID == zoneId);
			zone.TopLeft = topLeft;
			zone.BottomRight = bottomRight;
			zone.GroundType = groundType;
		}

		/// <summary>
		/// Add zone
		/// </summary>
		/// <param name="topLeft"></param>
		/// <param name="bottomRight"></param>
		/// <param name="groundType"></param>
		public void AddZone(Point topLeft, Point bottomRight, GroundType groundType)
		{
			_currentLevel.Zones.Add(new ZoneModel(0, topLeft, bottomRight, groundType));
		}

		/// <summary>
		/// Remove zone
		/// </summary>
		/// <param name="zoneId"></param>
		public void RemoveZone(int zoneId)
		{
			CheckZoneExistence(zoneId);

			_currentLevel.Zones.Remove(_currentLevel.Zones.FirstOrDefault());				
		}

		/// <summary>
		/// Check zone id is valid
		/// </summary>
		/// <param name="zoneId"></param>
		/// <exception cref="InvalidAggregateOperationException"></exception>
		private void CheckZoneExistence(int zoneId)
		{
			if (!_currentLevel.Zones.Any(z => z.ID == zoneId))
				throw new InvalidAggregateOperationException(typeof(ZoneModel).ToString(), zoneId);
		}
	}
}
