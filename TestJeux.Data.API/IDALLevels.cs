using System.Collections.Generic;
using TestJeux.API.Models;

namespace TestJeux.Data.Api
{
    public interface IDALLevels
    {
        LevelDto GetDataById(int id);

        List<LevelDto> LoadAllLevels();

        void SaveLevel(LevelDto level);
    }
}