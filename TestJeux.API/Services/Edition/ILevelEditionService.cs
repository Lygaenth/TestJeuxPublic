using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestJeux.API.Models;
using TestJeux.Business.Services.API;

namespace TestJeux.API.Services.Edition
{
	/// <summary>
	/// Interface for edition services
	/// </summary>
	public interface ILevelEditionService : ILevelService
	{
		/// <summary>
		/// Update zone
		/// </summary>
		/// <param name="zoneDto"></param>
		void UpdateZone(ZoneDto zoneDto);

		/// <summary>
		/// Add new zone
		/// </summary>
		/// <param name="zoneDto"></param>
		void AddZone(ZoneDto zoneDto);

		/// <summary>
		/// Remove zone
		/// </summary>
		/// <param name="zoneId"></param>
		public void RemoveZone(int zoneId);
	}
}
