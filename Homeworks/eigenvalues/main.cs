using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using vector_class;
using matrix_class;
using jacobi_class;

public class Program{

    // Hamiltonian builder for part B
    public static matrix Hamiltonian(int npoints, double dr, vector r){
        matrix H = new matrix(npoints, npoints);
        for(int i = 0; i < npoints - 1; i++){
            H[i, i] = -2.0 * (-0.5 / dr / dr);
            H[i, i + 1] = 1.0 * (-0.5 / dr / dr);
            H[i + 1, i] = 1.0 * (-0.5 / dr / dr);
        }
        H[npoints - 1, npoints - 1] = -2.0 * (-0.5 / dr / dr);
        for(int i = 0; i < npoints; i++){
            H[i, i] += -1.0/r[i];
        }
        return H;
    }

    public static void Main(string[] args){

        // ---------- PART A ---------- //
        Console.WriteLine("---------- PART A ----------");
        // Generate a random symmetric matrix A
        int n = 5;
        matrix A = new matrix(n, n);
        Random rand = new Random();
        for(int i = 0; i < n; i++){
            for(int j = 0; j < n; j++){
                A[i, j] = rand.NextDouble();
            }
        }
        for(int i = 0; i < n; i++){
            for(int j = i + 1; j < n; j++){
                A[j, i] = A[i, j];
            }
        }
        A.print("Matrix A:");
        Console.WriteLine();

        // Extract the orthogonal matrix V of eigenvectors
        matrix V = matrix.id(n);
        vector w = new vector(n);
        (w, V) = Jacobi.cyclic(A);

        // Check V^T * A * V = D
        matrix D_check = V.transpose() * A * V;
        D_check.print("The product V^T * A * V yields:"); 
        Console.WriteLine();

        // Print the eigenvalues and check if they match the diagonal of D
        w.print("Eigenvalue vector w from Jacobi:");
        Console.WriteLine();
        Console.WriteLine("Are the eigenvalues in w equal to the diagonal of D within a tolerance?");
        bool areEqual = true;
        for(int i = 0; i < n; i++){
            if(Math.Abs(D_check[i, i] - w[i]) > 1e-10){
                areEqual = false;
                break;
            }
        }
        Console.WriteLine(areEqual);
        Console.WriteLine();

        // Check that V * D * V^T = A
        matrix A_check = V * D_check * V.transpose();
        A_check.print("The product V * D * V^T yields:");
        Console.WriteLine();
        Console.WriteLine("Is V * D * V^T equal to A within a tolerance?");
        Console.WriteLine(A_check.approx(A));
        Console.WriteLine();

        // Check that V^T * V = V * V^T = I
        matrix I_check1 = V.transpose() * V;
        matrix I_check2 = V * V.transpose();
        I_check1.print("The product V^T * V yields:");
        Console.WriteLine();
        Console.WriteLine("Is V^T * V equal to I within a tolerance?");
        Console.WriteLine(I_check1.approx(matrix.id(n)));
        Console.WriteLine();

        I_check2.print("The product V * V^T yields:");
        Console.WriteLine();
        Console.WriteLine("Is V * V^T equal to I within a tolerance?");
        Console.WriteLine(I_check2.approx(matrix.id(n)));
        Console.WriteLine();



        // ---------- PART B ---------- //
        Console.WriteLine("---------- PART B ----------");
        
        // Set default values of rmax and dr
        double rmax = 10.0;
        double dr = 0.3;

        // Extract new values of rmax and dr from command line arguments if provided
        for(int i = 0; i < args.Length; i++){
            if(args[i] == "-rmax"){
                rmax = double.Parse(args[i + 1]);
            }
            if(args[i] == "-dr"){
                dr = double.Parse(args[i + 1]);
            }
        }

        // Build the Hamiltonian matrix
        int npoints = (int)(rmax / dr) - 1;
        Console.WriteLine($"Recieved inputs: rmax = {rmax}, dr = {dr} => npoints = {npoints}");
        vector r = new vector(npoints);
        for(int i = 0; i < npoints; i++){
            r[i] = dr * (i + 1);
        }

        matrix H = Hamiltonian(npoints, dr, r);

        // Extract eigenvalues and eigenvectors
        (vector w2, matrix V2) = Jacobi.cyclic(H);
        w2.print("Eigenvalue vector of H (w) from Jacobi method:");
        Console.WriteLine();

        // ----- Convergence check of rmax and dr ----- //
        Console.WriteLine("Convergence check of rmax and dr ...");
        
        // Keep rmax fixed and vary dr
        using(StreamWriter writer = new StreamWriter("varying_dr.txt")){
            for(double dr_test = 0.05; dr_test <= 0.5; dr_test += 0.01){
                int npoints_test = (int)(rmax / dr_test) - 1;
                vector r_test = new vector(npoints_test);
                for(int i = 0; i < npoints_test; i++){
                    r_test[i] = dr_test * (i + 1);
                }
                matrix H_test = Hamiltonian(npoints_test, dr_test, r_test);
                vector w_test = Jacobi.cyclic(H_test).Item1;
                writer.WriteLine($"{dr_test:f10} {w_test[0]:f10}");
            }
        }

        // Keep dr fixed and vary rmax
        using(StreamWriter writer = new StreamWriter("varying_rmax.txt")){
            for(double rmax_test = 5.0; rmax_test <= 20.0; rmax_test += 0.5){
                int npoints_test = (int)(rmax_test / dr) - 1;
                vector r_test = new vector(npoints_test);
                for(int i = 0; i < npoints_test; i++){
                    r_test[i] = dr * (i + 1);
                }
                matrix H_test = Hamiltonian(npoints_test, dr, r_test);
                vector w_test = Jacobi.cyclic(H_test).Item1;
                writer.WriteLine($"{rmax_test:f10} {w_test[0]:f10}");
            }
        }
        Console.WriteLine("Convergence check files varying_dr.txt and varying_rmax.txt written.\n");

        // ----- Wave function check ----- //
        double N = 1.0 / Math.Sqrt(dr); // Normalization constant
        Console.WriteLine("Wave function check ...");

        // n=1
        Console.WriteLine("Comparing the n=1 approximated wave function with the analytical result ...");
        using(StreamWriter writer = new StreamWriter("radial_n1.txt")){
            for(int i = 0; i < npoints; i++){
                double f0_num = N * V2[i, 0];
                double f0_ana = 2.0 * r[i] * Math.Exp(-r[i]);
                writer.WriteLine($"{r[i]:f10} {f0_num:f10} {f0_ana:f10}");
            }
        }
        Console.WriteLine("Comparison complete. Results written to radial_n1.txt and plotted as radial_n1.pdf.\n");

        // n=2
        Console.WriteLine("Comparing the n=2 approximated wave function with the analytical result ...");
        using(StreamWriter writer = new StreamWriter("radial_n2.txt")){
            for(int i = 0; i < npoints; i++){
                double f1_num = N * V2[i, 1];
                double f1_ana = 1.0 / Math.Sqrt(2.0) * (r[i] / 2.0 - 1.0) * r[i] * Math.Exp(-r[i] / 2.0);
                writer.WriteLine($"{r[i]:f10} {f1_num:f10} {f1_ana:f10}");
            }
        }
        Console.WriteLine("Comparison complete. Results written to radial_n2.txt and plotted as radial_n2.pdf.\n");

        // n=3
        Console.WriteLine("Comparing the n=3 approximated wave function with the analytical result ...");
        using(StreamWriter writer = new StreamWriter("radial_n3.txt")){
            for(int i = 0; i < npoints; i++){
                double f2_num = N * V2[i, 2];
                double f2_ana = 2.0/(27.0*Math.Sqrt(3.0)) * r[i] * (27.0 - 18.0*r[i] + 2.0*r[i]*r[i]) * Math.Exp(-r[i] / 3.0);
                writer.WriteLine($"{r[i]:f10} {f2_num:f10} {f2_ana:f10}");
            }
        }
        Console.WriteLine("Comparison complete. Results written to radial_n3.txt and plotted as radial_n3.pdf.\n");



        // ---------- PART C ---------- //
        Console.WriteLine("---------- PART C ----------");
        Console.WriteLine("Checking that the Jacobi diagonalization scales as O(n^3) with multiprocessing ...");

        // Build the list of sizes 10,20,…,200
        var sizes = Enumerable.Range(1, 20).Select(i => i * 10);

        // Thread-safe bag to collect (N, time)
        var results = new ConcurrentBag<(int N, double TimeMs)>();

        Parallel.ForEach(sizes, npoints_test =>
        {
            // Build r_test and H_test just as before
            var r_test = new vector(npoints_test);
            for (int i = 0; i < npoints_test; i++)
                r_test[i] = dr * (i + 1);

            var H_test = Hamiltonian(npoints_test, dr, r_test);

            // Time the diagonalization
            var sw = Stopwatch.StartNew();
            Jacobi.cyclic(H_test);
            sw.Stop();

            // Add to bag
            results.Add((npoints_test, sw.Elapsed.TotalMilliseconds));
        });

        // Write out in ascending N
        using (var writer = new StreamWriter("timing.txt"))
        {
            // OrderBy uses 'p' so we don't shadow 'r'
            foreach (var entry in results.OrderBy(p => p.N))
            {
                writer.WriteLine($"{entry.N} {entry.TimeMs}");
            }
        }

        Console.WriteLine("Check complete. Timing data written to timing.txt and plotted as timing.pdf.");

    }
}