using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HObjectToHtml
{
    public static class Template
	{
		public static string StartVal = "${";

		public static string EndVal = "}";

		[DebuggerStepThrough]
		public static Dictionary<string, string> GetDictionary(params string[] KeyDataPairs)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (KeyDataPairs.Length % 2 != 0)
			{
				throw new Exception("Get Dictionary must be called with an even number of parameters.  Maybe you don't understand what this function does.");
			}
			for (int i = 0; i < KeyDataPairs.Length; i += 2)
			{
				dictionary[KeyDataPairs[i]] = KeyDataPairs[i + 1];
			}
			return dictionary;
		}

		[DebuggerStepThrough]
		public static string ParseTemplateFromString(string templatedata, Dictionary<string, string> values, List<Dictionary<string, string>> loop = null)
		{
			string text = templatedata;
			if (loop != null && templatedata.Contains("%loop%"))
			{
				string loopContents = GetLoopContents(templatedata);
				templatedata = templatedata.Substring(0, templatedata.IndexOf("%loop%")) + "%%%loopcontents%%%" + templatedata.Substring(templatedata.IndexOf("%end loop%") + 10);
				string text2 = "";
				foreach (Dictionary<string, string> item in loop)
				{
					text2 += ParseTemplateFromString(loopContents, item);
				}
				templatedata = templatedata.Replace("%%%loopcontents%%%", text2);
			}
			foreach (KeyValuePair<string, string> value in values)
			{
				templatedata = templatedata.Replace(StartVal + value.Key + EndVal, value.Value ?? "");
			}
			return templatedata;
		}

		public static string ParseTemplateFromStringAnon(string templatedata, object d)
		{
			try
			{
				Dictionary<string, object> source = (Dictionary<string, object>)d.ToJson().FromJson<object>();
				return ParseTemplateFromString(templatedata, source.ToDictionary((KeyValuePair<string, object> v) => v.Key, (KeyValuePair<string, object> b) => b.Value?.ToString()));
			}
			catch (Exception exc)
			{
				throw new Exception("Error parsing template, object has to be a simple anonymousobject, see inner exception for details: "+exc.Message, exc);
			}
		}

		public static string GetLoopContents(string templatedata)
		{
			string text = "";
			text = templatedata.Substring(templatedata.IndexOf("%loop%"), templatedata.IndexOf("%end loop%") - templatedata.IndexOf("%loop%"));
			return text.Replace("%loop%", "").Replace("%end loop%", "");
		}
	}
}
