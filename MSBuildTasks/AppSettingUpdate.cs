using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using System.Xml;

namespace BlueBay.MSBuildTasks
{
	public class AppSettingUpdate : Task
	{
		public string ConfigFile
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}
		
		public override bool Execute()
		{
			Log.LogMessage("Updating AppSetting key \"{0}\" with value \"{1}\"", Key, Value);
			var xmlFileDoc = new XmlDocument();
			xmlFileDoc.PreserveWhitespace = true;
			xmlFileDoc.Load(ConfigFile);

			XmlNamespaceManager localnamespaceManager = new XmlNamespaceManager(xmlFileDoc.NameTable);

			var node = xmlFileDoc.SelectSingleNode(@"//configuration/appSettings/add[@key='" + Key + "']/@value", localnamespaceManager);

			if (node == null)
				throw new Exception(String.Format("Could not find AppSetting key \"{0}\" in config file \"{1}\"", Key, ConfigFile));

			node.Value = Value;

			xmlFileDoc.Save(ConfigFile);

			return true;
		}
	}
}
