using BoTech.SharpStudio.CSharpEngine.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.ViewModels.Editor.Tools
{
	/// <summary>
	/// Implementations of this interface are intended to support tool-based user interfaces that operate
	/// on objects of type T. The interface provides static metadata about the tool, as well as mechanisms for handling
	/// object changes and user-initiated reload actions
	/// </summary>
	/// <typeparam name="T">The type of object managed by the tool view model. Must be a reference type. Primitives are not allowed!</typeparam>
	internal interface IToolViewModel<T> where T : class
	{
		/// <summary>
		/// Represents the name associated with the current tool.
		/// Is not changeable at runtime.
		/// </summary>
		public static readonly string Name;
        /// <summary>
		/// Provides a textual description of the associated tool.
		/// </summary>
		public static readonly string Description;
		/// <summary>
		/// Gets or sets the current object of type <typeparamref name="T"/> managed/displayed/edited by the tool.
		/// </summary>
		public T CurrentObject { get; set; }
		/// <summary>
		/// Initializes the tool-ui and prepares it for use.
		/// </summary>
		public void InitializeTool();
		/// <summary>
		/// Will be called when the CurrentObject property changes.
		/// </summary>
		/// <param name="oldObject">The old instance of the CurrentObject Property</param>
		/// <param name="newObject">New instance</param>
		public void OnCurrentObjectChanged(T oldObject, T newObject);
		/// <summary>
		/// Can be called when deep settings have been changed or the user requests a reload of the project / solution.
		/// </summary>
		/// <param name="solution">The reloaded Solution object</param>
		public void OnSolutionReloaded(Solution solution);
		/// <summary>
		/// Can be called when deep settings have been changed or the user requests a reload of the project / solution.
		/// </summary>
		/// <param name="project">The reloaded Project object</param>
		public void OnProjectReloaded(Project project);
		/// <summary>
		/// Invoked when the user clicks the reload button in the tool UI.
		/// </summary>
		public void OnReload();
	}
}
