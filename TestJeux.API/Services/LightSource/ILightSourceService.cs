using System;
using TestJeux.Business.ObjectValues;

namespace TestJeux.API.Services.LightSource
{
	public interface ILightSourceService : ISubscribingService
    {
        /// <summary>
        ///  light state changed
        /// </summary>
        event EventHandler<LightState> ItemLightChanged;

        /// <summary>
        /// Get item light source state
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        LightState GetLightSourceState(int itemId);

        /// <summary>
        /// Set light source state
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="lightSourceDto"></param>
        void SetLightSourceState(int itemId, LightState lightSourceDto);
	}
}
