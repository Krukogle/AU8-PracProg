using System;
using System.IO;
using System.Diagnostics;
using integrator_class;

public static class Program{

    // Error function approximation (Abramowitz and Stegun on Wikipedia)
    public static double erf_exact(double x){
        if(x < 0){
            return -erf_exact(-x);
        }
        double[] a = {0.254829592, -0.284496736, 1.421413741, -1.453152027, 1.061405429};
        double t = 1.0 / (1.0 + 0.3275911 * x);
        double sum = t * (a[0] + t * (a[1] + t * (a[2] + t * (a[3] + t * a[4]))));
        return 1.0 - sum * Math.Exp(-x * x);
        }
    
    // Approximation of two doubles
    public static bool approx(double x, double y, double acc=1e-9, double eps=1e-9){
	if(Math.Abs(x-y)<acc)return true;
	if(Math.Abs(x-y)/Math.Max(Math.Abs(x),Math.Abs(y))<eps)return true;
	return false;
	}

    // Main function
    public static void Main(string[] args){

        // ---------- PART A ---------- //
        Console.WriteLine("---------- PART A ----------");

        // ----- Testing integrator on provided examples from 0 to 1 within absolute and relative precission of 0.001 ----- //
        Console.WriteLine("Testing integrator on provided examples from 0 to 1 within absolute and relative precission of 0.001 ...");
        double a_test = 0.0;
        double b_test = 1.0;

        // √(x)
        Func<double, double> test1 = (double x) => Math.Sqrt(x);
        var res1 = Integrator.integrate(test1, a_test, b_test);
        Console.WriteLine($"    ∫ √(x) dx = {res1.Item1:F4}");

        // 1/√(x)
        Func<double, double> test2 = (double x) => 1.0 / Math.Sqrt(x);
        var res2 = Integrator.integrate(test2, a_test, b_test);
        Console.WriteLine($"    ∫ 1/√(x) dx = {res2.Item1:F4}");

        // √(1 - x²)
        Func<double, double> test3 = (double x) => Math.Sqrt(1.0 - x * x);
        var res3 = Integrator.integrate(test3, a_test, b_test);
        Console.WriteLine($"    ∫ √(1 - x²) dx = {res3.Item1:F4}");

        // ln(x)/√(x)
        Func<double, double> test4 = (double x) => Math.Log(x) / Math.Sqrt(x);
        var res4 = Integrator.integrate(test4, a_test, b_test);
        Console.WriteLine($"    ∫ ln(x)/√(x) dx = {res4.Item1:F4}\n");

        // ----- Investigating the error function ----- //
        Console.WriteLine("Investigating the error function ...\n");

        // Plotting the error function from -3 to 3 with integral representation
        Console.WriteLine("Plotting the error function from -3 to 3 with integral representation ...");
        using(StreamWriter writer = new StreamWriter("erf.txt")){
            writer.WriteLine("# x erf_approx erf_exact");
            for(double x = -3; x <= 3.0001; x += 0.1){
                double approx = Integrator.erf(x);
                double exact = erf_exact(x);
                writer.WriteLine($"{x:F2} {approx:F8} {exact:F8}");
            }
        }
        Console.WriteLine("Error function data written to erf.txt.");
        Console.WriteLine("The plot 'erf.pdf' shows the comparison of the approximated and exact error function.\n");

        // ----- erf(1) compared to the analytical solution as a function of absolute accuracy ----- //
        Console.WriteLine("Comparing erf(1) to the analytical solution as a function of absolute accuracy (eps = 0) ...");
        double erf1 = 0.84270079294971486934;
        using(StreamWriter writer = new StreamWriter("erf1.txt")){
            writer.WriteLine("# acc erf1_approximated absolute_error");
            for(double acc = 0.00000001; acc <= 0.1; acc *= 1.1){
                double erf1_approx = Integrator.erf(x: 1.0, acc: acc, eps: 0.0);
                double abs_error = Math.Abs(erf1_approx - erf1);
                writer.WriteLine($"{acc:F8} {erf1_approx:F8} {abs_error:F16}");
            }
        }
        Console.WriteLine("Error function data for erf(1) written to erf1.txt.");
        Console.WriteLine("The plot 'erf1.pdf' shows the convergence of the approximated error function to the analytical solution.\n");

        // ---------- PART B ---------- //
        Console.WriteLine("---------- PART B ----------");

        // Comparing Clenshaw–Curtis integration with the standard integration method using provided examples
        Console.WriteLine("Comparing Clenshaw-Curtis integration with the standard integration method using provided examples ...");

        // 1/√(x)
        var res2_cc = Integrator.integrate_cc(test2, a_test, b_test);
        Console.WriteLine($"    ∫ 1/√(x) dx = {res2_cc.Item1:F4} (Clenshaw-Curtis)");
        Console.WriteLine($"     - Number of function calls: {res2_cc.Item2}");
        Console.WriteLine($"     - Number of function calls (standard): {res2.Item2}\n");

        // ln(x)/√(x)
        var res4_cc = Integrator.integrate_cc(test4, a_test, b_test);
        Console.WriteLine($"    ∫ ln(x)/√(x) dx = {res4_cc.Item1:F4} (Clenshaw-Curtis)");
        Console.WriteLine($"     - Number of function calls: {res4_cc.Item2}");
        Console.WriteLine($"     - Number of function calls (standard): {res4.Item2}\n");

        // Switching over to Python and comparing with SciPy's integration routine ...
        Console.WriteLine("Switching over to Python and comparing with SciPy's integration routine ...");
        var psi1 = new ProcessStartInfo{
            FileName = "python3",
            Arguments = "comparison_definite.py",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using(var process = Process.Start(psi1)){
            string pyout = process.StandardOutput.ReadToEnd();
            Console.WriteLine(pyout);
            process.WaitForExit();
        }
        Console.WriteLine("Switching back to C# ...\n");

        // Testing integrator with infinite limits
        Console.WriteLine("Testing integrator with infinite limits ...\n");

        // ∫ exp(-x²) dx from -∞ to ∞
        Console.WriteLine("The Gaussian integral from -∞ to ∞ ...");
        Func<double, double> test5 = (double x) => Math.Exp(-x * x);
        var res5 = Integrator.integrate(test5, double.NegativeInfinity, double.PositiveInfinity);
        Console.WriteLine($"    ∫ exp(-x²) dx = {res5.Item1:F4}");
        Console.WriteLine($"     - Number of function calls: {res5.Item2}");
        Console.WriteLine(
            $"     The correct result is √π. Is this result correct within a tolerance? " +
            $"{approx(res5.Item1, Math.Sqrt(Math.PI), acc: 0.001)}.\n"
        );

        // ∫ 1/(1 + x^2) dx from 0 to ∞
        Console.WriteLine("The integral of 1/(1 + x²) from 0 to ∞ ...");
        Func<double, double> test6 = (double x) => 1.0 / (1.0 + x * x);
        var res6 = Integrator.integrate(test6, 0.0, double.PositiveInfinity);
        Console.WriteLine($"    ∫ 1/(1 + x²) dx = {res6.Item1:F4}");
        Console.WriteLine($"     - Number of function calls: {res6.Item2}");
        Console.WriteLine(
            $"     The correct result is π/2. Is this result correct within a tolerance? " +
            $"{approx(res6.Item1, Math.PI / 2, acc: 0.001)}.\n"
        );

        // Switching over to Python and comparing with SciPy's integration routine ...
        Console.WriteLine("Switching over to Python and comparing with SciPy's integration routine ...");
        var psi2 = new ProcessStartInfo{
            FileName = "python3",
            Arguments = "comparison_improper.py",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using(var process = Process.Start(psi2)){
            string pyout = process.StandardOutput.ReadToEnd();
            Console.WriteLine(pyout);
            process.WaitForExit();
        }
        Console.WriteLine("Switching back to C# ...\n");

        // ---------- PART C ---------- //
        Console.WriteLine("---------- PART C ----------");

        // Including the integration error
        Console.WriteLine("Including the integration error ...\n");

    }
}