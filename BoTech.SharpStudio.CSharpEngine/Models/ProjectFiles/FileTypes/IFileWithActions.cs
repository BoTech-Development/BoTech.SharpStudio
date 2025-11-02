using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes
{
    public interface IFileWithActions	: IFile
	{
		/// <summary>
		/// Moves the file to the specified path, updating its location within the file system.
		/// </summary>
		/// <param name="newPath">The destination path where the file will be moved. Must be a valid, writable file path. Cannot be null or empty.</param>
        public void MoveFile(string newPath);
		/// <summary>
		/// Moves the file to the specified path, ensuring that for example namespaces are updated accordingly.
		/// </summary>
		/// <param name="newPath">The destination file path to which the file will be moved. Must be a valid, non-empty path.</param>
		public void MoveFileSavely(string newPath);
		/// <summary>
		/// Deletes the associated file from the file system.
		/// </summary>
		/// <remarks>If the file does not exist, no action is taken. Ensure that the file is not in use by another process before calling this method to avoid
		/// potential access issues.</remarks>
		public void DeleteFile();
		/// <summary>
		/// Deletes the target file, ensuring that all refernces and dependencies are delete, to ensure that the project works after it.
		/// </summary>
		/// <remarks>If the file does not exist, no action is taken. Ensure that the file is not in use by another process before calling this method to avoid
		/// potential access issues.</remarks>
		public void DeleteFileSavely();
		/// <summary>
		/// Renames the file to the specified new name.
		/// </summary>
		/// <param name="newName">The new name to assign to the file. Must not be null, empty, or contain invalid file name characters.</param>
		public void RenameFile(string newName);
		/// <summary>
		/// Renames the file to the specified new name, ensuring that the code references are updated accordingly.
		/// </summary>
		/// <param name="newName">The new name for the file. Must be a valid file name and cannot be null or empty.</param>
		public void RenameFileSavely(string newName);
		public void AnalyzeFile();
	}
}
