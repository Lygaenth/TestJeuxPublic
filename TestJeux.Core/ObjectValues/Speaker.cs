using System;
using System.Collections.Generic;
using System.Text;

namespace TestJeux.Core.ObjectValues
{
	public class Speaker
	{
		public string Icon { get; }
		public string Name { get; }
		public IReadOnlyList<string> Lines { get; }

		public Speaker(string icon, string name, List<string> lines)
		{
			Icon = icon;
			Name = name;
			Lines = lines;
		}
	}
}
