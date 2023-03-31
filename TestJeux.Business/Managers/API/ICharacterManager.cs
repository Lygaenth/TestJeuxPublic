using System.Collections.Generic;
using TestJeux.API.Models;
using TestJeux.API.Services;
using TestJeux.Core.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers.API
{
	public interface ICharacterManager : IService
    {
        ItemModel GetCharacter(int itemId);

        ItemModel GetCharacter(int itemId, ItemCode code);

        ItemStateDto GetCharacterState(int itemId);

        List<string> GetCurrentSprites(int itemId);

        void SetCharacterOrientation(int itemId, DirectionEnum direction);

        DirectionEnum GetCharacterOrientation(int itemId);
    }
}