using LevelEditor.wpf.Builders;
using LevelEditor.wpf.ViewModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using Test.Jeux.Data;
using TestJeux.API.Models;
using TestJeux.Business.Command;
using TestJeux.Business.Managers;
using TestJeux.Business.Managers.API;
using TestJeux.Business.Services;
using TestJeux.Business.Services.API;
using TestJeux.Business.Supervisor;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities.Items;
using TestJeux.Display.Helper;
using TestJeux.Display.ViewModels.Display.Levelelements;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor
{
	public class LevelEditorViewModel : BindableBase
    {
        private bool _showZones;
        private bool _showDecorations;
        private readonly ILevelService _levelService;
        private readonly ICharacterManager _characterManager;
        private readonly IImageManager _imageManager;
        private readonly IEquipmentManager _equipmentManager;
        private readonly ITileService _tileManager;
        private readonly DecorationManager _decorationManager;
        private readonly ICharacterBuilder _characterBuilder;
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
                    SelectedTile.UpdateSprites(ImageHelper.GetImages(_tileManager.GetTileSprites(SelectedTile.GroundSprite)));
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

        public ZoneViewModel SelectedZone { get => _selectedZone; set => SetProperty(ref _selectedZone, value); }

        public LevelEditorViewModel()
        {
            var gameRoot = new GameAggregate();

            Levels = new ObservableCollection<LevelViewModel>();
            Characters = new ObservableCollection<ItemModel>();
            DebugZones = new ObservableCollection<ZoneViewModel>();
            _imageManager = new ImageManager();
            _equipmentManager = new EquipmentManager(gameRoot);
            _characterManager = new CharacterManager(gameRoot);
            _levelService = new LevelService(gameRoot, new DALLevels());
            _tileManager = new TileService(gameRoot);
            _decorationManager = new DecorationManager();

            _characterBuilder = new CharacterBuilderEditor(_characterManager);


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
                TilesSprites.Add(new TileSpriteInfo(groundSprite, ImageHelper.GetImages(_tileManager.GetTileSprites(groundSprite))[0]));
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
                var levelVm = new LevelViewModel(_levelService, _tileManager, _characterManager, _characterBuilder);
                levelVm.LoadLevel(id);
				Levels.Add(levelVm);
            }

            if (Levels.Count == 0)
                Levels.Add(new LevelViewModel(_levelService, _tileManager, _characterManager, _characterBuilder));
            
            SelectedLevel = Levels[0];
            if (SelectedLevel.Tiles.Count == 0)
            {
                var defaultTile = new TileViewModel(ImageHelper.GetImages(new List<string> { "Grass"}), GroundSprite.Grass, 0, 0, 0);
                SelectedLevel.Tiles.Add(defaultTile);
            }

            SelectedTile = SelectedLevel.Tiles[0];

            if (DebugZones.Count > 0)
                SelectedZone = DebugZones[0];
        }

        private void LoadLevel(LevelViewModel level)
        {
            var dto = _levelService.GetLevel(level.ID);
            if (dto.ID == -1)
                return;

            level = new LevelViewModel(_levelService, _tileManager, _characterManager, _characterBuilder);
            
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
    
        private void AddZone()
        {
            var zone = new ZoneViewModel(DebugZones.OrderBy(z => z.ID).Last().ID + 1, new System.Drawing.Point(0, 0), 100, 100, GroundType.Block);
            DebugZones.Add(zone);
        }

        private void DeleteZone()
        {
            if (SelectedZone == null)
                return;

            DebugZones.Remove(SelectedZone);

            SelectedZone = DebugZones[0];
        }

        private void SaveCurrentLevel()
        {
            var dal = new DALLevels();
            var levelDto = new LevelDto();
            levelDto.ID = SelectedLevel.ID;
            levelDto.LevelMusic = SelectedLevel.Music;
            levelDto.Shader = SelectedLevel.Shader;
            levelDto.DefaultTile = 1;

            foreach(var zone in _selectedLevel.Zones)
            {
                var zoneDto = new ZoneDto();
                zoneDto.BottomRight = new Point(zone.X + zone.Width, zone.Y+zone.Heigth);
                zoneDto.TopLeft = new Point(zone.X, zone.Y);
                zoneDto.ID = zone.ID;
                zoneDto.GroundType = zone.GroundType;
                levelDto.Zones.Add(zoneDto);
            }

            foreach(var tile in _selectedLevel.Tiles)
            {
                var tileDto = new TileZoneDto();
                tileDto.Angle = tile.Angle;
                tileDto.BottomRight = new Point(tile.X + 50, tile.Y + 50);
                tileDto.TopLeft = new Point(tile.X, tile.Y);
                tileDto.Tile = tile.GroundSprite;
                levelDto.TilesZones.Add(tileDto);
            }

            foreach(var decoration in _selectedLevel.Decorations)
            {
                var decorationDto = new DecorationDto();
                decorationDto.Decoration = decoration.Decoration;
                decorationDto.TopLeft = new Point(decoration.X, decoration.Y);
                levelDto.Decorations.Add(decorationDto);
            }

            foreach(var item in _selectedLevel.Items)
            {
                var itemDto = new ItemDto();
                itemDto.StartPosition = item.Position;
                itemDto.Code = item.Code;
                // Make other view model for editor since some values can be modified that shouldn't in game
                itemDto.DefaultState = 0;
            }

            _levelService.SaveLevel(levelDto);
        }
    }
}
