
# HObjectToHtml - c# object to HTML 

Converts a c# object to html with some optional formatting.

This is ancient code that I use quite a lot.  Handy for debugging and sending simple lists with links via email.
Inspired by LINQpads .Dump() function.

(There is functionality in the source that is not in use - this project needs some cleanup - is buggy if you have Column names that are duplicate in subtables - if using columnsetup for those columns.)

Simple example:

```c#

var customerData = GetCustomerData();
//true includes styling in the header.
customerData.GetHtml(true);

// returns https://i.imgur.com/srpKGsX.png
```
![No setup](https://i.imgur.com/srpKGsX.png)

With column setup we can hide and create links.

```c#
customerData.GetHtml(includeStyleHeader:true,	
	columnSetup:ColumnSetup.List(
		ColumnSetup.Hide("PhoneNo_"),
        ColumnSetup.Hide("PreferredBankAccountCode"),
        ColumnSetup.Hide("GlobalDimension2Code"),
		ColumnSetup.Hide("ExternalDocumentNo_"),
		ColumnSetup.ForUrlNewWindow("GlobalDimension1Code","https://www.google.com/search?q={0} {Name}")		
	));
//returns https://i.imgur.com/JPHZmLG.png

```
![With column setup](https://i.imgur.com/fnuJya4.png)

Please note that raw JSON of row data is included in the html.

```c#
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
```		
Gives you:
![With column setup](https://i.imgur.com/WxHMohw.png)
