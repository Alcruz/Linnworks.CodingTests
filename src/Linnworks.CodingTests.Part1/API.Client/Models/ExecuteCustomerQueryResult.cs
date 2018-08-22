namespace Linnworks.CodingTests.Part1.Server.API.Client.Models
{
	public class ExecuteCustomScriptResult<TResult>
	{
		public bool IsError { get; set; }
		public string ErrorMessage { get; set; }
		public long TotalResults { get; set; }
		public CustomScriptColumn[] Columns { get; set; }
		public TResult[] Results { get; set; }
	}

	public class CustomScriptColumn
	{
		public int Index { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
	}
}