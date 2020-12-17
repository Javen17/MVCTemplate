using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace Tests
{
	public class ConfigurationHelper
	{
		public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
		{
			return new ConfigurationBuilder()
		.SetBasePath(outputPath)
		.AddJsonFile("appsettings.json", optional: true)
		.AddEnvironmentVariables()
		.Build();

		}
	}
}