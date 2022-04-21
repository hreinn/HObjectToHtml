using HObjectToHtml.Convert;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HObjectToHtml
{
    public class ColumnSetup
	{
		public bool Manual = false;

		public string Url;

		public string Name;

		public bool Editable;

		public DataType Type;

		public List<Option> Options = new List<Option>();

		public bool PrimaryKey;

		public string ImageUrl
		{
			get;
			set;
		}

		public string ImageMaxWidthString
		{
			get;
			set;
		}

		public string ImageMaxHeightString
		{
			get;
			set;
		}

		public bool Visible
		{
			get;
			set;
		} = true;


		public bool PageBreak
		{
			get;
			set;
		}

		public string Classstring
		{
			get;
			set;
		}

		public object Attributes
		{
			get;
			set;
		}

		public bool Pre
		{
			get;
			set;
		}

		public string ManualValue
		{
			get;
			set;
		}

		public ColumnSetup AsManual(string manualValue = null)
		{
			Manual = true;
			ManualValue = manualValue;
			return this;
		}

		public static ColumnSetup ForUrl(string name, string url, object attributes = null)
		{
			return new ColumnSetup
			{
				Name = name,
				Url = url,
				Attributes = attributes
			};
		}

		public static ColumnSetup ForUrlNewWindow(string name, string url, string target = null)
		{
			return new ColumnSetup
			{
				Name = name,
				Url = url,
				Attributes = new
				{
					target = (target ?? "_blank")
				}
			};
		}

		public static ColumnSetup ForPre(string name, object attributes = null)
		{
			return new ColumnSetup
			{
				Pre = true,
				Name = name,
				Attributes = attributes
			};
		}

		public static ColumnSetup WithClass(string name, string classstring, object attributes = null)
		{
			return new ColumnSetup
			{
				Name = name,
				Classstring = classstring,
				Attributes = attributes
			};
		}

		public static ColumnSetup WithAttributes(string name, object o)
		{
			return new ColumnSetup
			{
				Name = name,
				Attributes = o
			};
		}

		public static ColumnSetup AddPageBreakAfterTable()
		{
			return new ColumnSetup
			{
				PageBreak = true
			};
		}

		public static ColumnSetup ForImage(string name, string imageurl, string url = null, string maxheight = null, string maxwidth = null, object attributes = null)
		{
			return new ColumnSetup
			{
				Name = name,
				ImageUrl = (imageurl ?? "{0}"),
				Url = url,
				ImageMaxHeightString = maxheight,
				ImageMaxWidthString = maxwidth
			};
		}

		public static ColumnSetup Hide(string name)
		{
			return new ColumnSetup
			{
				Name = name,
				Visible = false
			};
		}

		public static IEnumerable<ColumnSetup> List(params ColumnSetup[] setups)
		{
			return setups.ToList();
		}
		
		public string PopulateWithValue(string val, Dictionary<string, object> rowsrcd)
		{
			if (!val.HasText())
			{
				return val;
			}
			foreach (KeyValuePair<string, object> item in rowsrcd)
			{
				val = val.Replace("{" + item.Key + "}", item.Value?.ToString() ?? "");
			}
			return val;
		}
		
		internal void SetAttributeValues(XElement element, bool overwriteExisting, Dictionary<string, object> rowsrc)
		{
			if (Attributes == null)
			{
				return;
			}
			Dictionary<string, string> dictionary = Attributes.AnonymousToDictonary();
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				if (!overwriteExisting && element.Attributes(item.Key).Any())
				{
					continue;
				}
				string text = item.Key;
				if (item.Key.StartsWith("data_"))
				{
					text = text.Replace("data_", "data-");
				}
				string text2 = item.Value ?? "";
				foreach (KeyValuePair<string, object> item2 in rowsrc)
				{
					text2 = text2.Replace("{" + item2.Key + "}", item2.Value?.ToString() ?? "");
				}
				element.SetAttributeValue(text, text2);
			}
		}
	}
	
}
