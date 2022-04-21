using HObjectToHtml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
			var obj = new
			{
				Numbers = new[] { 1, 3, 4 },
				Data = new[]
				{
					new {Name="Test1",Email="me@gmail.com"},
					new {Name="Test2",Email="me1@gmail.com"},
					new {Name="Test3",Email="me2@gmail.com"},
				},
				MoreData = new object[]
				{
					new {Name="Test1",Email="me@gmail.com",Url="http://google.com"},
					new {Name="Test2",Email="me1@gmail.com",Pets = new object[]{
						"Cat","Dog"
					} },
					new {Name="Test3",Email="me2@gmail.com",Picture="https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png"},
				}
			};
			var obj2 = new
			{
				Numbers = new[] { 1, 3, 4 },
				Data2 = new[]
						{
					new {Name2="Test1",Email="me@gmail.com"},
					new {Name2="Test2",Email="me1@gmail.com"},
					new {Name2="Test3",Email="me2@gmail.com"},
				},
				MoreData = new object[]
						{
					new {Name="Test1",Email="me@gmail.com",Url="http://google.com"},
					new {Name="Test2",Email="me1@gmail.com",Pets = new object[]{
						new {Type="Cat",PetName="Börk"},
						new {Type="Dog",PetName="Kátur"}
					} },
					new {Name="Test3",Email="me2@gmail.com",Picture="https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png"},
						}
			};


			var html = obj.GetHtml(true);
			File.WriteAllText(@"c:\tmp\test-nosetup.html",html);

			new object[] { obj, obj2 }.GetHtml(true,
			columnSetup: new[]{
		ColumnSetup.ForImage("Picture","{0}",maxheight:"300px",maxwidth:"300px"),
		ColumnSetup.ForUrlNewWindow("Url","{0}?q={Name}"),
		ColumnSetup.ForUrlNewWindow("Name2","https://www.google.com/search?q={0} {Email}"),
		ColumnSetup.ForUrlNewWindow("Name","mailto:{Email}"),
		ColumnSetup.ForUrlNewWindow("Email","mailto:{0}"),
			}, filename: @"c:\tmp\test-withsetup.html");
		}
    }
}
