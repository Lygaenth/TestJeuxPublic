using System;
using System.Xml.Linq;
using TestJeux.API.Models;
using TestJeux.API.Services.LightSource;
using TestJeux.Business.ObjectValues;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities.Items;

namespace TestJeux.Business.Services
{
	public class LightSourceService : ILightSourceService
	{
		private readonly GameAggregate _gameRoot;

		public LightSourceService(GameAggregate gameRoot)
		{
			_gameRoot = gameRoot;
		}

		public event EventHandler<LightState> ItemLightChanged;

		private void OnItemLightStateChanged(object sender, LightState e)
		{
			if (ItemLightChanged != null)
				ItemLightChanged(this, e);
		}

		public LightState GetLightSourceState(int itemId)
		{
			var item = _gameRoot.GetItemFromCurrentLevel(itemId);
			if (item is ILightSource lightSource)
				return lightSource.LightState;

			return new LightState(false,0);
		}

		public void SetLightSourceState(int itemId, LightState lightState)
		{
			var item = _gameRoot.GetItemFromCurrentLevel(itemId);
			if (item is ILightSource lightSource)
			{
				lightSource.LightState = lightState;
				if (ItemLightChanged != null)
					ItemLightChanged(this, lightSource.LightState);
			}
		}

		public void Subscribe()
		{
			foreach(var item in _gameRoot.GetItems())
			{
				if (item is ILightSource lightSource)
					lightSource.LightSourceChanged += OnItemLightStateChanged;
			}
		}

		public void Unsubscribe()
		{
			if (_gameRoot.GetCurrentLevel() == null)
				return;

			foreach (var item in _gameRoot.GetItems())
			{
				if (item is ILightSource lightSource)
					lightSource.LightSourceChanged -= OnItemLightStateChanged;
			}
		}

		public void Reset()
		{
			// Nothing to do
		}
	}
}
