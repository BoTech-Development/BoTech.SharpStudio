using Material.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes
{
    internal class UnknownFileType : AFile
    {
      
        public UnknownFileType(FileSystemItem item) : base(item)
		{
            
        }
        public override void AnalyzeFileType()
        {
            Icon = MaterialIconKind.FileQuestionOutline;
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
