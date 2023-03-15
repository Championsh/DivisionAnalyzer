**ZERO Division Analyzer**

In this project **Roslyn-Based** **Zero Denominator Detector** was created.

The analyzer detects Following Fraction Patterns:
+ If the denominator value is Numeric Literal (0);
+ If the value is const string (e.g. .../"abc") then the Fraction will be Pointed Out as Zero-denominator err;
+ If denominator is an expression (e.g. 1+1, (12+14), (a+b), 0*17, a-a) and it's value is "none" or "0";

**To-do:** Add the value-of-parametr Checker (e.g. {int a = 0; int b = 5/a}).
