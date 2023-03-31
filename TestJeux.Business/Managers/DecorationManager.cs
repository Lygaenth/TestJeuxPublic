using System.Collections.Generic;
using TestJeux.API.Models;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers
{
	public class DecorationManager
    {
        public DecorationManager()
        {

        }

        public List<Decoration> GenerateDecorations(List<DecorationDto> decorationDtos)
        {
            var decorations = new List<Decoration>();
            int id = 0;
            foreach (var deco in decorationDtos)
            {
                decorations.Add(new Decoration(id, deco.Decoration, deco.TopLeft));
                id++;
            }

            return decorations;
        }

        private string GetDecorationSprite(Decorations decoration)
        {
            return decoration switch
            {
                Decorations.DustPath => "DustPath",
                Decorations.PineTree => "PineTree",
                Decorations.CaveEntry => "CaveEntry",
                Decorations.Cave => "Cave",
                Decorations.Bridge => "Bridge",
                Decorations.Ladder => "Ladder",
                _ => "Error"
            };
        }
    }
}
