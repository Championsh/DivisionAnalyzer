using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Simplification;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Division
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DivisionCodeFixProvider)), Shared]
    public class DivisionCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DivisionAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {

        }

        private static async void DivisionAsync(Document document,
            LocalDeclarationStatementSyntax localDeclaration,
                CancellationToken cancellationToken)
        {
            return;
        }
    }
}
