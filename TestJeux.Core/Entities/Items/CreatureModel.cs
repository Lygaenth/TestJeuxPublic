using TestJeux.Business.Entities.Sprites;
using TestJeux.SharedKernel.Enums;
using TestJeux.Business.ObjectValues;
using TestJeux.Core.ObjectValues;
using System;
using System.Collections.Generic;

namespace TestJeux.Core.Entities.Items
{
	public abstract class CreatureModel : ItemModel, ILightSource
    {
        List<string> _lastSpriteCodes;

        public bool CanMove { get; set; }

        public bool IsIdle { get; set; }

		public event EventHandler<LightState> LightSourceChanged;

		private LightState _lightState;
		public LightState LightState
		{
			get => _lightState;
			set
			{
				_lightState = value;
				if (LightSourceChanged != null)
					LightSourceChanged(this, LightState);
			}
		}

		public CreatureModel(int id)
            : base(id)
        {
            ItemType = ItemType.Npc;
            _lastSpriteCodes = new List<string>();
        }

        public override void Initialize()
        {
            base.Initialize();
            CurrentSprites = GetSprites(SpriteModel.Front);
            Orientation = DirectionEnum.Bottom;
            CanMove = true;
        }

        public CreatureSpriteModel CreatureSpriteModel
        {
            get => SpriteModel as CreatureSpriteModel;
        }

        protected Speaker GetLines(string imageCode, string name, List<string> lines)
        {
            return new Speaker(imageCode, name, lines);
        }

        public override void Refresh()
        {
            if (_isSpecialSprite)
                return;
            var spriteCodes = new List<string>();

            switch (_orientation)
            {
                case DirectionEnum.Left:
                    spriteCodes = IsMoving ? SpriteModel.MovingLeft : SpriteModel.Left;
                    break;
                case DirectionEnum.Right:
                    spriteCodes = IsMoving ? SpriteModel.MovingRight : SpriteModel.Right;
                    break;
                case DirectionEnum.Top:
                    spriteCodes = IsMoving ? SpriteModel.MovingBack : SpriteModel.Back;
                    break;
                case DirectionEnum.Bottom:
                    spriteCodes = IsMoving ? SpriteModel.MovingFront : SpriteModel.Front;
                    break;
            }
            if (_lastSpriteCodes == spriteCodes)
                return;

            CurrentSprites = GetSprites(spriteCodes);
            _lastSpriteCodes = spriteCodes;
        }
    
        
    }
}
