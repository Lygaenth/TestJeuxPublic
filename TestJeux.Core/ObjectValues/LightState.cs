namespace TestJeux.Business.ObjectValues
{
	/// <summary>
	/// Light state objet value
	/// </summary>
	public class LightState
	{
		public bool IsLit { get; }
		public int Intensity { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="isLit"></param>
		/// <param name="intensity"></param>
		public LightState(bool isLit, int intensity)
		{
			IsLit = isLit;
			Intensity = intensity;
		}

		/// <summary>
		/// Turn of the light
		/// </summary>
		/// <returns></returns>
		public LightState TurnOff()
		{
			return new LightState(false, this.Intensity);
		}

		/// <summary>
		/// Tur
		/// </summary>
		/// <returns></returns>
		public LightState Light()
		{
			return new LightState(true, this.Intensity);
		}

		public LightState Intensify(int intensity)
		{
			return new LightState(IsLit, intensity);
		}
	}
}
