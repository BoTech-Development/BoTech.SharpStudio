using Material.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes
{
    public class RazorCsFile : AFile
	{
		public RazorCsFile(FileSystemItem file) : base(file)
		{
			Icon = MaterialIconKind.FileCode;
		}
		public override void AnalyzeFileType()
		{
			Icon = MaterialIconKind.FileCode;
		}

		public override void AnalyzeFile()
		{
			
		}

		public override void DeleteFile()
		{
			
		}

		public override void DeleteFileSavely()
		{
			
		}

		public override void MoveFile(string newPath)
		{
			
		}

		public override void MoveFileSavely(string newPath)
		{
			
		}

		public override void RenameFile(string newName)
		{
			
		}

		public override void RenameFileSavely(string newName)
		{
			
		}
	}
}
