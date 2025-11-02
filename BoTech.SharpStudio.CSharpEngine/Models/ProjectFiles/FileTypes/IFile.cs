using Material.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes
{
    public interface IFile
    {
        public FileSystemItem FileInfo { get; }
        public MaterialIconKind Icon { get; }
	}
}
