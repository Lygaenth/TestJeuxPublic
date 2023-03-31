using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFairies.Enums;
using TestFairies.Models.Effects;
using TestFairies.Models;

namespace TestFairies
{
    public class FightManager
    {

        public void ManageFight(Artist player, Artist opponent)
        {
            Console.WriteLine("Starting fight with initial state:");
            Console.WriteLine("");
            DisplayState(player, opponent);

            while (!opponent.HasLost())
            {
                foreach (var fairy in player.Fairies)
                {
                    var selectedMove = RequestFairyAction(fairy);
                    var target = opponent.Fairies[0];

                    foreach (var effect in selectedMove.Effects)
                    {
                        if (effect.EffectType == EffectType.Attack)
                        {
                            Console.WriteLine(fairy.Name + " uses " + selectedMove.Name + " on " + target.Name);
                            var attackDmg = (player.Creativity + fairy.Creativity) * effect.Potency / 100;
                            target.DamageMorale(-attackDmg);
                            Console.WriteLine(target.Name + " takes " + attackDmg + " damages");
                        }
                        else if (effect.EffectType == EffectType.Buff)
                        {
                            if (!(effect is BuffEffect buff))
                                continue;

                            if (buff.TargetType == TargetType.Self)
                            {
                                buff.Execute(fairy);
                                Console.WriteLine(String.Format(effect.Description, fairy.Name));
                            }
                            else
                            {
                                buff.Execute(target);
                                Console.WriteLine(String.Format(effect.Description, target));
                            }
                        }
                    }
                }

                // End turn actions
                ManageEndTurnActions(player);
                ManageEndTurnActions(opponent);

                Console.WriteLine();
                DisplayState(player, opponent);
                Console.WriteLine();
            }
            Console.WriteLine("Dummy is beaten.");
        }

        private void ManageEndTurnActions(Artist artist)
        {
            foreach (var fairy in artist.Fairies)
            {
                foreach (var effect in fairy.StatusEffects)
                {
                    if (effect.EffectType == StatusEffectType.EffectOnTime)
                        effect.Action.Invoke();
                    effect.Duration--;
                    if (effect.Duration == 0 && effect.EffectType != StatusEffectType.EffectOnTime)
                        effect.Action.Invoke();
                }
                fairy.StatusEffects.RemoveAll(fairy => fairy.Duration == 0);
            }
        }

        private void DisplayState(Artist player, Artist opponent)
        {
            DisplayArtistsumUp(player);
            Console.WriteLine();
            DisplayArtistsumUp(opponent);
            Console.WriteLine();
        }

        private void DisplayArtistsumUp(Artist artist)
        {
            Console.WriteLine(artist.ToString());
            foreach (var fairy in artist.Fairies)
            {
                Console.WriteLine(fairy.ToString());
                foreach (var effect in fairy.StatusEffects)
                    Console.WriteLine(effect.ToString());
            }
        }

        private Move RequestFairyAction(Fairy fairy)
        {
            Console.WriteLine(fairy.GetMovesOptions());
            Move selectedMove = null;
            while (selectedMove == null)
            {
                Console.WriteLine("What will " + fairy.Name + " do ?");
                var attack = Console.ReadLine();
                if (Int32.TryParse(attack, out int moveNumber))
                    selectedMove = fairy.Moves[moveNumber - 1];
            }
            return selectedMove;
        }
    }
}
