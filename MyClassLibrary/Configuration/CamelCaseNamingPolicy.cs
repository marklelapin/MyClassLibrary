
using System.Text.Json;

namespace MyClassLibrary.Configuration
{
	public class CamelCaseNamingPolicy : JsonNamingPolicy
	{
		public override string ConvertName(string name)
		{
			// Convert the name to camelCase
			if (string.IsNullOrEmpty(name))
				return name;
			if (name.Length == 1)
				return name.ToLowerInvariant();
			return char.ToLowerInvariant(name[0]) + name.Substring(1);
		}
	}
}
