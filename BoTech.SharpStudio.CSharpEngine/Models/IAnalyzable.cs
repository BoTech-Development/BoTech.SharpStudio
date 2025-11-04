using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models
{
	/// <summary>
	/// Definse operations to analyze a code file or project.
	/// </summary>
	public interface IAnalyzable
    {
		/// <summary>
		/// True if errors were found during the analysis; otherwise, false.
		/// </summary>
		public bool HasErrors { get; }
		/// <summary>
		/// When there code peaces that could cause issues, but are not errors.
		/// </summary>
		public bool HasWarnings { get; }
		/// <summary>
		/// Analyzes the contents of the file or project file.
		/// </summary>
		public void AnalyzeFile();
	}
}
