// Practical programming and numerical methods 2025 examination project
// Project 11: Symmetric row/column update of a size-n symmetric eigenvalue problem
// Given the diagonal elements of the matrix D, the vector u, and the integer p, calculate the eigenvalues of the matrix A using O(n²) operations

using System;
using System.IO;
using System.Diagnostics;
using vector_class;
using matrix_class;
using eigenupdate_class;

public static class Program
{

    public static void Main(string[] args)
    {

        // ----- Execute the eigenvalue update method for demonstration ----- //
        Console.WriteLine("Executing the eigenvalue update method with randomly generated parameters for demonstration...\n");

        // Generate d, u, and p randomly
        int minSize = 2; // Minimum size of the matrix
        int maxSize = 10; // Maximum size of the matrix
        var rand = new Random();
        int n = rand.Next(minSize, maxSize + 1);
        int p = rand.Next(0, n);
        Console.WriteLine($"Random matrix size n ∈ [{minSize}, {maxSize}]:  {n}");
        Console.WriteLine($"Random index p ∈ [0, {n - 1}]:         {p}\n");

        double dmin = -10 * n;
        double dmax = 10 * n;
        vector d = EigenUpdate.randomVector(n, dmin, dmax);
        d.print($"Randomly generated diagonal elements of {n}x{n} matrix D:\n");
        Console.WriteLine();

        vector u = EigenUpdate.randomVector(n, 0.95 * dmin, 0.95 * dmax);
        u[p] = 0.0;
        u.print("Randomly generated vector u with p'th element set to zero:\n");
        Console.WriteLine();

        // Do the eigenvalue update and extract the results
        var (A, secular, e, poles, bounds, λ0s, λs) = EigenUpdate.eigenvalueUpdate(d, u, p, dmin, dmax, print: true);

        // Export the secular function, eigenvalues, and poles to a file
        Console.WriteLine("Exporting the secular function, eigenvalues, and poles to files...");
        using (StreamWriter writer = new StreamWriter("secular.txt"))
        {
            writer.WriteLine("# λ\tf(λ)");
            for (double λ = λs[0] - 0.2 * Math.Abs(λs[0]); λ <= λs[n - 1] + 0.2 * Math.Abs(λs[n - 1]); λ += 0.01)
            {
                vector result = secular(new vector(λ));
                writer.WriteLine($"{λ}\t{result[0]}");
            }
        }
        Console.WriteLine("    Secular function exported to 'secular.txt'.");
        using (StreamWriter writer = new StreamWriter("eigenvalues.txt"))
        {
            writer.WriteLine("# λ\tf(λ)");
            for (int i = 0; i < n; i++)
            {
                writer.WriteLine($"{λs[i]}\t{0.0}");
            }
        }
        Console.WriteLine("    Eigenvalues exported to 'eigenvalues.txt'.");
        using (StreamWriter writer = new StreamWriter("poles.txt"))
        {
            writer.WriteLine("# x\ty");
            for (int i = 0; i < n - 1; i++)
            {
                writer.WriteLine($"{poles[i]}\t{0.0}");
            }
        }
        Console.WriteLine("    Poles of the secular function exported to 'poles.txt'.\n");
        Console.WriteLine("All files exported successfully. The plot secular.pdf shows the secular function, eigenvalues, and poles.\n");

        // ----- Timing the execution for increasing n ----- //
        Console.WriteLine("Timing the execution of the eigenvalue update method for increasing n...");

        using (StreamWriter writer = new StreamWriter("timing.txt"))
        {
            writer.WriteLine("# Matrix size n\tTime [ms]");
            for (int n_timing = 2; n_timing <= 200; n_timing++)
            {
                // Generate random d, u, and p
                var rand_timing = new Random();
                double dmin_timing = -10 * n_timing;
                double dmax_timing = 10 * n_timing;

                int p_timing = rand_timing.Next(0, n_timing);
                vector d_timing = EigenUpdate.randomVector(n_timing, dmin_timing, dmax_timing);
                vector u_timing = EigenUpdate.randomVector(n_timing, 0.95 * dmin_timing, 0.95 * dmax_timing);
                u_timing[p_timing] = 0.0;

                // Start the stopwatch
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Execute the eigenvalue update method
                EigenUpdate.eigenvalueUpdate(d_timing, u_timing, p_timing, dmin_timing, dmax_timing, print: false);

                // Stop the stopwatch
                stopwatch.Stop();

                // Export the timing result
                writer.WriteLine($"{n_timing}\t{stopwatch.Elapsed.TotalMilliseconds}");
            }
        }
        Console.WriteLine("Timing results exported to 'timing.txt'.");
        Console.WriteLine("The plot timing.pdf shows the execution time as a function of n, where the O(n²) behavior is clearly seen.");

    }
}