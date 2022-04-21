using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HObjectToHtml.Convert
{
    public partial class EditableTable
	{
		public class ValueReply
		{
			public bool Xml;

			public string Contents;
		}

		public static string Hr1TblCss = @"table.hr1tbl { 
  width: 100%; 
  border-collapse: collapse; 
}
/* Zebra striping */
table.hr1tbl > tbody > tr:nth-of-type(odd)  { 
  background: #ddd; 

}
.hr1tbl > thead > tr  { 
  background: #333;   
  color: white; 
  font-weight: 500; 
  font-size:1.2em;
  vertical-align: top;
  margin:23px;
}
.hr1tbl > tbody > tr > td { 
  padding: 6px;   
  border: 1px solid #ccc; 
  text-align: left; 
  vertical-align: top;
}    

.hr1tbl > thead > tr > td { 
  padding: 6px; 
  padding-left:5px;
    
  border: 1px solid #ccc; 
  text-align: left; 
  vertical-align: top;
}    
";

		public string Javascript;

		public string Html;

		public string Style;

		private string primaryKey = "id";

		public int Tablecounter
		{
			get;
			private set;
		}

		public List<ColumnSetup> Setup
		{
			get;
			set;
		} = new List<ColumnSetup>();


		public bool AddEditableScript
		{
			get;
			set;
		}

		public string ColumnOrder
		{
			get;
			internal set;
		}

		public ValueReply GetVal(object value, string name, int recur)
		{
			if (value == null)
			{
				return new ValueReply
				{
					Contents = null
				};
			}
			if (value?.GetType().IsPrimitive ?? false)
			{
				return new ValueReply
				{
					Contents = value?.ToString()
				};
			}
			if (value?.GetType().FullName == "System.String")
			{
				string val = value.ToString();
				if (val.ContainsInCI("<a ", "</a"))
				{
					return new ValueReply
					{
						Contents = "<div>" + value.ToString() + "</div>",
						Xml = true
					};
				}
			}
			if (value?.GetType().FullName.IsAny("System.String", "System.Decimal", "System.DateTime") ?? false)
			{
				return new ValueReply
				{
					Contents = value?.ToString()
				};
			}
			return new ValueReply
			{
				Contents = Html2internal(value, "tbl" + name, "hr1tbl", recur + 1),
				Xml = true
			};
		}

		public Dictionary<string, object> crdic(string v, object o)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary[v] = o;
			return dictionary;
		}

		public void GenerateTable(object objTo, string tableid = null, string classes = "hr1tbl") //, Paging paging = null
		{
			tableid = "table";
			Html = Html2internal(objTo.ToJson().FromJson<object>(), tableid, classes);
			Style = Hr1TblCss;
			Javascript = "";

			//if (paging != null)
			//{
			//	Html = "<div id='boxfor_" + tableid + "'>" + paging.Get() + Html + "</div>";
			//}
		}

		private string Html2internal(object objTo, string tableid = null, string classes = "hr1tbl", int recur = 0)
		{
			if (recur == 0 && recur > 3)
			{
				return null;
			}
			IEnumerable<object> tablesrc;
			if (objTo.GetType().IsArray)
			{
				tablesrc = (IEnumerable<object>)objTo;
			}
			else
			{
				tablesrc = new[] { objTo };
			}
			string str = "";
			object obj = tablesrc.FirstOrDefault();
			if (obj == null)
			{
				return null;
			}
			if (!(obj is Dictionary<string, object>) && !obj.GetType().IsArray)
			{
				tablesrc = tablesrc.Select((object t) => crdic("Item", t)).AsEnumerable();
			}
			Dictionary<string, object> dictionary = (Dictionary<string, object>)((tablesrc.FirstOrDefault() is Dictionary<string, object>) ? tablesrc.FirstOrDefault() : "Item".AsSingleElementArray().ToDictionary((string x) => x, (string y) => tablesrc));
			List<XElement> list = new List<XElement>();
			foreach (object item2 in tablesrc)
			{
				List<XElement> list2 = new List<XElement>();
				if (item2.GetType().IsArray)
				{
					str += Html2internal(item2, null, "hr1tbl", recur + 1);
					continue;
				}
				List<XElement> list3 = new List<XElement>();
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item2;
				Dictionary<string, object> rowsrcd = dictionary2.Where((KeyValuePair<string, object> x) => !(from t in Setup
																											 where !t.Manual
																											 select t into y
																											 select y.Name).Contains(x.Key)).ToDictionary((KeyValuePair<string, object> r) => r.Key, (KeyValuePair<string, object> f) => f.Value);
				foreach (ColumnSetup item3 in Setup.Where((ColumnSetup r) => r.Manual))
				{
					dictionary2[item3.Name] = (item3.ManualValue.HasText() ? item3.PopulateWithValue(item3.ManualValue, rowsrcd) : item3.Name);
				}
				object obj2 = (from x in dictionary2
							   where x.Key.ToLower() == primaryKey
							   select x into v
							   select v.Value).FirstOrDefault() ?? dictionary2.Select((KeyValuePair<string, object> v) => v.Value).FirstOrDefault() ?? "NOPKDEFINED";
				foreach (KeyValuePair<string, object> dt in dictionary2)
				{
					string text = "ecc-td-" + obj2?.ToString().Justaz09() + "-" + dt.Key.Justaz09();
					XElement xElement = new XElement("td");
					xElement.SetAttributeValue("id", text);
					ValueReply val = GetVal(dt.Value, dt.Key, recur);
					xElement.SetAttributeValue("data-pk", obj2);
					xElement.SetAttributeValue("field", dt.Key.Justaz09());
					if (val.Xml && val.Contents != null)
					{
						xElement.Add(XElement.Parse(val.Contents));
						list3.Add(xElement);
						continue;
					}
					ColumnSetup columnSetup = Setup.FirstOrDefault((ColumnSetup x) => !x.PageBreak && x.Name.ToLower() == dt.Key.ToLower());
					if (columnSetup != null && !columnSetup.Visible)
					{
						continue;
					}
					xElement.SetAttributeValue("data-type", "text");
					xElement.SetAttributeValue("data-params", "{Field:" + dt.Key + "}");
					xElement.SetAttributeValue("data-url", "/save/" + tableid + "/" + dt.Key.Justaz09() + "/" + obj2);
					xElement.Value = val.Contents ?? "";
					if (columnSetup == null)
					{
						list3.Add(xElement);
						list3.Add(GetEditableScriptElement(dt, text));
						continue;
					}
					DataType type = columnSetup.Type;
					DataType dataType = type;
					if (dataType == DataType.Button)
					{
						RenderButton(tableid, list3, dictionary2, obj2, dt, text, val, columnSetup);
					}
					else if (columnSetup.Url == null && columnSetup.ImageUrl == null)
					{
						RenderColumn(tableid, list3, dictionary2, obj2, dt, text, val, columnSetup);
					}
					else
					{
						RenderUrlColumn(tableid, list3, dictionary2, obj2, dt, text, val, columnSetup);
					}
				}
				XElement item = new XElement("tr", list3, new XAttribute("class", "pk-row-" + obj2), new XAttribute("data-rowdata", item2.ToJson()));
				list.Add(item);
			}
			IEnumerable<string> hideCols = from v in Setup
										   where !v.Visible
										   select v.Name;
			XElement[] array = (from x in dictionary.Keys
								where !hideCols.Contains(x.ToString())
								select new XElement("td", x)).ToArray();
			XName name = "table";
			object[] obj3 = new object[4]
			{
				new XAttribute("id", tableid ?? "table"),
				new XAttribute("class", classes ?? ""),
				null,
				null
			};
			XName name2 = "thead";
			XName name3 = "tr";
			object[] content = array;
			obj3[2] = new XElement(name2, new XElement(name3, content));
			XName name4 = "tbody";
			content = list.ToArray();
			obj3[3] = new XElement(name4, content);
			XElement xElement2 = new XElement(name, obj3);
			if (Setup.Any((ColumnSetup x) => x.PageBreak))
			{
				xElement2.Add(new XElement("br", new XAttribute("style", "page-break-after: always;")));
			}
			return xElement2.ToString();
		}

		private void RenderButton(string tableid, List<XElement> xmls, Dictionary<string, object> rowsrcd, object pkval, KeyValuePair<string, object> dt, string id, ValueReply tddata, ColumnSetup colsetup)
		{
			colsetup.Url = colsetup.Url ?? colsetup.ImageUrl;
			string format = colsetup.PopulateWithValue(colsetup.ManualValue, rowsrcd);
			XElement xElement = new XElement("button", new XAttribute("value", string.Format(format, tddata.Contents ?? "")), string.Format(format, tddata.Contents ?? ""));
			XElement xElement2 = new XElement("td", new XAttribute("data-pk", pkval), new XAttribute("field", dt.Key.Justaz09()), new XAttribute("data-type", (colsetup != null) ? colsetup.Type.ToString("G").ToLower() : null), new XAttribute("id", id), new XAttribute("data-params", "{Field:" + dt.Key + "}"), new XAttribute("data-url", "/save/" + tableid + "/" + dt.Key.Justaz09() + "/" + pkval), xElement);
			if (colsetup != null && colsetup.Classstring?.Trim().HasText() == true)
			{
				xElement2.SetAttributeValue("class", colsetup.Classstring);
			}
			colsetup.SetAttributeValues(xElement2, overwriteExisting: false, rowsrcd);
			xmls.Add(xElement2);
		}

		private static void RenderUrlColumn(string tableid, List<XElement> xmls, Dictionary<string, object> rowsrcd, object pkval, KeyValuePair<string, object> dt, string id, ValueReply tddata, ColumnSetup colsetup)
		{
			colsetup.Url = colsetup.Url ?? colsetup.ImageUrl;
			string text = colsetup.PopulateWithValue(colsetup.Url, rowsrcd);
			string format = colsetup.PopulateWithValue(colsetup.ImageUrl, rowsrcd);
			if (!colsetup.ImageUrl.HasText())
			{
				XAttribute xAttribute = new XAttribute("href", string.Format(text, tddata.Contents, pkval));
				if (!text.HasText())
				{
					xAttribute = new XAttribute("__noattr", "");
				}
				XElement xElement = new XElement("td", new XAttribute("data-pk", pkval), new XAttribute("field", dt.Key.Justaz09()), new XAttribute("data-type", (colsetup != null) ? colsetup.Type.ToString("G").ToLower() : null), new XAttribute("id", id), new XAttribute("data-params", "{Field:" + dt.Key + "}"), new XAttribute("data-url", "/save/" + tableid + "/" + dt.Key.Justaz09() + "/" + pkval), new XElement("a", xAttribute, new XAttribute("target", colsetup.Name), tddata.Contents));
				if (colsetup != null && colsetup.Classstring?.Trim().HasText() == true)
				{
					xElement.SetAttributeValue("class", colsetup.Classstring);
				}
				colsetup.SetAttributeValues(xElement, overwriteExisting: false, rowsrcd);
				xmls.Add(xElement);
				return;
			}
			string text2 = colsetup.ImageMaxWidthString ?? "200px";
			string text3 = colsetup.ImageMaxHeightString ?? "100px";
			XElement xElement2 = new XElement("img", new XAttribute("src", string.Format(text, tddata.Contents ?? "")), new XAttribute("style", "max-width: " + text2 + "; max-height: " + text3 + ";"), "");
			XElement xElement3 = new XElement("a", new XAttribute("href", string.Format(format, tddata.Contents, pkval)), new XAttribute("target", colsetup.Name), xElement2);
			XElement xElement4 = new XElement("td", new XAttribute("data-pk", pkval), new XAttribute("field", dt.Key.Justaz09()), new XAttribute("data-type", (colsetup != null) ? colsetup.Type.ToString("G").ToLower() : null), new XAttribute("id", id), new XAttribute("data-params", "{Field:" + dt.Key + "}"), new XAttribute("data-url", "/save/" + tableid + "/" + dt.Key.Justaz09() + "/" + pkval), xElement3);
			if (colsetup != null && colsetup.Classstring?.Trim().HasText() == true)
			{
				xElement4.SetAttributeValue("class", colsetup.Classstring);
			}
			colsetup.SetAttributeValues(xElement4, overwriteExisting: false, rowsrcd);
			xmls.Add(xElement4);
		}

		private void RenderColumn(string tableid, List<XElement> xmls, Dictionary<string, object> rowsrcd, object pkval, KeyValuePair<string, object> dt, string id, ValueReply tddata, ColumnSetup colsetup)
		{
			if (colsetup.Pre)
			{
				XElement xElement = new XElement("td", new XAttribute("data-pk", pkval), new XAttribute("field", dt.Key.Justaz09()), new XAttribute("data-type", (colsetup != null) ? colsetup.Type.ToString("G").ToLower() : null), new XAttribute("id", id), new XAttribute("data-params", "{Field:" + dt.Key + "}"), new XAttribute("data-url", "/save/" + tableid + "/" + dt.Key.Justaz09() + "/" + pkval), new XElement("pre", tddata.Contents));
				if (colsetup != null && colsetup.Classstring?.Trim().HasText() == true)
				{
					xElement.SetAttributeValue("class", colsetup.Classstring);
				}
				colsetup.SetAttributeValues(xElement, overwriteExisting: false, rowsrcd);
				xmls.Add(xElement);
				return;
			}
			XElement xElement2 = new XElement("td", new XAttribute("data-pk", pkval), new XAttribute("field", dt.Key.Justaz09()), new XAttribute("data-type", (colsetup != null) ? colsetup.Type.ToString("G").ToLower() : null), new XAttribute("id", id), new XAttribute("data-params", "{Field:" + dt.Key + "}"), new XAttribute("data-url", "/save/" + tableid + "/" + dt.Key.Justaz09() + "/" + pkval), tddata.Contents ?? "");
			if (colsetup != null && colsetup.Classstring?.Trim().HasText() == true)
			{
				xElement2.SetAttributeValue("class", colsetup.Classstring);
			}
			colsetup.SetAttributeValues(xElement2, overwriteExisting: false, rowsrcd);
			xmls.Add(xElement2);
			xmls.Add(GetEditableScriptElement(dt, id));
		}

		private XElement GetEditableScriptElement(KeyValuePair<string, object> dt, string id)
		{
			if (!AddEditableScript)
			{
				return null;
			}
			if (Setup.Any((ColumnSetup x) => x.Name.ToLower() == dt.Key.ToLower()))
			{
				ColumnSetup columnSetup = Setup.First((ColumnSetup x) => x.Name.ToLower() == dt.Key.ToLower());
				string content = "$('#" + id + "').editable(\r\n                {\r\n\r\n                   // type: '" + columnSetup.Type.ToString("G").ToLower() + "',                    \r\n                }\r\n                );";
				return new XElement("script", content);
			}
			return new XElement("script", "$('#" + id + "').editable();");
		}

	}
	
}
