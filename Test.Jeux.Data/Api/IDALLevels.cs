using System.Collections.Generic;
using TestJeux.API.Models;

namespace Test.Jeux.Data.Api
{
    public interface IDALLevels
    {
        LevelDto GetDataById(int id);

        List<LevelDto> LoadAllLevels();

        void SaveLevel(LevelDto level);
    }
}