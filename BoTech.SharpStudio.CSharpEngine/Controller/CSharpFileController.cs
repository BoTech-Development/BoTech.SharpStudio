using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Controller
{
    internal class CSharpFileController
    {
        public void AnalyzeFile(string filePath)
        {
            // Implementation for analyzing a C# file
            Console.WriteLine($"Analyzing C# file at: {filePath}");
            if(!File.Exists(filePath) || !filePath.Contains(".cs"))
            {
                return;
			}
			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
           // syntaxTree.GetRoot().
		}
	}
}
