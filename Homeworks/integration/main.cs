using System;
using System.IO;
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
        double res1 = Integrator.integrate(test1, a_test, b_test);
        Console.WriteLine($"    ∫ √(x) dx = {res1:F4}");

        // 1/√(x)
        Func<double, double> test2 = (double x) => 1.0 / Math.Sqrt(x);
        double res2 = Integrator.integrate(test2, a_test, b_test);
        Console.WriteLine($"    ∫ 1/√(x) dx = {res2:F4}");

        // √(1 - x²)
        Func<double, double> test3 = (double x) => Math.Sqrt(1.0 - x * x);
        double res3 = Integrator.integrate(test3, a_test, b_test);
        Console.WriteLine($"    ∫ √(1 - x²) dx = {res3:F4}");

        // ln(x)/√(x)
        Func<double, double> test4 = (double x) => Math.Log(x) / Math.Sqrt(x);
        double res4 = Integrator.integrate(test4, a_test, b_test);
        Console.WriteLine($"    ∫ ln(x)/√(x) dx = {res4:F4}\n");

        // ----- Investigating the error function ----- //
        Console.WriteLine("Investigating the error function ...");

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

        // ----- erf(1) compared to the analytical solution as a function of absolute accuracy ----- //
        Console.WriteLine("Comparing erf(1) to the analytical solution as a function of absolute accuracy ...");
        

    }
}