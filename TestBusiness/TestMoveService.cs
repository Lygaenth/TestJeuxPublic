using Moq;
using NUnit.Framework;
using System.Drawing;
using TestBusiness.Mocks;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.Business.Managers;
using TestJeux.Business.Managers.API;
using TestJeux.Business.Services.API;
using TestJeux.Core.Aggregates;
using TestJeux.SharedKernel.Enums;

namespace TestBusiness
{
	internal class TestMoveService
	{
		private GameAggregate _gameRoot;
		private MoveService _moveManager;
		private Mock<ILevelService> _levelManager;
		private Mock<IActionManager> _actionManager;

		private MockItemModel _mockModel1;
		private MockItemModel _mockModel2;

		[SetUp]
		public void Setup()
		{
			_gameRoot = new GameAggregate();
			_levelManager = new Mock<ILevelService>();
			_actionManager= new Mock<IActionManager> ();

			_moveManager = new MoveService(_gameRoot, _levelManager.Object, _actionManager.Object);

			_gameRoot.AddLevel(new Level(0));
			_gameRoot.SetLevelAsCurrent(0);

			_mockModel1 = new MockItemModel(1);
			_mockModel2 = new MockItemModel(2);

			_gameRoot.GetCurrentLevel().Items.Add(_mockModel1);
			_gameRoot.GetCurrentLevel().Items.Add(_mockModel2);

			_mockModel1.X = 100;
			_mockModel1.Y = 150;
			_mockModel2.X = 300;
			_mockModel2.Y = 350;

			_moveManager.Register(_mockModel1.ID);
			_moveManager.Register(_mockModel2.ID);
		}

		[Test]
		public void TestRegisteringAndUnregistering()
		{
			Assert.AreEqual(1, _moveManager.IsPositionOccupied(new Point(100, 150)));
			Assert.AreEqual(2, _moveManager.IsPositionOccupied(new Point(300, 350)));

			_moveManager.Unregister(1);
			_moveManager.Unregister(2);

			Assert.AreEqual(-1, _moveManager.IsPositionOccupied(new Point(100, 150)));
			Assert.AreEqual(-1, _moveManager.IsPositionOccupied(new Point(300, 350)));
		}

		[Test]
		public void TestCanMove()
		{
			_moveManager.Register(1);
			_moveManager.Register(2);

			// check moving is block by item 1
			Assert.IsFalse(_moveManager.CanMove(MoveType.Walk, new Point(100,150)));

			// Check ground type incompatible with move
			_levelManager.Setup(s => s.GetGroundType(_gameRoot.GetCurrentLevel().ID, new Point(200, 250))).Returns(GroundType.Water);
			Assert.IsFalse(_moveManager.CanMove(MoveType.Walk, new Point(200, 250)));

			// Check move compatible with 
			Assert.IsTrue(_moveManager.CanMove(MoveType.Swim, new Point(200, 250)));
			Assert.IsTrue(_moveManager.CanMove(MoveType.WalkOrSwim, new Point(200, 250)));

			// Check item modifying ground type
			_levelManager.Setup(s => s.GetGroundType(_gameRoot.GetCurrentLevel().ID, new Point(300, 350))).Returns(GroundType.Water);
			Assert.IsFalse(_moveManager.CanMove(MoveType.WalkOrSwim, new Point(300, 350)));
			_mockModel2.IsGround = true;
			Assert.IsTrue(_moveManager.CanMove(MoveType.WalkOrSwim, new Point(300, 350)));
		}

		[Test]
		public void TestGetavailableDirection()
		{
			_mockModel2.X = 100;
			_mockModel2.Y = 250;

			_levelManager.Setup(s => s.GetGroundType(_gameRoot.GetCurrentLevel().ID, new Point(150, 100))).Returns(GroundType.Neutral);
			_levelManager.Setup(s => s.GetGroundType(_gameRoot.GetCurrentLevel().ID, new Point(150, 200))).Returns(GroundType.Water);

			var availableDirectionsToWalk = _moveManager.GetAvailableDirections(new Point(100, 200), MoveType.Walk);
			Assert.AreEqual(1, availableDirectionsToWalk.Count);
			Assert.AreEqual(DirectionEnum.Left, availableDirectionsToWalk[0]);

			var availableDirectionsToSwim = _moveManager.GetAvailableDirections(new Point(100, 200), MoveType.Swim);
			Assert.AreEqual(1, availableDirectionsToSwim.Count);
			Assert.AreEqual(DirectionEnum.Right, availableDirectionsToSwim[0]);

			var availableDirectionsToSwimOrWalk = _moveManager.GetAvailableDirections(new Point(100, 200), MoveType.WalkOrSwim);
			Assert.AreEqual(2, availableDirectionsToSwimOrWalk.Count);
			Assert.AreEqual(DirectionEnum.Left, availableDirectionsToSwimOrWalk[0]);
			Assert.AreEqual(DirectionEnum.Right, availableDirectionsToSwimOrWalk[1]);
		}

		[Test]
		public void TestMoveAndReaction()
		{
			_moveManager.MoveCharacter(DirectionEnum.Right, _mockModel1.ID);
			_moveManager.NotifyEndOfMove(_mockModel1.ID);

			Assert.AreEqual(150, _mockModel1.X);
			Assert.AreEqual(150, _mockModel1.Y);
			Assert.AreEqual(true, _mockModel2.WasRequestedReaction);
		}
	}
}
