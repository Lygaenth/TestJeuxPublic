using System;
using System.Collections.Generic;
using TestJeux.Business.Managers.API;

namespace TestJeux.Business.Managers
{
	public class ImageManager : IImageManager
	{
		private const string Hero = "Hero";
		private const string Fish = "Fish";
		private const string Ground = "Ground";
		private const string Equipment = "Equipment";
		private const string Decoration = "Decoration";
		private const string Door = "Door";
		private const string PNJ = "PNJ";
		private const string Torch = "Torch";
		private const string Chest = "Chest";
		private const string Caisse = "Caisse";
		private const string Wall = "Wall";

		private readonly Dictionary<string, string> _images;
		public ImageManager()
		{
			_images = new Dictionary<string, string>();

			Load();
		}

		public List<string> GetImages(List<string> codes)
		{
			var images = new List<string>();
			foreach (var code in codes)
				images.Add(GetImage(code));
			return images;
		}

		public string GetImage(string code)
		{
			if (_images.ContainsKey(code))
				return _images[code];
			else
				return String.Empty;
		}

		private void Load()
		{
			// Hero
			_images["HeroFront"] = GetPath(Hero, "Perso_front");
			_images["HeroFront2"] = GetPath(Hero, "Perso_front2");
			_images["HeroLeft"] = GetPath(Hero, "Perso_Left");
			_images["HeroRight"] = GetPath(Hero, "Perso_Right");
			_images["HeroBack"] = GetPath(Hero, "Perso_back");
			_images["HeroDeath"] = GetPath(Hero, "Perso_death");
			_images["HeroFront_walking1"] = GetPath(Hero, "Perso_front_walking1");
			_images["HeroFront_walking2"] = GetPath(Hero, "Perso_front_walking2");
			_images["HeroFront_walking3"] = GetPath(Hero, "Perso_front_walking3");
			_images["HeroFront_walking4"] = GetPath(Hero, "Perso_front_walking4");
			_images["HeroRight_walking1"] = GetPath(Hero, "Perso_right_walking1");
			_images["HeroRight_walking2"] = GetPath(Hero, "Perso_right_walking2");
			_images["HeroLeft_walking1"] = GetPath(Hero, "Perso_left_walking");
			_images["HeroLeft_walking2"] = GetPath(Hero, "Perso_left_walking");
			_images["HeroBack_walking1"] = GetPath(Hero, "Perso_back_walking1");
			_images["HeroBack_walking2"] = GetPath(Hero, "Perso_back_walking2");
			_images["HeroBack_walking3"] = GetPath(Hero, "Perso_back_walking3");
			_images["HeroBack_walking4"] = GetPath(Hero, "Perso_back_walking4");
			_images["HeroFront_swimming1"] = GetPath(Hero, "Perso_front_swimming_1");
			_images["HeroFront_swimming2"] = GetPath(Hero, "Perso_front_swimming_2");
			_images["HeroLeft_swimming1"] = GetPath(Hero, "Perso_Left_swimming1");
			_images["HeroLeft_swimming2"] = GetPath(Hero, "Perso_Left_swimming2");
			_images["HeroRight_swimming1"] = GetPath(Hero, "Perso_Right_swimming1");
			_images["HeroRight_swimming2"] = GetPath(Hero, "Perso_Right_swimming2");
			_images["HeroBack_swimming"] = GetPath(Hero, "Perso_Back_swimming");

			// Guardian
			_images["GoblinFront"] = GetPath(PNJ, "Goblin_front");
			_images["GoblinLeft"] = GetPath(PNJ, "Goblin_Left");
			_images["GoblinRight"] = GetPath(PNJ, "Goblin_Right");
			_images["GoblinBack"] = GetPath(PNJ, "Goblin_Back");
			_images["GoblinDeath"] = GetPath(PNJ, "DeadGoblin");

			// Fisher
			_images["FisherRight"] = GetPath(PNJ, "FisherFishing");

			// Fish
			_images["FishFront1"] = GetPath(Fish, "Fish_front1");
			_images["FishFront2"] = GetPath(Fish, "Fish_front2");
			_images["FishLeft"] = GetPath(Fish, "Fish_left");
			_images["FishRight"] = GetPath(Fish, "Fish_right");
			_images["FishBack"] = GetPath(Fish, "Fish_back");

			// Object
			_images["Caisse"] = GetPath(Caisse, "Caisse");
			_images["SubmergedCaisse"] = GetPath(Caisse, "SubmergedCaisse");

			_images["Chest"] = GetPath(Chest, "Chest");
			_images["OpenedChest"] = GetPath(Chest, "OpenedChest");

			_images["Torch1"] = GetPath(Torch, "Torch1");
			_images["Torch2"] = GetPath(Torch, "Torch2");
			_images["Torch3"] = GetPath(Torch, "Torch3");
			_images["TorchOff"] = GetPath(Torch, "TorchOff");

			_images["BreakableWall"] = GetPath(Wall, "BreakableWall");
			_images["BrokenWallVertical"] = GetPath(Wall, "BrokenWallVertical");
			_images["BrokenWall"] = GetPath(Wall, "BrokenWall");

			_images["Bush"] = GetPath("Bush");

			_images["Door"] = GetPath(Door, "Door");
			_images["OpenedDoor"] = GetPath(Door, "OpenedDoor");

			// Equipment
			_images["Maillot"] = GetPath(Equipment, "Maillot");
			_images["Swimsuit"] = GetPath(Equipment, "Maillot");
			_images["TorchItem"] = GetPath(Equipment, "torchItem");
			_images["PickAxe"] = GetPath(Equipment, "pickAxe");
			_images["DoorKey"] = GetPath(Equipment, "Key");
			_images["Key"] = GetPath(Equipment, "Key");
			_images["Knife"] = GetPath(Equipment, "Knife");

			// Ground
			_images["Grass"] = GetPath(Ground, "Grass1");
			_images["Water"] = GetPath(Ground, "Water1");
			_images["Grass1"] = GetPath(Ground, "Grass1");
			_images["Water1"] = GetPath(Ground, "Water1");
			_images["WaterSide"] = GetPath(Ground, "WaterSideTop");
			_images["WaterSide2"] = GetPath(Ground, "WaterSide2");
			_images["CaveFloor"] = GetPath(Ground, "CaveFloor");
			_images["CaveWall"] = GetPath(Ground, "CaveWall");
			_images["CaveWallOneSide"] = GetPath(Ground, "CaveWallOneSide");
			_images["CaveWallOneCorner"] = GetPath(Ground, "CaveWallCorner");
			_images["CaveWallOpposedSide"] = GetPath(Ground, "CaveWallOpposedSide");

			// Decoration
			_images["Bridge"] = GetPath(Decoration, "Bridge");
			_images["DustPath"] = GetPath(Decoration, "DustPath");
			_images["Cave"] = GetPath(Decoration, "Cave");
			_images["CaveEntry"] = GetPath(Decoration, "CaveEntry");
			_images["PineTree"] = GetPath(Decoration, "PineTree");
			_images["Ladder"] = GetPath(Decoration, "Ladder");
			_images["CaveUp"] = GetPath(Decoration, "CaveUp");

			// Various
			_images["MagicTofu"] = GetPath(Equipment, "Magic_tofu");
		}

		private string GetPath(string code)
		{
			return @".\Ressource\Sprites\" + code + ".png";
		}

		private string GetPath(string folder, string code)
		{
			return GetPath(folder + @"\" + code);
		}

		public void Reset()
		{
			_images.Clear();
		}

		public void Subscribe()
		{
			// Nothing to do
		}

		public void Unsubscribe()
		{
			// Nothing to do
		}
	}
}
