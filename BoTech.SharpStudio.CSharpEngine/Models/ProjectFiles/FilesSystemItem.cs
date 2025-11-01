using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles
{
    /// <summary>
    /// Represents a file system item, such as a file or directory, and provides access to its metadata and child items.
    /// </summary>
    /// <remarks>Use this class to encapsulate information about files and directories, including their names,
    /// paths, and associated metadata. For directory items, the Children property contains the immediate subdirectories
    /// and files. The DirectoryInfo and FileInfo properties are populated based on whether the item represents a
    /// directory or a file, respectively.</remarks>
    public class FilesSystemItem
    {
		/// <summary>
		/// Gets the directory or file name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// The absolute Path to the directory or file.
		/// </summary>
		public string Path { get; private set; }
        public bool IsDirectory { get; private set; }
		public bool IsFile { get; private set; }
		public DirectoryInfo? DirectoryInfo { get; private set; }
        public FileInfo? FileInfo { get; private set; }
		public List<FilesSystemItem> Children { get; private set; } = new List<FilesSystemItem>();
        public FilesSystemItem(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            Path = fileInfo.FullName;
            IsDirectory = false;
            IsFile = true;
			FileInfo = fileInfo;
        }
        public FilesSystemItem(DirectoryInfo directoryInfo)
        {
            Name = directoryInfo.Name;
            Path = directoryInfo.FullName;
            IsDirectory = true;
            IsFile = false;
			DirectoryInfo = directoryInfo;
			CreateSubDirectoryItems(directoryInfo.GetDirectories().ToList());
			CreateFileItems(directoryInfo.GetFiles().ToList());
		}
        private void CreateSubDirectoryItems(List<DirectoryInfo> directories)
        {
            foreach (var directory in directories)
				Children.Add(new FilesSystemItem(directory));
        }
        private void CreateFileItems(List<FileInfo> files)
        {
            foreach (var file in files)
                Children.Add(new FilesSystemItem(file));
		}
	}
}
