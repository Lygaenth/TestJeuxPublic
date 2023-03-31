using System.Collections.Generic;
using TestJeux.API.Services;

namespace TestJeux.Business.Managers.API
{
    public interface IImageManager : IService
	{
		string GetImage(string code);
		List<string> GetImages(List<string> codes);
	}
}