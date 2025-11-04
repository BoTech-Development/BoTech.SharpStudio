using Material.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes
{
    public abstract class AFile : IFileWithActions
    {
		/// <inheritdoc/>
		public FileSystemItem FileInfo { get; }
		/// <inheritdoc/>
		public MaterialIconKind Icon { get; protected set; }
		/// <inheritdoc/>
		public bool HasErrors { get; }
		/// <inheritdoc/>
		public bool HasWarnings { get; }

		protected AFile(FileSystemItem file)
        {
            FileInfo = file;
			AnalyzeFileType();
		}
        public virtual void AnalyzeFileType()
        {
            Icon = MaterialIconKind.File;
		}
        /// <inheritdoc/>
        public abstract void AnalyzeFile();
		/// <inheritdoc/>
		public abstract void DeleteFile();
		/// <inheritdoc/>
		public abstract void DeleteFileSavely();
		/// <inheritdoc/>
		public abstract void MoveFile(string newPath);
		/// <inheritdoc/>
		public abstract void MoveFileSavely(string newPath);
		/// <inheritdoc/>
		public abstract void RenameFile(string newName);
		/// <inheritdoc/>
		public abstract void RenameFileSavely(string newName);

    }
}
