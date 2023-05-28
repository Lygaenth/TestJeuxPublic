using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Media.Imaging;
using TestJeux.API.Services;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Managers.API;
using TestJeux.Display.Helper;
using TestJeux.Display.ViewModels.Base;
using TestJeux.Display.ViewModels.Display.Stats;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModels.Display
{
	public class ItemViewModel : BaseViewModel
    {
		private const int DefaultCaseChangetime = 800;
		private const int ThreadSleep = 30;
		private const int Step = 5;

		private readonly IMoveService _moveManager;
		private readonly ICharacterManager _characterManager;

		protected DateTime _lastTimeMoving;
		private DateTime _lastRefresh;
		private int _currentSpriteIndex = 0;
		private string _currentSpriteCode = "";
		public int AnimationDuration { get; set; }
		public bool IsSelected { get; private set; }

		protected int _X;
		protected int _Y;

		public int ID { get; private set; }
		public int Priority { get; set; }

		private ItemCode _code;
		public ItemCode Code { get => _code ; set => SetProperty(ref _code, value); }

		public int X
		{
			get { return _X; }
			set
			{
				if (X == value)
					return;

				SetProperty(ref _X, value);
				RaisePropertyChanged("Center");
			}
		}

		public int Y
		{
			get { return _Y; }
			set
			{
				if (Y == value)
					return;

				SetProperty(ref _Y, value);
				RaisePropertyChanged("Center");
			}
		}

		public Point Position
		{
			get => new Point(X, Y);
		}

		public Point Center
		{
			get { return new Point(X + ConstantesDisplay.UnitSize / 2, Y + ConstantesDisplay.UnitSize / 2); }
		}

		bool _isMoving;
		public bool IsMoving
		{
			get { return _isMoving; }
			set
			{
				if (_isMoving == value)
					return;
				SetProperty(ref _isMoving, value);
				Debug.WriteLine("Set moving = " + value.ToString());
			}
		}

		private CachedBitmap _displayedSprite;
		public CachedBitmap DisplayedSprite { get => _displayedSprite; set => SetProperty(ref _displayedSprite, value); }

        private readonly ItemModel _model;

		private StatsViewModel _stats;
		public StatsViewModel Stats { get => _stats; set => SetProperty(ref _stats, value); }

        public ItemViewModel(ItemModel model, ICharacterManager characterManager, IMoveService moveManager)
        {
			ID = model.ID;

			_characterManager = characterManager;
			_moveManager = moveManager;

			// Will be replaced by proper methods from character manager later
			_model = model;
			Code = model.Code;
			IsSelected = _model.ItemType == ItemType.Character;
            Priority = model.Priority;
			AnimationDuration = 500;
			X = _model.X;
			Y = _model.Y;

			Stats = new StatsViewModel(_model.Stats);
		}

		public void Subscribe()
		{
			_moveManager.MoveStarted += OnMoveStarted;
		}

		public void Unsubscribe()
		{
			_moveManager.MoveStarted -= OnMoveStarted;
		}

		private void OnMoveStarted(object? sender, MovementDto e)
		{
			if (sender == null || !Int32.TryParse(sender.ToString(), out int id) || id != ID)
				return;

			MoveCharacter(e.Direction);
		}

		public void RefreshSprite()
		{
			var sprites = _characterManager.GetCurrentSprites(ID);
			var refreshTime = DateTime.Now;
			if (_currentSpriteIndex < sprites.Count)
			{
				if (_currentSpriteCode != sprites[_currentSpriteIndex])
				{
					_currentSpriteCode = sprites[_currentSpriteIndex];
					ExecuteUithread(() => DisplayedSprite = ImageHelper.GetImage(sprites[_currentSpriteIndex]));
				}
				if (AnimationDuration / sprites.Count < (refreshTime - _lastRefresh).TotalMilliseconds)
				{
					_currentSpriteIndex++;
					_lastRefresh = refreshTime;
				}
			}
			else
			{
				if (sprites.Count != 0 && _currentSpriteCode != sprites[0])
				{
					_currentSpriteCode = sprites[0];
					ExecuteUithread(() => DisplayedSprite = ImageHelper.GetImage(sprites[0]));
				}
				_currentSpriteIndex = 0;
			}
		}

		/// <summary>
		/// Move item in direction
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="itemId"></param>
		public void MoveCharacter(DirectionEnum direction)
		{
			Debug.WriteLine("VM move started");
			int sens = (direction == DirectionEnum.Top || direction == DirectionEnum.Left) ? -1 : +1;
			var step = Step * sens;

			if (direction == DirectionEnum.Bottom || direction == DirectionEnum.Top)
			{
				int startingPoint = Y;
				while (Y != startingPoint + sens * ConstantesDisplay.UnitSize)
				{
					ExecuteUithread(() => Y += step);
					Thread.Sleep(ThreadSleep);
				}
			}
			else
			{
				int startingPoint = X;
				while (X != startingPoint + sens * ConstantesDisplay.UnitSize)
				{
					ExecuteUithread(() => X += step);
					Thread.Sleep(ThreadSleep);
				}
			}

			if (!_moveManager.HasQueuedMove(ID))
				IsMoving = false;

			_moveManager.NotifyEndOfMove(ID);
			RefreshSprite();
		}
	}
}
