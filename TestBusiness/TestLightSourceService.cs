using NUnit.Framework;
using TestJeux.API.Services.LightSource;
using TestJeux.Business.ObjectValues;
using TestJeux.Business.Services;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities.Items;
using TestJeux.Core.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;

namespace TestBusiness
{
	public class TestLightSourceService
	{
		private GameAggregate _gameAggregate;
		private ILightSourceService _lightSourceService;

		[SetUp]
		public void SetUp()
		{
			_gameAggregate = new GameAggregate();
			_lightSourceService = new LightSourceService(_gameAggregate);
			_gameAggregate.AddLevel(new Level(1));
			_gameAggregate.SetLevelAsCurrent(1);
		}

		[Test]
		public void TestGettingItemNotLit()
		{
			_gameAggregate.AddItemToCurrentLevel(1, ItemCode.Torche);
			var torch = _gameAggregate.GetItemFromCurrentLevel(1);
			torch.SetDefaultState(0);

			var result = _lightSourceService.GetLightSourceState(1);

			Assert.That(result.IsLit, Is.False);
			Assert.That(result.Intensity, Is.EqualTo(75));
		}

		[Test]
		public void TestGettingLitItem()
		{
			_gameAggregate.AddItemToCurrentLevel(1, ItemCode.Torche);

			var result = _lightSourceService.GetLightSourceState(1);

			Assert.That(result.IsLit, Is.True);
			Assert.That(result.Intensity, Is.EqualTo(75));
		}

		[Test]
		public void TestChangingLitState()
		{
			_gameAggregate.AddItemToCurrentLevel(1, ItemCode.Torche);

			var litItem = _gameAggregate.GetItemFromCurrentLevel(1) as ILightSource;
			_lightSourceService.SetLightSourceState(1, new LightState(false, 12));

			Assert.That(litItem.LightState.IsLit, Is.False);
			Assert.That(litItem.LightState.Intensity, Is.EqualTo(12));
		}

	}
}
