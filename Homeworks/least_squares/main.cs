using System;
using System.IO;
using vector_class;
using matrix_class;
using qr_class;
using ols_class;

public class Program{

    public static void Main(string[] args){

        // ---------- PART A ---------- //
        Console.WriteLine("---------- Part A ----------");

        // ----- Checking that the QR algorithm works for tall matrices ----- //
        Console.WriteLine("Checking that the QR algorithm works for tall matrices ...");

        // Generate a random tall matrix A
        var rnd = new System.Random(1);
        int m_test = 7, n_test = 4;
        matrix A = new matrix(m_test, n_test);
        for(int i = 0; i < m_test; i++){
            for(int j = 0; j < n_test; j++){
                A[i,j] = rnd.NextDouble();
            }
        }
        A.print($"Random tall matrix A (m={m_test}, n={n_test}):", "{0,10:g3} ");

        // Factorize into QR
        (matrix Q, matrix R) = QR.decomp(A);
        Q.print("\nMatrix Q:", "{0,10:g3} ");
        R.print("\nMatrix R:", "{0,10:g3} ");
        Console.WriteLine();

        // Check that Q is orthogonal
        matrix QTQ = Q.transpose() * Q;
        Console.WriteLine("Checking that Q^T * Q = I within a tolerance ...");
        Console.WriteLine(QTQ.approx(matrix.id(n_test)) + "\n");
        Console.WriteLine("Check complete: The QR decomposition works for tall matrices.\n");

        // ----- OLS fitting to the Rutherford and Soddy data ----- //
        Console.WriteLine("Beginning OLS fitting to the Rutherford and Soddy data ...");

        // Load data
        double[] t_data = {1.0, 2.0, 3.0, 4.0, 6.0, 9.0, 10.0, 13.0, 15.0};   // Time in days
        double[] y_data = {117.0, 100.0, 88.0, 72.0, 53.0, 29.5, 25.2, 15.2, 11.1};   // Activity in relative units
        double[] dy_data = {6.0, 5.0, 4.0, 4.0, 4.0, 3.0, 3.0, 2.0, 2.0};   // Uncertainties in activity

        // Since we want to fit ln(y) = ln(a) - λt (dln(y) = dy/y), we need to transform the data
        int n = t_data.Length;
        double[] ln_y_data = new double[n];
        double[] ln_dy_data = new double[n];
        for(int i = 0; i < n; i++){
            ln_y_data[i] = Math.Log(y_data[i]);
            ln_dy_data[i] = dy_data[i] / y_data[i];
        }

        // Convert to vectors
        vector t_vec = new vector(t_data);
        vector ln_y_vec = new vector(ln_y_data);
        vector ln_dy_vec = new vector(ln_dy_data);

        // Define the functions to fit
        Func<double, double>[] fs = new Func<double, double>[2];
        fs[0] = (t) => 1.0;   // Constant term (ln(a))
        fs[1] = (t) => t;   // Linear term (-λt)
        
        // Perform the OLS fit
        (vector c, matrix cov) = OLS.lsfit(fs, t_vec, ln_y_vec, ln_dy_vec);

        // Extract the parameters of interest
        double a = Math.Exp(c[0]);
        double lambda = -c[1];
        double da = Math.Sqrt(cov[0, 0]);
        double dlambda = Math.Sqrt(cov[1, 1]);
        double T_half = Math.Log(2.0) / lambda;
        double dT_half = Math.Log(2.0) * dlambda / (lambda * lambda);

        // Print the results to the console
        Console.WriteLine($"Fitted parameters:");
        Console.WriteLine($"a = {a:F3} ± {da:F3}");
        Console.WriteLine($"λ = {lambda:F3} ± {dlambda:F3}");
        Console.WriteLine($"T_½ = {T_half:F3} ± {dT_half:F3} days");
        Console.WriteLine("The modern value of T_½ is 3.6316 days, which is way off the predicted value here.\n");

        // Export the data to a txt file
        Console.WriteLine("Exporting the data to a txt file ...");
        using(StreamWriter writer = new StreamWriter("data.txt")){
            for (int i = 0; i < n; i++){
                writer.WriteLine($"{t_data[i]} {y_data[i]} {dy_data[i]}");
            }
        }
        Console.WriteLine("Data exported to data.txt.\n");

        // Export the fitted values to a txt file
        Console.WriteLine("Exporting the fitted values to a txt file ...");
        using(StreamWriter writer = new StreamWriter("fit.txt")){
            double t_i = t_data[0];
            double t_f = t_data[n - 1];
            int nsteps = 100;
            double t_step = (t_f - t_i) / nsteps;
            for (int i = 0; i <= nsteps; i++){
                double t = t_i + i * t_step;
                double y_fit = a * Math.Exp(-lambda * t);
                writer.WriteLine($"{t} {y_fit}");
            }
        }
        Console.WriteLine("Fitted values exported to fit.txt.");
        Console.WriteLine("Data and fitted values plotted as data_fit.pdf.\n");

        // ---------- PART B ---------- //
        Console.WriteLine("---------- Part B ----------");

        // Print the covariance matrix
        cov.print("Covariance matrix:", "{0,10:g3} ");
        Console.WriteLine();

        // Compare half-life uncertainty to modern value
        Console.WriteLine($"The predicted uncertainty in the half-life is {dT_half:F3} days.");
        Console.WriteLine("The modern value is 0.0014 days, which is much smaller than the predicted value here.\n");

        // ---------- PART C ---------- //
        Console.WriteLine("---------- Part C ----------");

        // Export the fitted values to a txt file with the fit coefficients at each uncertainty limit
        Console.WriteLine("Exporting the fitted values to a txt file with the fit coefficients at each uncertainty limit ...");
        using(StreamWriter writer = new StreamWriter("fit_limits.txt")){
            double t_i = t_data[0];
            double t_f = t_data[n - 1];
            int nsteps = 100;
            double t_step = (t_f - t_i) / nsteps;
            for (int i = 0; i <= nsteps; i++){
                double t = t_i + i * t_step;
                double y_fit_min = (a - da) * Math.Exp(-(lambda + dlambda) * t);
                double y_fit_max = (a + da) * Math.Exp(-(lambda - dlambda) * t);
                writer.WriteLine($"{t} {y_fit_min} {y_fit_max}");
            }
        }
        Console.WriteLine("Fitted values with uncertainty limits exported to fit_limits.txt and plotted as fit_limits.pdf.");

    }
}