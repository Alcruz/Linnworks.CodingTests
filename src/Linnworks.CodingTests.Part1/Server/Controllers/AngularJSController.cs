using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linnworks.CodingTests.Part1.Server.Controllers
{
	public class AngularJSController : ControllerBase
	{
		[HttpGet]
		public FileResult Index()
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			string indexResourceName = $"{executingAssembly.EntryPoint.DeclaringType.Namespace}.index.html";

			string indexText;

			using (Stream resourceStream = executingAssembly.GetManifestResourceStream(indexResourceName))
			using (StreamReader reader = new StreamReader(resourceStream))
			{
				indexText = reader.ReadToEnd();
			}

			return new FileContentResult(Encoding.UTF8.GetBytes(indexText), "text/html");
		}
	}
}
