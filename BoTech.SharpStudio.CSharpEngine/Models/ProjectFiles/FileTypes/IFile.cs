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
		/// <summary>
		/// Info about the file as a reference to a FileSystemItem.
		/// </summary>
		public FileSystemItem FileInfo { get; }
        /// <summary>
        /// The Icon of the file which will be displayed next to the file Name.
        /// </summary>
        public MaterialIconKind Icon { get; }
	}
}
