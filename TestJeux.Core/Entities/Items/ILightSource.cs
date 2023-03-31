using System;
using TestJeux.Business.ObjectValues;

namespace TestJeux.Core.Entities.Items
{
	/// <summary>
	/// Light source
	/// </summary>
	public interface ILightSource
    {
        event EventHandler<LightState> LightSourceChanged;

        /// <summary>
        /// Light state
        /// </summary>
        LightState LightState { get; set; }
    }
}
