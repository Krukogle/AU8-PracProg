using System;
using System.IO;
using System.Collections.Generic;
using vector_class;
using root_class;
using rungekutta_class;

public static class Program{

    // Gradient calculators of Rosenbrock's valley function and Himmelblau's function
    public static vector rosenbrock_grad(vector v){
        double x = v[0];
        double y = v[1];
        return new vector(
            -2 * (1 - x) - 400 * (y - x * x) * x,
            200 * (y - x * x)
        );
    }
    public static vector himmelblau_grad(vector v){
        double x = v[0];
        double y = v[1];
        return new vector(
            2 * (x * x + y - 11) * 2 * x + 2 * (x + y * y - 7),
            2 * (x * x + y - 11) + 2 * (x + y * y - 7) * 2 * y
        );
    }
    
    // Method to calculate the left-hand side of the differential equation
    public static vector ode_system(double r, double E, vector y){
        vector dydr = new vector(
            y[1],
            -2 * (E + 1 / r) * y[0]
        );
        return dydr;
    }

    // Auxillary function to find the energy E for the hydrogen atom
    public static vector M(vector y_hydrogen, double rmin, double rmax, double h, double acc, double eps)
    {
        double E = y_hydrogen[0];
        vector y0 = new vector(rmin - rmin * rmin, 1.0 - 2.0 * rmin);
        Func<double, vector, vector> F = (r, y) => ode_system(r, E, y);

        var (xlist, ylist) = RungeKutta.driver(F, (rmin, rmax), y0, h, acc, eps);
        vector y_end = ylist[ylist.Count - 1];
        return new vector(new double[] { y_end[0] }); // f(rmax)
    }

    public static void Main(string[] args)
    {

        // ---------- PART A ---------- //
        Console.WriteLine("---------- PART A ----------");
        Console.WriteLine("Newton's method with numerical Jacobian and back-tracking line-search\n");

        // ----- Debugging root-finding routine with some simple one- and two-dimensional functions ----- //
        Console.WriteLine("----- Debugging root-finding routine with some simple one- and two-dimensional functions -----");

        // 1. One-dimensional function: f(x) = x² - 2
        Console.WriteLine("    f(x) = x² - 2 (roots at x = ±√2)");

        Func<vector, vector> f_debug1 = (vector x) => new vector(x[0] * x[0] - 2);
        vector xi_debug1 = new vector(-1.0, 1.0);   // initial guesses
        vector roots_debug1 = new vector();
        for (int i = 0; i < xi_debug1.size; i++)
        {
            vector x_debug1 = new vector(xi_debug1[i]);
            vector x_sol_debug1 = RootFinder.newton(f_debug1, x_debug1);
            x_sol_debug1.print("    Root found at x = ");
            roots_debug1.append(x_sol_debug1[0]);
        }
        bool is_root_debug1 = vector.approx(roots_debug1[0], -Math.Sqrt(2), eps: 0.01)
                           && vector.approx(roots_debug1[1], Math.Sqrt(2), eps: 0.01);
        Console.WriteLine($"    Are these correct within a tolerance of 1%? {is_root_debug1}\n");

        // 2. Two-dimensional function: f(x,y) = x² + y² - 3
        Console.WriteLine("    f(x,y) = x² + y² - 3 (roots at x = ±√3, y = 0)");
        Func<vector, vector> f_debug2 = (vector x) => new vector(x[0] * x[0] + x[1] * x[1] - 3, x[1]);
        vector xi_debug2 = new vector(-2.0, 1.0, 2.0, 1.0);
        vector roots_debug2 = new vector();

        for (int i = 0; i < xi_debug2.size; i += 2)
        {
            vector x_debug2 = new vector(xi_debug2[i], xi_debug2[i + 1]);
            vector x_sol_debug2 = RootFinder.newton(f_debug2, x_debug2);
            x_sol_debug2.print("    Roots found at (x, y) = ");
            roots_debug2.append(x_sol_debug2);
        }
        bool is_root_debug2 = vector.approx(roots_debug2[0], -Math.Sqrt(3), eps: 0.01)
                           && vector.approx(roots_debug2[1], 0, eps: 0.01)
                           && vector.approx(roots_debug2[2], Math.Sqrt(3), eps: 0.01)
                           && vector.approx(roots_debug2[3], 0, eps: 0.01);
        Console.WriteLine($"    Are these correct within a tolerance of 1%? {is_root_debug2}\n");

        // ----- Extremum(s) of provided functions ----- //
        Console.WriteLine("----- Moving on to extremum(s) of provided functions -----");

        // Rosenbrock's valley function f(x,y) = (1-x)² + 100*(y-x²)²
        Console.WriteLine("    The Rosenbrock's valley function f(x,y) = (1-x)² + 100*(y-x²)²");
        Console.WriteLine("    According to Wikipedia, the global minimum is at (1, 1²) = (1, 1)");
        Func<vector, vector> rosenbrock = (vector x) => rosenbrock_grad(x);
        vector xi_rosenbrock = new vector(0.1, 0.1);
        vector extremum_rosenbrock = RootFinder.newton(rosenbrock, xi_rosenbrock);
        extremum_rosenbrock.print("    Extremum found at (x,y) = ");
        bool is_extremum_rosenbrock = vector.approx(extremum_rosenbrock[0], 1, eps: 0.01)
                                    && vector.approx(extremum_rosenbrock[1], 1, eps: 0.01);
        Console.WriteLine($"    Is this correct within a tolerance of 1%? {is_extremum_rosenbrock}\n");

        // Himmelblau's function f(x,y) = (x² + y - 11)² + (x + y² - 7)²
        Console.WriteLine("    The Himmelblau's function f(x,y) = (x² + y - 11)² + (x + y² - 7)²");
        Console.WriteLine("    According to Wikipedia, it has one local maximum and four local minima");

        // Maximum (x, y) = (-0.270845, -0.923039)
        Console.WriteLine("        1. Local maximum at (x, y) = (-0.270845, -0.923039)");
        Func<vector, vector> himmelblau = (vector x) => himmelblau_grad(x);
        vector xi_himmelblau_maximum = new vector(0.1, 0.1);
        vector extremum_himmelblau = RootFinder.newton(himmelblau, xi_himmelblau_maximum);
        extremum_himmelblau.print("        Extremum found at (x, y) = ");
        bool is_extremum_himmelblau = vector.approx(extremum_himmelblau[0], -0.270845, eps: 0.01)
                                    && vector.approx(extremum_himmelblau[1], -0.923039, eps: 0.01);
        Console.WriteLine($"        Is this correct within a tolerance of 1%? {is_extremum_himmelblau}\n");

        // Minima (x, y) = (3.0, 2.0), (-2.805118, 3.131312), (-3.779310, -3.283186), (3.584428, -1.848126)
        var himmelblau_minima = new vector[4] {   // Actual minima
            new vector(3.0, 2.0),
            new vector(-2.805118, 3.131312),
            new vector(-3.779310, -3.283186),
            new vector(3.584428, -1.848126)
        };
        var xi_himmelblau_minima = new vector[4] {   // Initial guesses for minima
            new vector(4.0, 4.0),
            new vector(-4.0, 4.0),
            new vector(-4.0, -4.0),
            new vector(4.0, -4.0)
        };
        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"        {i + 2}. Local minimum at (x, y) = ({himmelblau_minima[i][0]}, {himmelblau_minima[i][1]})");
            vector x_himmelblau_minima = xi_himmelblau_minima[i];
            vector extremum_himmelblau_minima = RootFinder.newton(himmelblau, x_himmelblau_minima);
            extremum_himmelblau_minima.print($"        Extremum found at (x, y) = ");
            bool is_extremum_himmelblau_minima = vector.approx(extremum_himmelblau_minima[0], himmelblau_minima[i][0], eps: 0.01)
                                               && vector.approx(extremum_himmelblau_minima[1], himmelblau_minima[i][1], eps: 0.01);
            Console.WriteLine($"        Is this correct within a tolerance of 1%? {is_extremum_himmelblau_minima}\n");
        }

        // ---------- PART B ---------- //
        Console.WriteLine("---------- PART B ----------");
        Console.WriteLine("Bound states of hydrogen atom with shooting method for boundary value problems.");
        Console.WriteLine("We have -(1/2)f'' -(1/r)f = Ef with f(rₘᵢₙ)=rₘᵢₙ-rₘᵢₙ² and f(rₘₐₓ)=0");
        Console.WriteLine("The solution with a given energy is F_E(r). We find this function with the Runge-Kutta method");
        Console.WriteLine("Defining y[0] = f and y[1] = f', we have y[0]' = y[1] and y[1]' = -2 * (E + 1 / r) * y[0]\n");

        // Parameters
        double rmin = 0.001;
        double rmax = 8.0;
        double h = 0.001;
        double acc = 1e-6;
        double eps = 1e-6;

        // Initial guess for the energy E
        vector Einit = new vector(-0.5);

        // Solution to be found numerically
        Func<vector, vector> M_func = (vector y) => M(y, rmin, rmax, h, acc, eps);

        // Finding the energy E
        Console.WriteLine("Finding the energy E for the hydrogen atom with rmax = 8.0...");
        vector E_root_vec = RootFinder.newton(M_func, Einit, 1e-6);
        double E_root = E_root_vec[0];
        Console.WriteLine($"Energy found: E = {E_root}");
        Console.WriteLine("The correct result is E = -0.5");
        Console.WriteLine($"Is this correct within a tolerance of 1%? {vector.approx(E_root, -0.5, eps: 0.01)}\n");

        // Moving on to convergence studies
        Console.WriteLine("----- Moving on to convergence studies -----");

        // Convergence of rmax
        Console.WriteLine("Checking convergence of rmax...");
        using (var writer = new StreamWriter("convergence_rmax.txt"))
        {
            writer.WriteLine("rmax\tE");
            for (double rmax_test = 1.0; rmax_test <= 10.0; rmax_test += 0.5)
            {
                Func<vector, vector> M_func_rmax = (vector y) => M(y, rmin, rmax_test, h, acc, eps);
                vector E_root_rmax = RootFinder.newton(M_func_rmax, Einit, 1e-6);
                writer.WriteLine($"{rmax_test}\t{E_root_rmax[0]}");
            }
        }
        Console.WriteLine("Convergence data for rmax saved to 'convergence_rmax.txt'. The plot 'convergence_rmax.pdf' shows the results.\n");

        // Convergence of rmin
        Console.WriteLine("Checking convergence of rmin...");
        using (var writer = new StreamWriter("convergence_rmin.txt"))
        {
            writer.WriteLine("rmin\tE");
            for (double rmin_test = 1e-10; rmin_test <= 1.1; rmin_test *= 10.0)
            {
                Func<vector, vector> M_func_rmin = (vector y) => M(y, rmin_test, rmax, h, acc, eps);
                vector E_root_rmin = RootFinder.newton(M_func_rmin, Einit, 1e-6);
                writer.WriteLine($"{rmin_test}\t{E_root_rmin[0]}");
            }
        }
        Console.WriteLine("Convergence data for rmin saved to 'convergence_rmin.txt'. The plot 'convergence_rmin.pdf' shows the results.\n");

        // Convergence of acc
        Console.WriteLine("Checking convergence of acc (absolute accuracy of ODE solver)...");
        using (var writer = new StreamWriter("convergence_acc.txt"))
        {
            writer.WriteLine("acc\tE");
            for (double acc_test = 1e-10; acc_test <= 100.1; acc_test *= 10.0)
            {
                Func<vector, vector> M_func_acc = (vector y) => M(y, rmin, rmax, h, acc_test, eps);
                vector E_root_acc = RootFinder.newton(M_func_acc, Einit, 1e-6);
                writer.WriteLine($"{acc_test}\t{E_root_acc[0]}");
            }
        }
        Console.WriteLine("Convergence data for acc saved to 'convergence_acc.txt'. The plot 'convergence_acc.pdf' shows the results.\n");

        // Convergence of eps
        Console.WriteLine("Checking convergence of eps (relative accuracy of ODE solver)...");
        using (var writer = new StreamWriter("convergence_eps.txt"))
        {
            writer.WriteLine("eps\tE");
            for (double eps_test = 1e-10; eps_test <= 100.0; eps_test *= 10.0)
            {
                Func<vector, vector> M_func_eps = (vector y) => M(y, rmin, rmax, h, acc, eps_test);
                vector E_root_eps = RootFinder.newton(M_func_eps, Einit, 1e-6);
                writer.WriteLine($"{eps_test}\t{E_root_eps[0]}");
            }
        }
        Console.WriteLine("Convergence data for eps saved to 'convergence_eps.txt'. The plot 'convergence_eps.pdf' shows the results.\n");

    }
}