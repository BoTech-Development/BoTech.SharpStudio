using BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes;
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
    public class FileSystemItem
    {
		/// <summary>
		/// Gets the directory or file name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// The absolute Path to the directory or file.
		/// </summary>
		public string Path { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the current item represents a directory.
        /// </summary>
        public bool IsDirectory { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the current item represents a file.
        /// </summary>
		public bool IsFile { get; private set; }
		/// <summary>
		/// Will be set if the item is a directory <see cref="IsDirectory"/>
		/// </summary>
		public DirectoryInfo? DirectoryInfo { get; private set; }
		/// <summary>
		/// Will be set if the item is a directory <see cref="IsFile"/>
		/// </summary>
		public FileInfo? FileInfo { get; private set; }
		/// <summary>
		/// Item will have no items when it is a file, otherwise it will contain all sub directories and files.
		/// </summary>
		public List<FileSystemItem> Children { get; private set; } = new List<FileSystemItem>();
		/// <summary>
		/// You can use this property to perform action on the file, depending on its type.
		/// You also be able to access the analyzed file information (could be code analysis, image metadata, etc.) through this property.
		/// Is null when the item is a directory.
		/// </summary>
		public IFileWithActions? FileWithActions { get; private set; }
		public FileSystemItem(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            Path = fileInfo.FullName;
            IsDirectory = false;
            IsFile = true;
			FileInfo = fileInfo;
            FileWithActions = FileTypeFactory.GetFileWithActions(this);
		}
        public FileSystemItem(DirectoryInfo directoryInfo)
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
				Children.Add(new FileSystemItem(directory));
        }
        private void CreateFileItems(List<FileInfo> files)
        {
            foreach (var file in files)
                Children.Add(new FileSystemItem(file));
		}
	}
}
