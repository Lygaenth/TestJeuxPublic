using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using Test.Jeux.Data.Api;
using TestJeux.API.Models;
using TestJeux.Business.Services;
using TestJeux.Core.Aggregates;
using TestJeux.SharedKernel.Enums;

namespace TestBusiness
{
	public class TestsLevelService
	{
		private LevelService _levelService;
		private Mock<IDALLevels> _mockDalLevel;
		private int _wasLevelChangeRequested;
		private int _lastLevelIdChangeRequest;
		private GameAggregate _gameRoot;


		[SetUp]
		public void Setup()
		{
			_gameRoot = new GameAggregate();
			_mockDalLevel = new Mock<IDALLevels>();
			_levelService = new LevelService(_gameRoot, _mockDalLevel.Object);
			_wasLevelChangeRequested = 0;
			_lastLevelIdChangeRequest = -1;
		}

		private List<LevelDto> CreateLevelDtos()
		{
			var level1Dto = new LevelDto();
			level1Dto.ID = 1;
			level1Dto.DefaultTile = 2;
			level1Dto.LevelMusic = Musics.LevelEasy;
			level1Dto.Shader = ShaderType.Natural;
			level1Dto.Zones.Add(new ZoneDto() { GroundType = GroundType.Water, TopLeft =  new Point(200, 350), BottomRight = new Point(400, 500)});
			level1Dto.TilesZones.Add(new TileZoneDto() { Tile = GroundSprite.Grass, TopLeft = new Point(50, 100), BottomRight = new Point(400,200), Angle = 0 });

			var level2Dto = new LevelDto();
			level2Dto.ID = 2;
			level2Dto.DefaultTile = 3;
			level2Dto.LevelMusic = Musics.Death;
			level2Dto.Shader = ShaderType.Evening;
			level2Dto.TilesZones.Add(new TileZoneDto() { Tile = GroundSprite.Water, TopLeft = new Point(50, 0), BottomRight = new Point(100,100), Angle = 0 });
			level2Dto.TilesZones.Add(new TileZoneDto() { Tile = GroundSprite.CaveWall, TopLeft = new Point(100, 150), Angle = 90 });

			return new List<LevelDto> { level2Dto, level1Dto };
		}

		private void CreateLevelandSetupDalMock()
		{
			var levels = CreateLevelDtos();
			_mockDalLevel.Setup(m => m.LoadAllLevels()).Returns(levels);
			_mockDalLevel.Setup(m => m.GetDataById(1)).Returns(levels[1]);
			_mockDalLevel.Setup(m => m.GetDataById(2)).Returns(levels[0]);
		}

		[Test]
		public void TestLoadingAllLevels()
		{
			_mockDalLevel.Setup(m => m.LoadAllLevels()).Returns(CreateLevelDtos());

			var levels = _levelService.GetAllLevelIds();
			Assert.That(levels.Count, Is.EqualTo(2));
			Assert.That(levels[0], Is.EqualTo(1));			
		}

		[Test]
		public void TestGettingCurrentLevelWithoutSettingAnyLevel()
		{
			var levels = CreateLevelDtos();
			_mockDalLevel.Setup(m => m.LoadAllLevels()).Returns(levels);
			
			 var firstLevel = _levelService.GetCurrentLevel();
			Assert.That(firstLevel, Is.EqualTo(-1));
		}

		[Test]
		public void TestSettingCurrentLevel()
		{
			CreateLevelandSetupDalMock();
			var loadedLevels = _levelService.GetAllLevelIds();

			var firstLevel = _levelService.GetLevel(loadedLevels[0]);
			Assert.That(firstLevel.ID, Is.EqualTo(1));
			Assert.AreEqual(1, _levelService.GetCurrentLevel());

			var secondLevel = _levelService.GetLevel(loadedLevels[1]);
			Assert.That(secondLevel.ID, Is.EqualTo(2));
		}

		[Test]
		public void TestRequestingLevelChangeRaisesEvent()
		{
			_levelService.RaiseLevelChange += OnLevelChanged;
			_levelService.ChangeLevel(4);
			Assert.AreEqual(1, _wasLevelChangeRequested);
			Assert.AreEqual(4, _lastLevelIdChangeRequest);
		}

		private void OnLevelChanged(object sender, TestJeux.Business.Services.API.LevelChangeArgs args)
		{
			_lastLevelIdChangeRequest = args.Level;
			_wasLevelChangeRequested++;
		}

		[Test]
		public void TestGettingOutOfBoundGroundTypeFromPosition()
		{
			CreateLevelandSetupDalMock();
			var loadedLevels = _levelService.GetAllLevelIds();

			_levelService.GetLevel(loadedLevels[0]);

			Assert.AreEqual(GroundType.OutOfBound, _levelService.GetGroundType(loadedLevels[0], new Point(-50, 50)));
			Assert.AreEqual(GroundType.OutOfBound, _levelService.GetGroundType(loadedLevels[0], new Point(0, 800)));
			Assert.AreEqual(GroundType.OutOfBound, _levelService.GetGroundType(loadedLevels[0], new Point(0, -50)));
			Assert.AreEqual(GroundType.OutOfBound, _levelService.GetGroundType(loadedLevels[0], new Point(0, 800)));
		}

		[Test]
		public void TestGettingDefaultGroundTypeInCurrentLevel()
		{
			CreateLevelandSetupDalMock();
			var loadedLevels = _levelService.GetAllLevelIds();

			_levelService.GetLevel(loadedLevels[0]);
			Assert.That(_levelService.GetGroundType(new Point(0, 0)), Is.EqualTo(GroundType.Neutral));
		}

		[Test]
		public void TestGettingGroundTypeFromZone()
		{
			CreateLevelandSetupDalMock();
			var loadedLevels = _levelService.GetAllLevelIds();

			_levelService.GetLevel(loadedLevels[0]);

			Assert.That(_levelService.GetGroundType(loadedLevels[0], new Point(200, 350)), Is.EqualTo(GroundType.Water));
		}
	}
}