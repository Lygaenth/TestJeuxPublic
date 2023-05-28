using LevelEditor.wpf.Builders;
using LevelEditor.wpf.ViewModel;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Test.Jeux.Data.Xml;
using TestJeux.Business.Command;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Managers;
using TestJeux.Business.Managers.API;
using TestJeux.Business.Services;
using TestJeux.Business.Services.API;
using TestJeux.Business.Supervisor;
using TestJeux.Core.Aggregates;
using TestJeux.Display.Helper;
using TestJeux.SharedKernel.Enums;
using LevelEditor.wpf.Converters;
using TestJeux.Core.Context;
using TestJeux.API.Services.Edition;
using TestJeux.Business.Edition.Service;
using TestJeux.API.Models;
using System.Drawing;

namespace LevelEditor
{
	public class LevelEditorViewModel : BindableBase
    {
        private bool _showZones;
        private bool _showDecorations;
        private readonly ILevelEditionService _levelService;
        private readonly IImageManager _imageManager;
        private readonly ITileService _tileService;
        private readonly DecorationManager _decorationManager;
        private LevelViewModel _selectedLevel;
        private TileViewModel _selectedTile;
        private ZoneViewModel _selectedZone;

        public bool ShowZones
        {
            get => _showZones;
            set => SetProperty(ref _showZones, value);
        }

        public bool ShowDecorations
        {
            get => _showDecorations;
            set => SetProperty(ref _showDecorations, value);
        }

        public ObservableCollection<LevelViewModel> Levels { get; set; }

        public ObservableCollection<ItemModel> Characters { get; set; }

        public ObservableCollection<ZoneViewModel> DebugZones { get; set; }

        public ObservableCollection<GroundType> GroundTypes { get; set; }

        public ObservableCollection<int> Angles { get; set; }

        public ObservableCollection<TileSpriteInfo> TilesSprites { get; set; }

        private TileSpriteInfo _selectedSpriteInfo;
        public TileSpriteInfo SelectedSpriteInfo
        {
            get => _selectedSpriteInfo;
            set
            {
                SetProperty(ref _selectedSpriteInfo, value);
                if (SelectedTile.GroundSprite != SelectedSpriteInfo.SpriteCode)
                {
                    SelectedTile.GroundSprite = SelectedSpriteInfo.SpriteCode;
                    SelectedTile.UpdateSprites(ImageHelper.GetImages(_tileService.GetTileSprites(SelectedTile.GroundSprite)));
                }
            }
        }

        public ICommand ReloadCmd { get; set; }

        public ICommand SelectTileCmd { get; set; }

        public ICommand AddZoneCmd { get; set; }

        public ICommand DeleteZoneCmd { get; set; }

        public ICommand SelectZoneCmd { get; set; }

        public ICommand SaveCurrentLevelCmd { get; set; }

        public LevelViewModel SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                LoadLevel(value);
                SetProperty(ref _selectedLevel, value); 
            }
        }

        public TileViewModel SelectedTile
        {
            get => _selectedTile;
            set
            {
                SetProperty(ref _selectedTile, value);
                var tileSpriteInfo = TilesSprites.First(t => t.SpriteCode == SelectedTile.GroundSprite);
                if (tileSpriteInfo != SelectedSpriteInfo)
                    SelectedSpriteInfo = tileSpriteInfo;
            }
        }

        public ZoneViewModel SelectedZone
        {
            get => _selectedZone;
            set
            {
                if (SelectedZone != null)
                    SelectedZone.PropertyChanged -= OnZonePropertyChanged;

                SetProperty(ref _selectedZone, value);

                if (SelectedZone != null)
    				SelectedZone.PropertyChanged += OnZonePropertyChanged;
            }
        }

		private void OnZonePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
            var zone = sender as ZoneViewModel;
            if (zone == null)
                return;

            if (e.PropertyName != nameof(ZoneViewModel.X) && e.PropertyName != nameof(ZoneViewModel.Y) && e.PropertyName != nameof(ZoneViewModel.GroundType))
                return;

            if ((zone.X % 50 != 0 || zone.Y % 50 != 0))
                return;

            _levelService.UpdateZone(zone.Convert());
		}

		public LevelEditorViewModel()
        {
            var gameRoot = new GameEditorAggregate();

            Levels = new ObservableCollection<LevelViewModel>();
            Characters = new ObservableCollection<ItemModel>();
            DebugZones = new ObservableCollection<ZoneViewModel>();
            _imageManager = new ImageManager();
            _levelService = new LevelEditionService(gameRoot, new DALLevels(), new GameContext());
            _tileService = new TileService(gameRoot);
            _decorationManager = new DecorationManager();


			GroundTypes = new ObservableCollection<GroundType>
			{
				GroundType.Block,
				GroundType.Neutral,
				GroundType.GoDown,
				GroundType.WinLevel,
				GroundType.Water
			};

			Angles = new ObservableCollection<int>
			{
				0,
				90,
				180,
				270
			};

			TilesSprites = new ObservableCollection<TileSpriteInfo>();
            var tileSprites = Enum.GetValues(typeof(GroundSprite));
            for (int i = 0; i < tileSprites.Length; i++)
            {
                var value = tileSprites.GetValue(i);
                if (value == null)
                    continue;

                var groundSprite = (GroundSprite)value;
                TilesSprites.Add(new TileSpriteInfo(groundSprite, ImageHelper.GetImages(_tileService.GetTileSprites(groundSprite))[0]));
            }

            ShowDecorations = true;
            ShowZones = true;

            ReloadCmd = new Command(ReloadLevel);
            SelectTileCmd = new Command<TileViewModel>(SelectTile);
            AddZoneCmd = new Command(AddZone);
            DeleteZoneCmd = new Command(DeleteZone);
            SaveCurrentLevelCmd = new Command(SaveCurrentLevel);

            var levelIds = _levelService.GetAllLevelIds();
            foreach (var id in levelIds)
            {
                var levelVm = new LevelViewModel(_levelService, _tileService);
                levelVm.LoadLevel(id);
				Levels.Add(levelVm);
            }

            if (Levels.Count == 0)
                Levels.Add(new LevelViewModel(_levelService, _tileService));
            
            SelectedLevel = Levels[0];

            if (DebugZones.Count > 0)
                SelectedZone = DebugZones[0];
        }

        private void LoadLevel(LevelViewModel level)
        {
            var dto = _levelService.GetLevel(level.ID);
            if (dto.ID == -1)
                return;

            level = new LevelViewModel(_levelService, _tileService);
            level.LoadLevel(dto.ID);
            DebugZones.Clear();
            foreach(var zone in level.Zones)
                DebugZones.Add(zone);
        }

        private void ReloadLevel()
        {
            _levelService.Reset();
            DebugZones.Clear();
            LoadLevel(SelectedLevel);
        }

        private void SelectTile(TileViewModel tile)
        {
            SelectedTile = tile;
        }
    
        /// <summary>
        /// Add zone
        /// </summary>
        private void AddZone()
        {
            var zone = new ZoneViewModel(DebugZones.OrderBy(z => z.ID).Last().ID + 1, new System.Drawing.Point(0, 0), 100, 100, GroundType.Block);
            DebugZones.Add(zone);
            _levelService.AddZone(zone.Convert());
        }

        /// <summary>
        /// Delete zone
        /// </summary>
        private void DeleteZone()
        {
            if (SelectedZone == null)
                return;

            DebugZones.Remove(SelectedZone);

            _levelService.RemoveZone(SelectedZone.ID);

            SelectedZone = DebugZones[0];

        }

        /// <summary>
        /// Save current modifications
        /// </summary>
        private void SaveCurrentLevel()
        {
            _levelService.SaveLevel(SelectedLevel.ID);
            ReloadLevel();
        }
    }
}
