using BoTech.SharpStudio.CSharpEngine.Models;
using BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Controller
{
    internal class ProjectFileController
    {
		
        public List<AnalyzingError> AnalyzeProjectFiles(Project project)
        {
			List<AnalyzingError> errors = new List<AnalyzingError>();
			AnalyzeMultipeleFiles(project.FileSystemItem.Children, errors);
			return errors;
		}
        private void AnalyzeMultipeleFiles(List<FileSystemItem> items, List<AnalyzingError> errors)
        {
			foreach (FileSystemItem item in items)
			{
				if (item.IsFile && item.FileWithActions != null)
				{
					try
					{
						item.FileWithActions.AnalyzeFile();
					}
					catch (Exception ex)
					{
						errors.Add(new AnalyzingError(item, ex));
					}
				}
				else if (item.IsDirectory)
				{
					AnalyzeMultipeleFiles(item.Children, errors);
				}
			}
		}
	}
	public class AnalyzingError
	{
		public FileSystemItem File { get; set; }
		public Exception Exception { get; set; }
		public AnalyzingError(FileSystemItem file, Exception exception)
		{
			File = file;
			Exception = exception;
		}
	}
}
