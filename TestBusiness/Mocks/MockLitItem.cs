using System;
using TestJeux.Business.ObjectValues;
using TestJeux.Core.Entities.Items;

namespace TestBusiness.Mocks
{

	public class MockLitItem : MockItemModel, ILightSource
	{

		public LightState LightState { get; set; }

		public event EventHandler<LightState> LightSourceChanged;

		public MockLitItem(int id, bool isLit, int intensity)
			: base(id)
		{
			LightState = new LightState(isLit, intensity);
		}

	}
}
