#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mobile.PrintTemplates
{
using System;

#line 1 "ShowResultPrintTemplate.cshtml"
using System.Collections.Generic;

#line default
#line hidden
using System.Linq;
using System.Text;

#line 2 "ShowResultPrintTemplate.cshtml"
using Xamarin.Forms;

#line default
#line hidden


[System.CodeDom.Compiler.GeneratedCodeAttribute("RazorTemplatePreprocessor", "17.4.0.312")]
public partial class ShowResultPrintTemplate : ShowResultPrintTemplateBase
{

#line hidden

#line 3 "ShowResultPrintTemplate.cshtml"
public List<ModelsShared.Models.StudentAnswersQustions> Model { get; set; }

#line default
#line hidden


public override void Execute()
{
WriteLiteral("<html");

WriteLiteral(" lang=\"en\"");

WriteLiteral(">\r\n<head>\r\n\t<!-- iOS - needs to reside in the \"Resources\" directory and have buil" +
"d action of \"BundleResource\" -->\r\n\t<!-- Droid - needs to reside in \"Assets\" dire" +
"ctory and have build action of \"AndroidAsset\" -->\r\n\t<meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(">\r\n\t<meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\"");

WriteLiteral(">\r\n\r\n\t<!-- Bootstrap CSS -->\r\n\t<link");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css\"");

WriteLiteral(" integrity=\"sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6J" +
"Xm\"");

WriteLiteral(" crossorigin=\"anonymous\"");

WriteLiteral(">\r\n\t<link");

WriteLiteral(" rel=\"stylesheet\"");

WriteAttribute ("href", " href=\"", "\""

#line 14 "ShowResultPrintTemplate.cshtml"
, Tuple.Create<string,object,bool> ("", DependencyService.Get<Mobile.Interfaces.IPrintService>().GetStyle()

#line default
#line hidden
, false)
);
WriteLiteral(@">
	<style>
		div {
			-webkit-user-select: none; /* Safari 3.1+ */
			-moz-user-select: none; /* Firefox 2+ */
			-ms-user-select: none; /* IE 10+ */
			user-select: none; /* Standard syntax */
			cursor: default;
		}
		.card {
			box-shadow: 0 0 20px 0 rgba(100, 100, 100, 0.56);
		}
	</style>
</head>
<body>
	<div");

WriteLiteral(" class=\"container-fluid\"");

WriteLiteral(" style=\"padding-top: 10px;padding-bottom: 10px;\"");

WriteLiteral(">\r\n");


#line 30 "ShowResultPrintTemplate.cshtml"
		

#line default
#line hidden

#line 30 "ShowResultPrintTemplate.cshtml"
         foreach (var item in Model)
		{


#line default
#line hidden
WriteLiteral("\t\t\t<div");

WriteLiteral(" class=\"card\"");

WriteLiteral(" style=\"margin-top: 10px;margin-bottom: 10px;\"");

WriteLiteral(">\r\n\t\t\t\t<div");

WriteLiteral(" class=\"card-body\"");

WriteLiteral(">\r\n\t\t\t\t\t<h3");

WriteLiteral(" class=\"card-title\"");

WriteLiteral(">");


#line 34 "ShowResultPrintTemplate.cshtml"
                                      Write(item.User.FullName);


#line default
#line hidden
WriteLiteral("</h3>\r\n\t\t\t\t\t<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n\t\t\t\t\t\t<div");

WriteLiteral(" class=\"col\"");

WriteLiteral(">\r\n\t\t\t\t\t\t\t<h4");

WriteLiteral(" class=\"card-text\"");

WriteLiteral(">");


#line 37 "ShowResultPrintTemplate.cshtml"
                                             Write(item.User.UniversityNumber);


#line default
#line hidden
WriteLiteral("</h4>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t<div");

WriteLiteral(" class=\"col\"");

WriteLiteral(">\r\n\t\t\t\t\t\t\t<h4");

WriteLiteral(" class=\"card-text text-right font-weight-bold\"");

WriteLiteral(" style=\"color:green;\"");

WriteLiteral(">");


#line 40 "ShowResultPrintTemplate.cshtml"
                                                                                              Write(item.Result);


#line default
#line hidden
WriteLiteral("</h4>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</div>\r\n\t\t\t</div>\r\n");


#line 45 "ShowResultPrintTemplate.cshtml"
		}


#line default
#line hidden
WriteLiteral("\t</div>\r\n</body>\r\n</html>");

}
}

// NOTE: this is the default generated helper class. You may choose to extract it to a separate file 
// in order to customize it or share it between multiple templates, and specify the template's base 
// class via the @inherits directive.
public abstract class ShowResultPrintTemplateBase
{

		// This field is OPTIONAL, but used by the default implementation of Generate, Write, WriteAttribute and WriteLiteral
		//
		System.IO.TextWriter __razor_writer;

		// This method is OPTIONAL
		//
		/// <summary>Executes the template and returns the output as a string.</summary>
		/// <returns>The template output.</returns>
		public string GenerateString ()
		{
			using (var sw = new System.IO.StringWriter ()) {
				Generate (sw);
				return sw.ToString ();
			}
		}

		// This method is OPTIONAL, you may choose to implement Write and WriteLiteral without use of __razor_writer
		// and provide another means of invoking Execute.
		//
		/// <summary>Executes the template, writing to the provided text writer.</summary>
		/// <param name="writer">The TextWriter to which to write the template output.</param>
		public void Generate (System.IO.TextWriter writer)
		{
			__razor_writer = writer;
			Execute ();
			__razor_writer = null;
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the template output without HTML escaping it.</summary>
		/// <param name="value">The literal value.</param>
		protected void WriteLiteral (string value)
		{
			__razor_writer.Write (value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the TextWriter without HTML escaping it.</summary>
		/// <param name="writer">The TextWriter to which to write the literal.</param>
		/// <param name="value">The literal value.</param>
		protected static void WriteLiteralTo (System.IO.TextWriter writer, string value)
		{
			writer.Write (value);
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a value to the template output, HTML escaping it if necessary.</summary>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected void Write (object value)
		{
			WriteTo (__razor_writer, value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes an object value to the TextWriter, HTML escaping it if necessary.</summary>
		/// <param name="writer">The TextWriter to which to write the value.</param>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected static void WriteTo (System.IO.TextWriter writer, object value)
		{
			if (value == null)
				return;

			var write = value as Action<System.IO.TextWriter>;
			if (write != null) {
				write (writer);
				return;
			}

			//NOTE: a more sophisticated implementation would write safe and pre-escaped values directly to the
			//instead of double-escaping. See System.Web.IHtmlString in ASP.NET 4.0 for an example of this.
			writer.Write(System.Net.WebUtility.HtmlEncode (value.ToString ()));
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to the template output.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		protected void WriteAttribute (string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			WriteAttributeTo (__razor_writer, name, prefix, suffix, values);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to a TextWriter.
		/// </summary>
		/// <param name="writer">The TextWriter to which to write the attribute.</param>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		///<remarks>Used by Razor helpers to write attributes.</remarks>
		protected static void WriteAttributeTo (System.IO.TextWriter writer, string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			// this is based on System.Web.WebPages.WebPageExecutingBase
			// Copyright (c) Microsoft Open Technologies, Inc.
			// Licensed under the Apache License, Version 2.0
			if (values.Length == 0) {
				// Explicitly empty attribute, so write the prefix and suffix
				writer.Write (prefix);
				writer.Write (suffix);
				return;
			}

			bool first = true;
			bool wroteSomething = false;

			for (int i = 0; i < values.Length; i++) {
				Tuple<string,object,bool> attrVal = values [i];
				string attPrefix = attrVal.Item1;
				object value = attrVal.Item2;
				bool isLiteral = attrVal.Item3;

				if (value == null) {
					// Nothing to write
					continue;
				}

				// The special cases here are that the value we're writing might already be a string, or that the
				// value might be a bool. If the value is the bool 'true' we want to write the attribute name instead
				// of the string 'true'. If the value is the bool 'false' we don't want to write anything.
				//
				// Otherwise the value is another object (perhaps an IHtmlString), and we'll ask it to format itself.
				string stringValue;
				bool? boolValue = value as bool?;
				if (boolValue == true) {
					stringValue = name;
				} else if (boolValue == false) {
					continue;
				} else {
					stringValue = value as string;
				}

				if (first) {
					writer.Write (prefix);
					first = false;
				} else {
					writer.Write (attPrefix);
				}

				if (isLiteral) {
					writer.Write (stringValue ?? value);
				} else {
					WriteTo (writer, stringValue ?? value);
				}
				wroteSomething = true;
			}
			if (wroteSomething) {
				writer.Write (suffix);
			}
		}
		// This method is REQUIRED. The generated Razor subclass will override it with the generated code.
		//
		///<summary>Executes the template, writing output to the Write and WriteLiteral methods.</summary>.
		///<remarks>Not intended to be called directly. Call the Generate method instead.</remarks>
		public abstract void Execute ();

}
}
#pragma warning restore 1591
