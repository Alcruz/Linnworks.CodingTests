using Linnworks.CodingTests.Part1.Server.API.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linnworks.CodingTests.Part1.Server.API.Client
{
	public class LinnworksBadRequestException : InvalidOperationException
	{
		public LinnworksBadRequestException(ErrorResponse errorResponse)
		{
			ErrorResponse = errorResponse;
		}

		public ErrorResponse ErrorResponse { get; }
	}
}
