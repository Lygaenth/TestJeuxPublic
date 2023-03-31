using System;
using System.Collections.Generic;
using System.Drawing;
using TestJeux.API.Models;
using TestJeux.API.Services;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Services.API
{
	public class LevelChangeArgs : EventArgs
    {
        public int Level { get; private set; }

        public LevelChangeArgs(int level)
        {
            Level = level;
        }
    }

    public delegate void LevelChange(object sender, LevelChangeArgs args);

    public interface ILevelService : IService
    {
        int GetCurrentLevel();

        LevelDto GetLevel(int id);

        List<int> GetAllLevelIds();

        GroundType GetGroundType(int levelId, Point position);

        void ChangeLevel(int id);

        void SaveLevel(LevelDto levelDto);

        event LevelChange RaiseLevelChange;
    }
}