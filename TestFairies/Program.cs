using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using TestFairies.Enums;
using TestFairies.Manager;
using TestFairies.Models;
using TestFairies.Models.Effects;

namespace TestFairies
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********************** Welcome to imaginary fight *********************");
            Console.WriteLine("Initializing");
            var moveSetManager = new MovesetManager();

            var artist1 = new Artist("Artist1", 10, 10);
            var fairy1 = new Fairy("FireFairy", ElementType.Fire, 10, 20);

            fairy1.Moves.Add(moveSetManager.GetMove(MoveId.Flame));
            fairy1.Moves.Add(moveSetManager.GetMove(MoveId.Rage));
            fairy1.Moves.Add(moveSetManager.GetMove(MoveId.Ember));

            artist1.Fairies.Add(fairy1);
            var dummyArtist = new Artist("DummyArtist", 0, 0);
            var dummy = new Fairy("Dummy", ElementType.Neutral, 0, 100);
            dummyArtist.Fairies.Add(dummy);

            var fightManager = new FightManager();
            fightManager.ManageFight(artist1, dummyArtist);
            
            Console.WriteLine("Hit any key to quit.");
            Console.ReadLine();
        }

    }
}
