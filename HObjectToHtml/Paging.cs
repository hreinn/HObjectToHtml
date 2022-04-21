using System.Collections.Generic;
using System.Linq;

namespace HObjectToHtml
{
	//todo 
 //   public partial class EditableTable
	//{
 //       public class Paging
	//	{
	//		public int TotalPages = 0;

	//		public string UrlRoot = "";

	//		public int Page = 0;

	//		public int PageSize = 100;

	//		public int Skip = 0;

	//		public string HtmlId = "hr1page";

	//		public string Classes = "hr1pagecls";

	//		public string PagesMarkup;

	//		public string PageTemplate = "\r\n<div id='${HtmlId}'>\r\n${PagesMarkup}\r\n</div>\r\n";

	//		public Paging()
	//		{
	//		}

	//		public Paging(string url, int totalrecords, int? pagesize = null)
	//		{
	//			UrlRoot = url;
	//			string[] array = (from v in url.TextAfterFirst("?").Split("&")
	//							  where v.StartsWith("hpage=")
	//							  select v).FirstOrDefault().TextAfterFirst("hpage=")?.Split("=");
	//			if (array == null)
	//			{
	//				PageSize = pagesize ?? PageSize;
	//				TotalPages = totalrecords / PageSize;
	//				return;
	//			}
	//			PageSize = array.Where((string v) => v.StartsWith("t")).FirstOrDefault().HOnlyDigits()
	//				.HParseForceNullOnError() ?? PageSize;
	//			if (pagesize > 0)
	//			{
	//				PageSize = pagesize.Value;
	//				Skip = array.Where((string v) => v.StartsWith("s")).FirstOrDefault().HOnlyDigits()
	//					.HParseForceNullOnError() ?? Skip;
	//				TotalPages = totalrecords / PageSize;
	//			}
	//			else
	//			{
	//				Skip = array.Where((string v) => v.StartsWith("s")).FirstOrDefault().HOnlyDigits()
	//					.HParseForceNullOnError() ?? Skip;
	//			}
	//			TotalPages = totalrecords / PageSize;
	//			if (Skip > 0)
	//			{
	//				Page = Skip / PageSize;
	//			}
	//		}
			
	//		public string Get()
	//		{
	//			string text = "hpage=s{0}=t{1}";
	//			PagesMarkup = "";
	//			string text2 = UrlRoot;
	//			if (!text2.Contains("?"))
	//			{
	//				text2 += "?";
	//			}
	//			if (text2.Contains("hpage="))
	//			{
	//				string s = text2.TextAfterFirst("?");
	//				string text3 = (from v in s.Split("&")
	//								where !v.StartsWith("hpage")
	//								select v).FlattenText("&").TrimStart('&');
	//				if (text3.HasText())
	//				{
	//					text3 += "&";
	//				}
	//				text2 = text2.HTextUntil("?") + "?" + text3 + text;
	//			}
	//			else
	//			{
	//				text2 = text2 + "&" + text;
	//			}
	//			for (int i = 0; i < TotalPages + 1; i++)
	//			{
	//				PagesMarkup += $"<a href='{string.Format(text2, i * PageSize, PageSize)}'>{i}</a> | ";
	//			}
	//			Dictionary<string, object> source = (Dictionary<string, object>)this.ToJson().FromJson<object>();
	//			Dictionary<string, string> dictionary = source.ToDictionary((KeyValuePair<string, object> v) => v.Key, (KeyValuePair<string, object> p) => p.Value?.ToString());
	//			return Template.ParseTemplateFromStringAnon(PageTemplate, this);
	//		}
	//	}

	//}
}
