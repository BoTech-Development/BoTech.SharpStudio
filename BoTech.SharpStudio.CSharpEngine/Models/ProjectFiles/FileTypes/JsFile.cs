using Material.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes
{
    public class JsFile : AFile
	{
		public JsFile(FileSystemItem file) : base(file)
		{
			Icon = MaterialIconKind.FileCode;
		}
		public override void AnalyzeFileType()
		{
			Icon = MaterialIconKind.FileCode;
		}

		public override void AnalyzeFile()
		{
			throw new NotImplementedException();
		}

		public override void DeleteFile()
		{
			throw new NotImplementedException();
		}

		public override void DeleteFileSavely()
		{
			throw new NotImplementedException();
		}

		public override void MoveFile(string newPath)
		{
			throw new NotImplementedException();
		}

		public override void MoveFileSavely(string newPath)
		{
			throw new NotImplementedException();
		}

		public override void RenameFile(string newName)
		{
			throw new NotImplementedException();
		}

		public override void RenameFileSavely(string newName)
		{
			throw new NotImplementedException();
		}
	}
}
