using System.Collections.Generic;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers.API
{
	/// <summary>
	/// Tile service
	/// </summary>
	public interface ITileService
    {
        /// <summary>
        /// Return tiles of a specific level 
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns></returns>
        List<TileZoneDto> GetTiles(int levelID);

        /// <summary>
        /// Return sprites associated to a ground sprite code
        /// </summary>
        /// <param name="groundSprite"></param>
        /// <returns></returns>
        List<string> GetTileSprites(GroundSprite groundSprite);
    }
}