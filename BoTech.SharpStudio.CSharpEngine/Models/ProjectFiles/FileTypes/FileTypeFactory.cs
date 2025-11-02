using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes
{
    public class FileTypeFactory
    {
		/// <summary>
		/// Creates an instance of a file type based on the file extension.
		/// </summary>
		/// <param name="fileSystemItem">The FileSystemItem with the rquired info.</param>
		/// <returns>A new instance of a <see cref="IFileWithActions"/> depending on the given file extension/info.
		/// If the file extension is not recognized, an instance of <see cref="UnknownFileType"/> is returned.
		/// </returns>
		public static IFileWithActions GetFileWithActions(FileSystemItem fileSystemItem)
        {
            string extension = System.IO.Path.GetExtension(fileSystemItem.Name).ToLower();
            return extension switch
            {
                ".cs" => new CSharpFile(fileSystemItem),
                ".xml" => new XmlFile(fileSystemItem),
                ".json" => new JsonFile(fileSystemItem),
                ".config" => new ConfigFile(fileSystemItem),
                ".axaml" => new AxamlFile(fileSystemItem),
                ".axaml.cs" => new AxamlCsFile(fileSystemItem),
                ".xaml" => new XamlFile(fileSystemItem),
                ".xaml.cs" => new XamlCsFile(fileSystemItem),
                ".razor" => new RazorFile(fileSystemItem),
                ".razor.cs" => new RazorCsFile(fileSystemItem),
                ".razor.css" => new RazorCssFile(fileSystemItem),
                ".razor.js" => new RazorJsFile(fileSystemItem),
                ".cshtml" => new CsHtmlFile(fileSystemItem),
				".html" => new HtmlFile(fileSystemItem),
				".css" => new CssFile(fileSystemItem),
				".js" => new JsFile(fileSystemItem),
				".resx" => new ResourceFile(fileSystemItem),
				_ => new UnknownFileType(fileSystemItem),
            };
		}
	}
}
