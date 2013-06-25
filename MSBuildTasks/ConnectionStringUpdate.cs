using System;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Build.Utilities;

namespace MSBuildTasks
{
	public class ConnectionStringUpdate : Task
	{
		public string ConfigFile { get; set; }

		public string Name { get; set; }

		public string DataSource { get; set; }
		public string Server { get; set; }
		public string Password { get; set; }

		public override bool Execute()
		{
			Log.LogMessage("Updating ConnectionString name \"{0}\".", Name);

			var xmlFileDoc = new XmlDocument();
			xmlFileDoc.PreserveWhitespace = true;
			xmlFileDoc.Load(ConfigFile);

			XmlNamespaceManager localnamespaceManager = new XmlNamespaceManager(xmlFileDoc.NameTable);

			var node = xmlFileDoc.SelectSingleNode(@"//configuration/connectionStrings/add[@name='" + Name + "']/@connectionString", localnamespaceManager);
			RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled;

			if (!String.IsNullOrEmpty(DataSource))
			{
				Log.LogMessage(" Setting Data Source: \"{0}\".", DataSource);
				node.Value = Regex.Replace(node.Value, @"Data Source=[^;]*", "Data Source=" + DataSource, options);
			}

			if (!String.IsNullOrEmpty(Server))
			{
				Log.LogMessage(" Setting Server: \"{0}\".", Server);
				node.Value = Regex.Replace(node.Value, @"Server=[^;]*", "Server=" + Server, options);
			}

			if (!String.IsNullOrEmpty(Password))
			{
				Log.LogMessage(" Setting Password: \"{0}\".", new String('*', Password.Length));
				node.Value = Regex.Replace(node.Value, @"password=[^;]*", "password=" + Password, options);
			}

			xmlFileDoc.Save(ConfigFile);

			return true;
		}
	}
}
