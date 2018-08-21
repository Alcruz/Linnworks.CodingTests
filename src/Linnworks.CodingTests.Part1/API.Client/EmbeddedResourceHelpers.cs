using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Linnworks.CodingTests.Part1.Server.API.Client
{
	// https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
	internal static class EmbeddedResourceHelpers
	{
		public static string ReadFile(string resourceName)
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
				return reader.ReadToEnd();
		}
	}
}
