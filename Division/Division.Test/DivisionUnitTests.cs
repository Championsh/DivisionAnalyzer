using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = Division.Test.CSharpCodeFixVerifier<
    Division.DivisionAnalyzer,
    Division.DivisionCodeFixProvider>;

namespace Division.Test
{
    [TestClass]
    public class DivisionUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task EmptyProg()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic should be triggered
        [TestMethod]
        public async Task ZeroVariableDivision()
        {
            var test = @"
using System;

class Program
{
    static void Main()
    {
        int a = 0;
        int b = [|5 / a|];
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic should be triggered
        [TestMethod]
        public async Task ZeroExpressionDivision()
        {
            var test = @"
using System;

class Program
{
    static void Main()
    {
        int a = 2 / 1;
        int b = 3 * 2;
        int x = [|12 / (12 - a * b)|];
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic should be triggered
        [TestMethod]
        public async Task SimpleMinusExpression()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;

class Program
{
    static void Main()
    {
        int a = 4;
        int x = [|12 / (a - a)|];
    }
}
");
        }

        //Diagnostic should be triggered
        [TestMethod]
        public async Task ZeroMultiplyExpression()
        {
            var test = @"
using System;

class Program
{
    static void Main()
    {
        int a = 4;
        int x = [|12 / (0 * a)|];
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic should be triggered
        [TestMethod]
        public async Task ZeroDivision()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;

class Program
{
    static int g()
    {
        int a = 4;
        return a;
    }

    static void Main()
    {
        int x = [|g() / 0|];
    }
}
");
        }

        //No diagnostics expected to show up
        [TestMethod]
        public async Task GooodFraction()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;

class Program
{
    static void Main()
    {
        int x = 2 / 1;
        float y = 3 / x;
        int z = 4 / x;
    }
}
");
        }
    }
}
