using BoTech.SharpStudio.CSharpEngine.Models.CSharp;
using Material.Icons;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoTech.SharpStudio.CSharpEngine.Models.ProjectFiles.FileTypes
{
    public class CSharpFile : AFile
	{
        public CSharpFileType Type { get; set; } 
        public string Name { get; set; }
		public AccessModifier AccessModifier { get; set; }
		public CSharpFile(FileSystemItem file) : base(file)
		{
			Icon = MaterialIconKind.FileCode;
		}
		public override void AnalyzeFileType()
		{
			Icon = MaterialIconKind.FileCode;
		}

		public override void AnalyzeFile()
		{
			if (this.FileInfo.FileInfo != null)
			{
				SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(this.FileInfo.FileInfo.FullName));
				AnalyzeSyntaxTree(syntaxTree);
			}
		}
		private void AnalyzeSyntaxTree(SyntaxTree syntaxTree)
		{
			CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();
			AnalyzeTypeModifiers(root);

		}

		private void AnalyzeTypeModifiers(CompilationUnitSyntax root)
		{
			List<BaseTypeDeclarationSyntax> typeDeclarations = root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>().ToList();
			if (typeDeclarations.Count == 1)
			{
				var typeDeclaration = typeDeclarations.First();
				if (typeDeclaration is ClassDeclarationSyntax)
				{
					Type = CSharpFileType.Class;
				}
				else if (typeDeclaration is InterfaceDeclarationSyntax)
				{
					Type = CSharpFileType.Interface;
				}
				else if (typeDeclaration is EnumDeclarationSyntax)
				{
					Type = CSharpFileType.Enum;
				}
				else if (typeDeclaration is StructDeclarationSyntax)
				{
					Type = CSharpFileType.Struct;
				}
                if (typeDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword))
                {
                    AccessModifier = AccessModifier.Public;
                }
                else if (typeDeclaration.Modifiers.Any(SyntaxKind.InternalKeyword))
                {
                    AccessModifier = AccessModifier.Internal;
                }
                else if (typeDeclaration.Modifiers.Any(SyntaxKind.ProtectedKeyword))
                {
                    AccessModifier = AccessModifier.Protected;
                }
                else if (typeDeclaration.Modifiers.Any(SyntaxKind.PrivateKeyword))
                {
                    AccessModifier = AccessModifier.Private;
                }
                else if(typeDeclaration.Modifiers.Any(SyntaxKind.ProtectedKeyword) && typeDeclaration.Modifiers.Any(SyntaxKind.InternalKeyword))
				{
					AccessModifier = AccessModifier.ProtectedInternal;
				}
           
            }
			else if (typeDeclarations.Count > 1)
			{
				Type = CSharpFileType.MultipleTypes;
			}



		}

		public override void DeleteFile()
		{
			
		}

		public override void DeleteFileSavely()
		{
			
		}

		public override void MoveFile(string newPath)
		{
			
		}

		public override void MoveFileSavely(string newPath)
		{
			
		}

		public override void RenameFile(string newName)
		{
			
		}

		public override void RenameFileSavely(string newName)
		{
			
		}
	}
   

	
}
