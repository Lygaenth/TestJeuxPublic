using System;
using System.Collections.Generic;
using System.Text;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.ObjectValues
{
	public class MakeSound
	{
		public SoundEffects Sound { get; }
		public bool WaitForEnd { get; }
		
		public MakeSound(SoundEffects sound, bool waitForEnd)
		{
			Sound = sound;
			WaitForEnd = waitForEnd;
		}
	}
}
