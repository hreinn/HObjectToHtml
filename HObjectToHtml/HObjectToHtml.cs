using HObjectToHtml.Convert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HObjectToHtml
{


    public static class HObjectToHtml
	{

		public static string GetHtml(this object o,
			bool includeStyleHeader = false,
			string tableid = "table",
			string classes = "hr1tbl",
			//EditableTable.Paging paging = null,
			IEnumerable<ColumnSetup> columnSetup = null,
			string columnOrder = null,
			string filename = null,
			bool open = false
			)
		{
			EditableTable editableTable = new EditableTable();
			if (columnSetup != null)
			{
				editableTable.Setup = columnSetup.ToList();
			}
			editableTable.ColumnOrder = columnOrder;
			editableTable.GenerateTable(o, tableid, classes);//, paging
			var stylehdr = "";
			if (includeStyleHeader) stylehdr = $"<style>{EditableTable.Hr1TblCss}</style>";
			var html = stylehdr + editableTable.Html;
			if (filename != null)
			{
				File.WriteAllText(filename, html);
				if (open)
					System.Diagnostics.Process.Start(filename);
			}
			return stylehdr + editableTable.Html;
		}
		
	}

	#region testclass
	public class Customer
	{
		public string No_ { get; set; }
		public string Name { get; set; }
		public string PhoneNo_ { get; set; }
		public string PreferredBankAccountCode { get; set; }
		public string CustomerPostingGroup { get; set; }
		public string GlobalDimension1Code { get; set; }
		public string GlobalDimension2Code { get; set; }
		public Order[] Orders { get; set; }
	}

	public class Order
	{
		public int DocumentType { get; set; }
		public string ExternalDocumentNo_ { get; set; }
		public DateTime PostingDate { get; set; }
		public string No_ { get; set; }
		public string YourReference { get; set; }
		public Orderline[] OrderLines { get; set; }
	}

	public class Orderline
	{
		public int LineNo_ { get; set; }
		public int Type { get; set; }
		public string No_ { get; set; }
		public float Quantity { get; set; }
		public float UnitPrice { get; set; }
		public string Description { get; set; }
	}
	#endregion
}
