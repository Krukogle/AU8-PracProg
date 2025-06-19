using System;
using System.IO;
using System.Collections.Generic;
using vector_class;
using minimiser_class;

public static class Program
{

    public static void Main(string[] args)
    {

        // ---------- PART A ---------- //
        Console.WriteLine("----------- PART A ----------");
        Console.WriteLine("Newton's method with numerical gradient, numerical Hessian matrix and back-tracking linesearch.\n");

        // Rosenbrock's valley function f(x,y) = (1-x)² + 100*(y-x²)²
        Console.WriteLine("The Rosenbrock's valley function f(x,y) = (1-x)² + 100*(y-x²)²");
        Console.WriteLine("According to Wikipedia, the global minimum is at (1, 1²) = (1, 1)...");

        Func<vector, double> rosenbrock = (x) => (1 - x[0]) * (1 - x[0]) + 100 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]);
        vector xi_rosenbrock = new vector(2.0, 2.0);   // Initial guess
        xi_rosenbrock.print("Initial guess for Rosenbrock's valley function:\n");
        (vector min_rosenbrock, int iter_rosenbrock) = Minimiser.newton(rosenbrock, xi_rosenbrock);

        min_rosenbrock.print("Minimum of Rosenbrock's valley function found at:\n");
        Console.WriteLine($"Number of iterations: {iter_rosenbrock}");
        Console.WriteLine($"Is this correct within a tolerance of 1%? {vector.approx(min_rosenbrock, new vector(1.0, 1.0), eps: 0.01)}");
        Console.WriteLine();

        // Himmelblau's function f(x,y) = (x² + y - 11)² + (x + y² - 7)²
        Console.WriteLine("The Himmelblau's function f(x,y) = (x² + y - 11)² + (x + y² - 7)²");
        Console.WriteLine("According to Wikipedia, it has one local maximum and four local minima.");
        Console.WriteLine("We will try to find the minimum at (x, y) = (3, 2)...");

        Func<vector, double> himmelblau = (x) => Math.Pow(x[0] * x[0] + x[1] - 11, 2) + Math.Pow(x[0] + x[1] * x[1] - 7, 2);
        vector xi_himmelblau = new vector(5.0, 5.0);   // Initial guess
        xi_himmelblau.print("Initial guess for Himmelblau's function:\n");
        (vector min_himmelblau, int iter_himmelblau) = Minimiser.newton(himmelblau, xi_himmelblau);

        min_himmelblau.print("Minimum of Himmelblau's function found at:\n");
        Console.WriteLine($"Number of iterations: {iter_himmelblau}");
        Console.WriteLine($"Is this correct within a tolerance of 1%? {vector.approx(min_himmelblau, new vector(3.0, 2.0), eps: 0.01)}");
        Console.WriteLine();

        // ---------- PART B ---------- //
        Console.WriteLine("----------- PART B ----------");
        Console.WriteLine("Higgs boson discovery.\n");

        // Reading the data from the file "higgs.txt"
        var energyList = new List<double>();
        var signalList = new List<double>();
        var errorList = new List<double>();
        var separators = new char[] { ' ', '\t' };
        var options = StringSplitOptions.RemoveEmptyEntries;

        foreach (string line in File.ReadLines("higgs.txt"))
        {
            if (line.Trim().StartsWith("#") || line.Trim() == "") continue;
            string[] words = line.Split(separators, options);
            energyList.Add(double.Parse(words[0]));
            signalList.Add(double.Parse(words[1]));
            errorList.Add(double.Parse(words[2]));
        }
        vector energy = new vector(energyList.ToArray());
        vector signal = new vector(signalList.ToArray());
        vector error = new vector(errorList.ToArray());

        // Fitting the Breit–Wigner function by minimising the deviation function
        Console.WriteLine("Fitting the Breit-Wigner function by minimising the deviation function D(m, Γ, A)...");
        Func<double, double, double, double, double> breit_wigner = (E, m, Γ, A) => A / ((E-m) * (E-m) + Γ * Γ / 4.0);
        Func<vector, vector, vector, vector, double> deviation = (fit_params, E, σ, Δσ) =>
        {
            double m = fit_params[0];   // Mass
            double Γ = fit_params[1];   // Width
            double A = fit_params[2];   // Amplitude
            double sum = 0.0;
            for (int i = 0; i < E.size; i++)
            {
                double fit_value = breit_wigner(E[i], m, Γ, A);
                double diff = (σ[i] - fit_value) / Δσ[i];
                sum += diff * diff;   // Sum of squares of the deviations
            }
            return sum;
        };
        Func<vector, double> Higgs_fit = (fit_params) => deviation(fit_params, energy, signal, error);

        // Initial guess for the fit parameters
        vector xi_higgs = new vector(125.0, 3.0, 10.0);   // Initial guess for (m, Γ, A)
        xi_higgs.print("Initial guess for Higgs boson fit parameters (m, Γ, A):\n");

        // Perform the minimisation using Newton's method
        (vector min_higgs, int iter_higgs) = Minimiser.newton(Higgs_fit, xi_higgs, max_iter: 10000);
        min_higgs.print("Minimum of the deviation function found at (m, Γ, A):\n");
        Console.WriteLine($"Number of iterations: {iter_higgs}");

        // Data file for plot of fit function
        using (StreamWriter writer = new StreamWriter("higgs_fit.txt"))
        {
            double Ei = energy[0];
            double Ef = energy[energy.size - 1];
            double dE = (Ef - Ei) / 250.0;
            for (double E = Ei; E <= Ef; E += dE)
            {
                double fit_value = breit_wigner(E, min_higgs[0], min_higgs[1], min_higgs[2]);
                writer.WriteLine($"{E} {fit_value}");
            }
        }
        Console.WriteLine("Fit data for Higgs saved to 'higgs_fit.txt'. The plot 'higgs.pdf' shows the results.\n");

        // ---------- PART C ---------- //
        Console.WriteLine("----------- PART C ----------");
        Console.WriteLine("Part A, but with central differences for the gradient and Hessian matrix.\n");

        // Repeating the minimisation for Rosenbrock's valley function with central differences
        Console.WriteLine("Rosenbrock's valley function with central differences for gradient and Hessian matrix...");
        (vector min_rosenbrock_central, int iter_rosenbrock_central) = Minimiser.newton(rosenbrock, xi_rosenbrock, central: true);
        min_rosenbrock_central.print("Minimum of Rosenbrock's valley function found at (central differences):\n");
        Console.WriteLine($"Number of iterations: {iter_rosenbrock_central}");
        Console.WriteLine($"Is this correct within a tolerance of 1%? {vector.approx(min_rosenbrock_central, new vector(1.0, 1.0), eps: 0.01)}\n");

        // Repeating the minimisation for Himmelblau's function with central differences
        Console.WriteLine("Himmelblau's function with central differences for gradient and Hessian matrix...");
        (vector min_himmelblau_central, int iter_himmelblau_central) = Minimiser.newton(himmelblau, xi_himmelblau, central: true);
        min_himmelblau_central.print("Minimum of Himmelblau's function found at (central differences):\n");
        Console.WriteLine($"Number of iterations: {iter_himmelblau_central}");
        Console.WriteLine($"Is this correct within a tolerance of 1%? {vector.approx(min_himmelblau_central, new vector(3.0, 2.0), eps: 0.01)}");
        Console.WriteLine("So the central method works :)");
        
        // Comparing the methods
        Console.WriteLine("To better compare the methods, we do the test again but starting very far away from the minimum...\n");

        // Repeating the minimisation for Rosenbrock's valley function with central differences, starting far from the minimum
        vector xi_rosenbrock_far = new vector(100.0, 100.0);   // Initial guess far from the minimum
        xi_rosenbrock_far.print("Initial guess for Rosenbrock's valley function (far from minimum):\n");

        (vector min_rosenbrock_far, int iter_rosenbrock_far) = Minimiser.newton(rosenbrock, xi_rosenbrock_far);
        (vector min_rosenbrock_far_central, int iter_rosenbrock_far_central) = Minimiser.newton(rosenbrock, xi_rosenbrock_far, central: true);
        min_rosenbrock_far.print("Minimum of Rosenbrock's valley function found at (forward differences):\n");
        min_rosenbrock_far_central.print("Minimum of Rosenbrock's valley function found at (central differences):\n");
        Console.WriteLine($"Number of iterations (forward differences): {iter_rosenbrock_far}");
        Console.WriteLine($"Number of iterations (central differences): {iter_rosenbrock_far_central}");
        Console.WriteLine();

        // Repeating the minimisation for Himmelblau's function with central differences, starting far from the minimum
        vector xi_himmelblau_far = new vector(1e4, 1e4);   // Initial guess far from the minimum
        xi_himmelblau_far.print("Initial guess for Himmelblau's function (far from minimum):\n");

        (vector min_himmelblau_far, int iter_himmelblau_far) = Minimiser.newton(himmelblau, xi_himmelblau_far);
        (vector min_himmelblau_far_central, int iter_himmelblau_far_central) = Minimiser.newton(himmelblau, xi_himmelblau_far, central: true);
        min_himmelblau_far.print("Minimum of Himmelblau's function found at (forward differences):\n");
        min_himmelblau_far_central.print("Minimum of Himmelblau's function found at (central differences):\n");
        Console.WriteLine($"Number of iterations (forward differences): {iter_himmelblau_far}");
        Console.WriteLine($"Number of iterations (central differences): {iter_himmelblau_far_central}");
    }
}