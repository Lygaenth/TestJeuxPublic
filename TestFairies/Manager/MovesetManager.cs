using System;
using System.Collections.Generic;
using TestFairies.Enums;
using TestFairies.Models;
using TestFairies.Models.Effects;

namespace TestFairies.Manager
{
    public class MovesetManager
    {
        private readonly Dictionary<MoveId, Move> _moveDictionary;

        public MovesetManager()
        {
            _moveDictionary = new Dictionary<MoveId, Move>();
            Initialize();
        } 

        public Move GetMove(MoveId id)
        {
            if (_moveDictionary.ContainsKey(id))
                return _moveDictionary[id];

            throw new Exception("Unknown move");
        }

        private void Initialize()
        {
            var flameAttack = new Effect(EffectType.Attack, TargetType.Opponent, 80, 0);             
            _moveDictionary[MoveId.Flame] = new Move("Flame", new List<Effect> { flameAttack });

            var rageBuff = new BuffEffect("Rage", TargetType.Self, Characteristic.Creativity, StatusEffectType.AppliedForSetDuration, 200, 3, "{0} is entering a rage state");
            _moveDictionary[MoveId.Rage] = new Move("Rage", new List<Effect> { rageBuff });

            var burnBuff = new BuffEffect("Burn", TargetType.Opponent, Characteristic.Morale, StatusEffectType.AppliedForSetDuration, 5, 5, "{0} is slowly burning");
            _moveDictionary[MoveId.Ember] = new Move("Ember", new List<Effect> { burnBuff });
        }
    }
}
