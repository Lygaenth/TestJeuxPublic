using System.Collections.Generic;
using TestJeux.API.Services.TextInteractions;
using TestJeux.Business.Action;
using TestJeux.Business.Managers.API;
using TestJeux.Business.Services.API;
using TestJeux.Core.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Action
{
	public class ActionFabric
    {
        ILevelService _levelManager;
        IMoveService _moveManager;
        IEquipmentManager _equipmentManager;
        IChatService _chatManager;
        ISoundService _soundManager;

        public ActionFabric(ILevelService levelManager,
            IMoveService moveManager,
            IEquipmentManager equipmentManager,
            IChatService chatManager,
            ISoundService soundManager)
        {
            _levelManager = levelManager;
            _moveManager = moveManager;
            _equipmentManager = equipmentManager;
            _chatManager = chatManager;
            _soundManager = soundManager;
        }

        public EffectAction CreateEffectAction(ItemModel source, ItemModel target, string property, int modifier)
        {
            return new EffectAction(source, target, property, modifier);
        }

        public MoveAction CreateMoveAction(ItemModel item, GroundType groundType)
        {
            return new MoveAction(ActionType.Move, _moveManager, item, groundType, true);
        }

        public MoveAction CreatePjMoveAction(ItemModel item, GroundType groundType)
        {
            return new MoveAction(ActionType.MovePj, _moveManager, item, groundType, true);
        }

        public SpeakAction CreateSpeakAction(List<SpeakerDto> lines)
        {
            return new SpeakAction(_chatManager, lines);
        }

        public InventoryAction CreateInventoryAction(ItemModel item, ItemAction action, EquipmentCode code)
        {
            return new InventoryAction(_equipmentManager, item, action, code);
        }

        public LevelChangeAction CreateLevelChangeAction(int id)
        {
            return new LevelChangeAction(_levelManager, id);
        }

        public RemoveItemAction CreateRemoveAction(ItemModel item)
        {
            return new RemoveItemAction(item);
        }

        public SoundAction CreateSoundAction(SoundEffects soundEffect, bool waitForEnd)
        {
            return new SoundAction(_soundManager, soundEffect, waitForEnd);
        }

        public ActionChangeState CreateChangeStateAction(System.Action action)
        {
            return new ActionChangeState(action);
        }
    }
}
