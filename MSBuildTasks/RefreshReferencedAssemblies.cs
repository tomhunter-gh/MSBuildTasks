using System.IO;
using Microsoft.Build.Utilities;

namespace MSBuildTasks
{
	public class RefreshReferencedAssemblies : Task
	{
		public string BinDirectory { get; set; }

		public override bool Execute()
		{
			Log.LogMessage("Refreshing referenced assemblies..");
			foreach (var refreshFilePath in Directory.GetFiles(BinDirectory, "*.refresh"))
			{
				var assemblyFilePath = Directory.GetParent(BinDirectory.TrimEnd(new[] { '\\' }))
					+ @"\" + File.ReadAllText(refreshFilePath);
				var assemblyDir = Path.GetDirectoryName(assemblyFilePath);
				var baseName = Path.GetFileNameWithoutExtension(assemblyFilePath);

				foreach (var file in Directory.GetFiles(assemblyDir, baseName + "*"))
				{
					var destinationFile = BinDirectory + @"\" + Path.GetFileName(file);
					Log.LogMessage(" Copying {0} to {1}", file, destinationFile);
					if (File.Exists(destinationFile))
						File.SetAttributes(destinationFile, FileAttributes.Normal);
					File.Copy(file, destinationFile, true);
				}
			}
			return true;
		}
	}
}