using System.Collections.Generic;
using System.Windows.Input;
using TestJeux.Business.Managers;
using System.Linq;
using TestJeux.Business.Managers.API;
using TestJeux.Business.Services;
using TestJeux.Business.Services.API;
using System;
using System.Windows;
using TestJeux.Business.Supervisor;
using TestJeux.Display.ViewModels.Base;
using Test.Jeux.Data;
using TestJeux.Display.ViewModels.Display;
using TestJeux.API.Services.LightSource;
using TestJeux.Display.Controllers;
using TestJeux.Display.Managers;
using TestJeux.Business.Entities.Stats;
using TestJeux.Business.Entities.Action;
using TestJeux.Display.ViewModels.Display.Menu;
using TestJeux.Display.ViewModels.Display.Levelelements;
using TestJeux.Display.ViewModels.Display.ChatBox;
using TestJeux.API.Services.TextInteractions;
using TestJeux.SharedKernel.Enums;
using TestJeux.Core.Aggregates;
using TestJeux.Core.ObjectValues;
using TestJeux.Core.Entities;
using TestJeux.API.Services;

namespace TestJeux.Display.ViewModels
{
	public class GameViewModel : BaseViewModel
    {
        #region attributes

        private readonly ILevelService _levelService;
        private readonly IMoveService _moveManager;
        private readonly IScriptService _scriptManager;
        private readonly ICharacterManager _characterManager;
        private readonly IImageManager _imageManager;
        private readonly IActionManager _actionManager;
        private readonly IEquipmentManager _equipmentManager;
        private readonly ISoundService _soundManager;
        private readonly ITileService _tileManager;
        private readonly ILightSourceService _lightSourceService;
        private readonly IChatService _chatService;

		private readonly ActionFabric _actionFabric;
		private readonly ICharacterBuilder _characterSupervisor;

        private readonly List<ISubscribingService> _subscribedServices;

        private readonly ControllerService _controllerManager;
        private LevelViewModel _selectedLevel;
        
        private bool _isLost;
        private bool _isLevelWon;
        private DisplayViewModel _displayVm;
        private DisplayTextViewModel _displayTextVm;
        private MainMenuViewModel _mainMenuViewModel;

        public KeyBoardViewModel KeyBoardVm { get; set; }
        public EquipmentDisplayViewModel EquipmentVm { get; set; }

        private ViewEnum _currentView;

        public ViewEnum View
        {
            get => _currentView;
            set
            {
                _currentView = value;
                KeyBoardVm.SetView(value);
            }
        }
        #endregion

        #region properties
        public DisplayViewModel DisplayVm 
        {
            get { return _displayVm; }
            set { SetProperty(ref _displayVm, value); }
        }

        public DisplayTextViewModel DisplayTextVm
        {
            get {  return _displayTextVm; }
            set { SetProperty(ref _displayTextVm, value); }
        }

        public MainMenuViewModel MainMenuVm
        {
            get { return _mainMenuViewModel; }
            set { SetProperty(ref _mainMenuViewModel, value); }
        }
        #endregion

        public GameViewModel()
        {
			// Create root
			var gameRoot = new GameAggregate();

			// Managers
			_imageManager = new ImageManager();
            _equipmentManager = new EquipmentManager(gameRoot);

            // Initialize services but should be injected later
            _levelService = new LevelService(gameRoot, new DALLevels());
            _chatService = new ChatService(gameRoot);
			_actionManager = new ActionManager(gameRoot, _chatService);

			_moveManager = new MoveService(gameRoot, _levelService, _actionManager);
            _soundManager = new SoundManager(gameRoot);
            _scriptManager = new ScriptService(gameRoot);
            _tileManager = new TileService(gameRoot);

			_actionFabric = new ActionFabric(_levelService, _moveManager, _equipmentManager, _chatService, _soundManager);

            _characterManager = new CharacterManager(gameRoot);

            _controllerManager = new ControllerService();

            (_moveManager as MoveService).Initialize(_actionFabric);

            _lightSourceService = new LightSourceService(gameRoot);

            _characterSupervisor = new CharacterBuilder(this, _moveManager, _characterManager, _scriptManager);

            _subscribedServices = new List<ISubscribingService>();
            _subscribedServices.Add(_soundManager);
			_subscribedServices.Add(_moveManager);
            _subscribedServices.Add(_equipmentManager);
            _subscribedServices.Add(_actionManager);
            _subscribedServices.Add(_lightSourceService);

			// VMs
			MainMenuVm = new MainMenuViewModel(_soundManager);
            KeyBoardVm = new KeyBoardViewModel(_controllerManager, MainMenuVm);
            DisplayTextVm = new DisplayTextViewModel(_chatService);
            _selectedLevel = new LevelViewModel(_levelService, _tileManager, _characterManager, _characterSupervisor);
            var tileVm = new TileDisplayViewModel(_tileManager, _imageManager);
            var decorationVm = new DecorationDisplayViewModel(new DecorationManager(), _imageManager);
            EquipmentVm = new EquipmentDisplayViewModel(_equipmentManager, _characterManager);
            var shaderVm = new ShaderViewModel(_lightSourceService);
            DisplayVm = new DisplayViewModel(DisplayTextVm, _selectedLevel, shaderVm);

            _levelService.RaiseLevelChange += OnLevelChangeRequested;

            View = ViewEnum.Menu;
            DisplayVm.DisplayView(ViewEnum.Menu);
            _soundManager.PlayBackGroundMusic(Musics.Menu);

            MainMenuVm.RequestGameLoad += OnRequestGameLoad;
            MainMenuVm.RequestGameQuit += OnRequestGameQuit;

            KeyBoardVm.RequestSwitchView += OnRequestSwitchView;
            KeyBoardVm.RequestAction += OnRequestAction;
            KeyBoardVm.RequestSwitchEquipment += OnRequestSwitchEquipment;

            _controllerManager.MoveRaised += OnMoveRaised;
            _controllerManager.Start();
        }

        private void OnRequestGameQuit(object sender, EventArgs e)
        {
            ActionQuit();
        }

        private void OnRequestGameLoad(object sender, EventArgs e)
        {
            ActionStart();
        }

        private void OnRequestSwitchEquipment(object sender, System.EventArgs e)
        {
            ExecuteUithread(SwitchEquipment);
        }

        private void OnRequestAction(object sender, System.EventArgs e)
        {
            if (View == ViewEnum.Menu)
                MainMenuVm.StartAction(Key.Space);
            else
                ExecuteUithread(LaunchAction);
        }

        private void OnRequestSwitchView(object sender, ViewEnum view)
        {
            View = view;
            DisplayVm.DisplayView(view);

            if(view == ViewEnum.Menu)
                _soundManager.PlayBackGroundMusic(Musics.Menu);
        }

        private void OnLevelChangeRequested(object sender, LevelChangeArgs args)
        {
			DisplayVm.Stop();
			DisplayTextVm.Reset();

			if (!LoadLevel(args.Level))
                return;

            if (DisplayVm.LevelVm.SelectedItem != null)
                EquipmentVm.SetCharacter(DisplayVm.LevelVm.SelectedItem.ID);

            DisplayVm.Start(); 
        }

        private void OnMoveRaised(object sender, DirectionEnum direction)
        {
            if (View == ViewEnum.Menu)
                MainMenuVm.MoveCursor(direction);
            else
                Move(direction);
        }

        #region Commands
        public void ActionStart()
        {
            View = ViewEnum.Game;

            _soundManager.PlaySoundEffect(SoundEffects.SwitchScreen);

            _levelService.Reset();
            DisplayTextVm.Reset();
            EquipmentVm.Reset();
            _characterManager.Reset();

            _isLost = false;
            DisplayVm.IsLost = false;
            DisplayVm.LevelVm.Reset();

            DisplayVm.DisplayView(ViewEnum.Game);

            LoadLevel(1);
        }

        public void ActionQuit()
        {
            Stop();            
            _soundManager.Reset();
            Application.Current.Shutdown();
        }

        public void Stop()
        {
            _soundManager.PlaySoundEffect(SoundEffects.SwitchScreen);
            View = ViewEnum.Menu;
            DisplayVm.Stop();
            _actionManager.Stop();
            _characterManager.Reset();
            _controllerManager.Stop();
            if(DisplayVm.LevelVm.SelectedItem != null)
				DisplayVm.LevelVm.SelectedItem.Stats.PropertyChanged -= StatusCheck;
        }
       
        public void SwitchEquipment()
        {
            EquipmentVm.SwitchEquipment();            
        }
        #endregion

        #region public methods

        public void Move(DirectionEnum direction)
        {
            if (_isLost || _isLevelWon || !DisplayVm.DisplayGame)
                return;

            Move(direction, _selectedLevel.SelectedItem);
        }

        public void Move(DirectionEnum direction, ItemViewModel item)
        {
            if (DisplayTextVm.IsTalking())
                return;

            if (DisplayVm.LevelVm.SelectedItem == null || DisplayVm.LevelVm.SelectedItem.IsMoving || _actionManager.IsActionBlocking() || _actionManager.IsActionWaitingForAquittement())
                return;

            var target = GetTargets(direction, item);
            if (target != null)
            {
                _characterManager.SetCharacterOrientation(item.ID, direction);

                var model = _characterManager.GetCharacter(item.ID);

				// Move character
				if (_moveManager.CanMove(model.GetMoveType(), _moveManager.GetTargetPosition(item.Position, direction)))
                {
                    var move = _actionFabric.CreatePjMoveAction(model, target.GroundType);
                    _actionManager.AddAction(move);
                }
            }
        }

        public void LaunchAction()
        {
            if (_isLost || View != ViewEnum.Game)
                return;

            if (_actionManager.IsActionWaitingForAquittement())
            {
                _actionManager.Acq();
            }
            else if (!_actionManager.IsActionBlocking())
            {
                if (DisplayVm.LevelVm.SelectedItem == null)
                    return;

				// TODO Replace by an interaction service
				var actionTarget = GetTargets(_characterManager.GetCharacterOrientation(DisplayVm.LevelVm.SelectedItem.ID), DisplayVm.LevelVm.SelectedItem);
                actionTarget.SourceItemId = _characterManager.GetCharacter(DisplayVm.LevelVm.SelectedItem.ID).ID;
                _actionManager.AddRangeActions(GetActionTarget(actionTarget));
            }
        }

        public ActionTarget GetTargets(DirectionEnum direction, ItemViewModel item)
        {
            var actionTarget = new ActionTarget();
            actionTarget.Orientation = direction;
            var targetPosition = _moveManager.GetTargetPosition(item.Position, direction);

            actionTarget.GroundType = ModifyGroundTypeBasedOnModifier(_levelService.GetGroundType(_selectedLevel.ID, targetPosition), targetPosition);

            if (item == null)
                return actionTarget;

            var targetId = _moveManager.IsPositionOccupied(targetPosition);
            if (targetId >= 0)
            {
                actionTarget.TargetItemId = _characterManager.GetCharacter(targetId).ID;
                var targetTargetPosition = _moveManager.GetTargetPosition(_characterManager.GetCharacterState(actionTarget.TargetItemId.Value).Position, direction);
                actionTarget.GroundType = ModifyGroundTypeBasedOnModifier(_levelService.GetGroundType(_selectedLevel.ID, targetTargetPosition), targetTargetPosition);
            }
            UpdateGroundTypeBasedOnTarget(actionTarget);
            return actionTarget;
        }

        private GroundType ModifyGroundTypeBasedOnModifier(GroundType groundType, System.Drawing.Point position)
        {
            var targetGroundModifier = _moveManager.HasGroundModifier(position);
            if (targetGroundModifier == GroundModifier.Bridge && groundType == GroundType.Water)
                return GroundType.Neutral;
            return groundType;
        }

        #endregion

        #region private methods
        private bool LoadLevel(int id)
        {
			foreach (var service in _subscribedServices)
				service.Unsubscribe();

			_controllerManager.MoveRaised -= OnMoveRaised;
            _controllerManager.Stop();
			_characterManager.Reset();
            _scriptManager.Reset();
            _moveManager.Reset();

            if (id == -1)
            {
                DisplayVm.IsWin = true;
                _actionManager.Stop();
                return false;
            }

            foreach (var item in DisplayVm.LevelVm.Items)
                _moveManager.Unregister(item.ID);

            ExecuteUithread(() =>
            {
				DisplayVm.LevelVm.LoadLevel(id);
            });

			foreach (var item in DisplayVm.LevelVm.Items)
				_moveManager.Register(item.ID);

            ExecuteUithread(() =>
            {
                DisplayVm.Shader = new ShaderViewModel(_selectedLevel.Shader, _lightSourceService, _selectedLevel.Items.ToList());

                if (DisplayVm.LevelVm.SelectedItem != null)
                {
                    DisplayVm.LevelVm.SelectedItem.Stats.PropertyChanged += StatusCheck;

                    EquipmentVm.SetCharacter(DisplayVm.LevelVm.SelectedItem.ID);
                }
            });

			foreach (var service in _subscribedServices)
				service.Subscribe();

			_actionManager.Start();
            _soundManager.PlayBackGroundMusic(_selectedLevel.Music);

            ExecuteUithread(DisplayVm.Start);

            _controllerManager.Start();
            _controllerManager.MoveRaised += OnMoveRaised;
            return true;
        }

        private void StatusCheck(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StatsModel.CurrentHP))
            {
                if (DisplayVm.LevelVm.SelectedItem.Stats.CurrentHP <= 0)
                {
                    _isLost = true;
                    DisplayVm.IsLost = true;
                    _soundManager.PlayBackGroundMusic(Musics.Death);
                }
            }
        }
        private void UpdateGroundTypeBasedOnTarget(ActionTarget actionTarget)
        {
            if (actionTarget.GroundType == GroundType.Water && actionTarget.TargetItemId != null && _characterManager.GetCharacter(actionTarget.TargetItemId.Value).IsGround)
                actionTarget.GroundType = GroundType.Neutral;
        }

        private List<ActionBase> GetActionTarget(ActionTarget actionTarget)
        {
            // interaction avec le sol
            if (actionTarget.TargetItemId == null)
                return GetActionInteractionWithGround(actionTarget);

            // interaction avec un objet
            var targetTarget = GetTargets(actionTarget.Orientation, _selectedLevel.Items.First(c => c.ID == actionTarget.TargetItemId.Value));
            targetTarget.SourceItemId = actionTarget.TargetItemId.Value;

            return _characterManager.GetCharacter(actionTarget.TargetItemId.Value).Interact(_characterManager.GetCharacter(actionTarget.SourceItemId), _characterManager.GetCharacter(actionTarget.TargetItemId.Value), targetTarget);
        }

        /// <summary>
        /// Get interaction with ground
        /// </summary>
        /// <param name="actionTarget"></param>
        /// <returns></returns>
        public List<ActionBase> GetActionInteractionWithGround(ActionTarget actionTarget)
        {
            var actions = new List<ActionBase>();
            if (actionTarget.GroundType == GroundType.Water)
            {
                if (!_characterManager.GetCharacter(actionTarget.SourceItemId).CanFloat)
                    actions.Add(_actionFabric.CreateSpeakAction(new List<SpeakerDto>() { new SpeakerDto("HeroFront", "You", "I can't swim without a swimsuit") }));
            }
            return actions;
        }

        #endregion
    }
}
