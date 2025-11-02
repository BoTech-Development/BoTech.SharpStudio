using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles
{
    public enum FileSystemItemTypes
    {
		/// <summary>
		/// .cs File
		/// </summary>
		CSharpFile,
		/// <summary>
		/// .xml File
		/// </summary>
		XmlFile,
		/// <summary>
		/// .json File
		/// </summary>
		JsonFile,
		/// <summary>
		/// .config File
		/// </summary>
		ConfigFile,
		/// <summary>
		/// .axaml File
		/// </summary>
		AxamlFile,
		/// <summary>
		/// .axaml.cs File
		/// </summary>
		AxamlCsFile,
		/// <summary>
		/// .xaml File
		/// </summary>
		XamlFile,
		/// <summary>
		/// .xaml.cs File
		/// </summary>
		XamlCsFile,
		/// <summary>
		/// .razor File
		/// </summary>
		RazorFile,
		/// <summary>
		/// .razor.cs File
		/// </summary>
		RazorCsFile,
		/// <summary>
		/// .razor.css File
		/// </summary>
		RazorCssFile,
		/// <summary>
		/// .razor.js File
		/// </summary>
		RazorJsFile,
		/// <summary>
		/// .cshtml File
		/// </summary>
		CsHtmlFile,
		/// <summary>
		/// .html File
		/// </summary>
		HtmlFile,
		/// <summary>
		/// .css File
		/// </summary>
		CssFile,
		/// <summary>
		/// .js File
		/// </summary>
		JsFile,
		/// <summary>
		/// .resx File
		/// </summary>
		ResourceFile,
		/// <summary>
		/// Not idenitified file type
		/// </summary>
		Other,
        /// <summary>
        /// Default value
        /// </summary>
		None
	}
}
