using System;
using TestJeux.API.Models;
using TestJeux.Business.ObjectValues;

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
