using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HObjectToHtml
{
    public static class ExtensionMethods
    {
		public static string JustDigits(this string s)
		{
			try
			{
				if (s == null)
				{
					return null;
				}

				return new string(s.Where(char.IsDigit).ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception("JustDigits threw error: "+ex.Message, ex);
			}
		}
		
		public static Dictionary<string, string> AnonymousToDictonary(this object o)
		{
			return AnonymousToDictionary(o);
		}
		public static T[] AsSingleElementArray<T>(this T el)
        {
			return new []{ el};
        }
		public static Dictionary<string, string> AnonymousToDictionary(object o)
		{
			Dictionary<string, object> source = (Dictionary<string, object>)o.ToJson().FromJson<object>();
			return source.ToDictionary((KeyValuePair<string, object> d) => d.Key, (KeyValuePair<string, object> v) => v.Value?.ToString());
		}
		
		public static bool HasText(this string s)
		{
			return !string.IsNullOrWhiteSpace(s);
		}
		public static string Justaz09(this string s)
		{
			Regex regex = new Regex("[^a-zA-Z0-9]");
			string text = regex.Replace(s, "");
			return text.ToLower();
		}
		public static T FromJson<T>(this string t)
		{
			try
			{
				JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
				javaScriptSerializer.MaxJsonLength = 9999999;
				return javaScriptSerializer.Deserialize<T>(t);
			}
			catch (Exception ex)
			{
				throw new Exception("Cannot convert JSON string to " + typeof(T).FullName + " " + ex.Message, ex);
			}
		}
		public static bool ContainsInCI(this string val, params string[] vals)
		{
			if (val == null)
			{
				return false;
			}

			return vals.Any((string p) => val.ToLower().Contains(p.ToLower()));
		}
		
		public static string FlattenText(this IEnumerable<string> stringList, string seperator = ", ")
		{
			if (stringList.Count() == 0)
			{
				return "";
			}

			return stringList.Aggregate((string current, string next) => current + seperator + next);
		}
		public static string[] Split(this string s, string sp, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
		{
			if (s == null)
			{
				throw new Exception("string cannot be null in HSplit");
			}

			return s.Split(new string[1]
			{
				sp
			}, stringSplitOptions);
		}
		public static bool IsAny(this string val, params string[] vals)
		{
			foreach (string b in vals)
			{
				if (val == b)
				{
					return true;
				}
			}

			return false;
		}
		public static string ToJson(this object obj)
		{
			if (obj == null)
			{
				return null;
			}
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			javaScriptSerializer.MaxJsonLength = 9999999;
			javaScriptSerializer.RecursionLimit = 12;
			string text = javaScriptSerializer.Serialize(obj);

			return text;
		}
	}
}
