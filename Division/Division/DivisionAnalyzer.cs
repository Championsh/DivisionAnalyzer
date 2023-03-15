using Division;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Division
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DivisionAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Division";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.DivideExpression);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var Division = (BinaryExpressionSyntax)context.Node;

            if (!Division.IsKind(SyntaxKind.DivideExpression))
                return;

            var Denominator = Division.Right;

            string s = GetConstValue(Denominator, context);
            if (s != "0" && s != "null")
                return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), Denominator.ToString()));
        }

        private static string GetConstValue(ExpressionSyntax denominator, SyntaxNodeAnalysisContext context)
        {
            if (denominator.IsKind(SyntaxKind.NumericLiteralExpression)
                || denominator.IsKind(SyntaxKind.StringLiteralExpression))
            {
                var literal = denominator as LiteralExpressionSyntax;
                var res = literal.Token.ValueText;

                //Console.WriteLine("I GOT THE VALUE, its: {0}", res);
                return res;
            }
            else if (denominator.IsKind(SyntaxKind.AddExpression))
            {
                var addExpression = denominator as BinaryExpressionSyntax;

                string valLeft = GetConstValue(addExpression.Left, context);
                string valRight = GetConstValue(addExpression.Right, context);
                string res = valLeft + valRight;

                double val1 = 0;
                double val2 = 0;
                if (double.TryParse(valLeft, out val1)
                        && double.TryParse(valRight, out val2))
                    res = (val1 + val2).ToString();
                //Console.WriteLine("I GOT THE VALUE, its: {0}", res);
                return res;
            }
            else if (denominator.IsKind(SyntaxKind.SubtractExpression))
            {
                var subExpression = denominator as BinaryExpressionSyntax;
                var res = GetConstValue(subExpression.Left, context)
                       == GetConstValue(subExpression.Right, context) ? "0" : "1";
                //Console.WriteLine("I GOT THE VALUE, its: {0}", res);
                return res;
            }
            else if (denominator.IsKind(SyntaxKind.MultiplyExpression))
            {
                var multExpression = denominator as BinaryExpressionSyntax;

                string valLeft = GetConstValue(multExpression.Left, context);
                string valRight = GetConstValue(multExpression.Right, context);
                string res = "null";

                double val1 = 0;
                double val2 = 0;
                if (double.TryParse(valLeft, out val1)
                        && double.TryParse(valRight, out val2))
                    res = (val1 * val2).ToString();

                //Console.WriteLine("I GOT THE VALUE, its: {0}", res);
                return res;
            }
            else if (denominator.IsKind(SyntaxKind.ParenthesizedExpression))
            {
                var parenthExpression = denominator as ParenthesizedExpressionSyntax;
                var mas = parenthExpression.DescendantNodes().OfType<ExpressionSyntax>();
                foreach (var elem in mas)
                {
                    //Console.WriteLine("ELEM in MAS is: {0}", elem);
                    var tmp_res = GetConstValue(elem, context);
                    if (tmp_res == "0" || tmp_res == "null")
                    {
                        return "null";
                    }
                    else
                        return "Passed";
                }
                return "null";
            }
            else if (denominator.IsKind(SyntaxKind.IdentifierName))
            {
                var Variable = denominator as IdentifierNameSyntax;
                var res = Variable.Identifier.Value;
                return res.ToString();
            }
            else
            {
                //Console.WriteLine("I GOT THE VALUE, its: {0}", "null");
                return "null";
            }
        }
    }
}
