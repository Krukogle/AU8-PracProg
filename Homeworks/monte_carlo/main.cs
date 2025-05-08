using System;
using System.IO;
using static System.Math;
using monte_carlo_class;
using vector_class;

public static class Program{

    public static void Main(string[] args){

        // ---------- PART A ---------- //
        Console.WriteLine("---------- PART A ----------");

        // Testing Monte-Carlo routine on the area of the unit circle and ∫ 1/(1-cos(x)cos(y)cos(z)) dx dy dz from 0 to π
        Console.WriteLine("Testing plain Monte-Carlo routine on the area of the unit circle ...");

        // The area of the unit circle
        vector a = new vector(-1, -1);
        vector b = new vector(1, 1);
        Func<vector, double> circle = (vector x) => (x[0] * x[0] + x[1] * x[1] <= 1.0) ? 1.0 : 0.0;
        double area = 0.0;
        double error = 0.0;
        double error_plain = 0.0;
        using(StreamWriter writer = new StreamWriter("circle_errors.txt")){
            writer.WriteLine("# N Error(estimated) Error(exact)");
            for(double N = 10; N <= 100000; N *= 1.1){
                var result_plain = MonteCarlo.plainmc(circle, a, b, (int)N);
                area = result_plain.Item1;
                error = result_plain.Item2;
                error_plain = Math.Abs(Math.PI - area);
                writer.WriteLine($"{N} {error} {error_plain}");
            }
        }
        Console.WriteLine($"Estimated area of the unit circle (should be π): {area:F6} ± {error:F6}");
        Console.WriteLine($"Actual error: {error_plain:F6}.\n");
        Console.WriteLine("Results of error comparison written to circle_errors.txt");
        Console.WriteLine("The plot 'circle_errors.pdf' shows the estimated error and the actual error as a function of N.");
        Console.WriteLine("It also shows that the actual error scales as 1/√N.\n");

        // Testing Monte-Carlo routine on ∫ 1/(1-cos(x)cos(y)cos(z)) dx dy dz from 0 to π
        Console.WriteLine("Testing plain Monte-Carlo routine on ∫ 1/(1-cos(x)cos(y)cos(z)) dx/π dy/π dz/π from 0 to π ...");
        vector a_3D = new vector(0, 0, 0);
        vector b_3D = new vector(PI, PI, PI);
        Func<vector, double> integrand = (vector x) => 1/(PI * PI * PI) * 1/(1 - Cos(x[0]) * Cos(x[1]) * Cos(x[2]));
        var result_3D = MonteCarlo.plainmc(integrand, a_3D, b_3D, 1000000);
        Console.WriteLine($"Result (should be 1.393203): ∫ 1/(1-cos(x)cos(y)cos(z)) dx/π dy/π dz/π = {result_3D.Item1:F6} ± {result_3D.Item2:F6}\n");

        // ---------- PART B ---------- //
        Console.WriteLine("---------- PART B ----------");

        // Testing quasi-random Monte-Carlo routine on the area of the unit circle
        Console.WriteLine("Testing quasi-random Monte-Carlo routine on the area of the unit circle ...");
        var result_quasi = MonteCarlo.quasimc(circle, a, b, 1000000);
        double error_quasi = Math.Abs(Math.PI - result_quasi.Item1);
        Console.WriteLine($"Estimated area of the unit circle (should be π): {result_quasi.Item1:F6} ± {result_quasi.Item2:F6}");
        Console.WriteLine($"Actual error: {error_quasi:F6}. This is ~{(int)(error_plain/error_quasi)} times smaller than the actual plain Monte-Carlo error.\n");

        // ---------- PART C ---------- //
        Console.WriteLine("---------- PART C ----------");

        // Testing recursive stratisfied Monte-Carlo routine on the area of the unit circle
        Console.WriteLine("Testing recursive stratisfied Monte-Carlo routine on the area of the unit circle ...");
        var result_strat = MonteCarlo.stratifiedmc(circle, a, b);
        double area_strat = result_strat.Item1;
        double error_strat = result_strat.Item2;
        Console.WriteLine($"Estimated area of the unit circle (should be π): {area_strat:F6} ± {error_strat:F6}.");
        double error_strat_plain = Math.Abs(Math.PI - area_strat);
        Console.WriteLine($"Actual error: {error_strat_plain:F6}. This is ~{(int)(error_plain/error_strat_plain)} times smaller than the actual plain Monte-Carlo error.\n");

    }
}