using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Models
{
	public class ShaderDto
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ShaderDto()
		{

		}

		public ShaderDto(ShaderType shaderType)
		{
			Id = shaderType.GetHashCode();
			Name = shaderType.ToString();
		}
	}
}
