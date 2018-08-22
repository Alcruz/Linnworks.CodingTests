using System.IO;
using System.Reflection;

namespace Linnworks.CodingTests.Part1.Server.Common
{
	// https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
	public static class EmbeddedResourceHelpers
	{
		public static string ReadFile(string resourceName)
		{
			var assembly = Assembly.GetCallingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
				return reader.ReadToEnd();
		}
	}
}
