using System;
using System.Collections.Generic;
using System.IO;
using vector_class;
using rungekutta_class;

public static class Program{

    // ODE function for the planetary orbit
    public static Func<double, vector, vector> orbital_function(double eps){
        return (double phi, vector u) => {
            vector dudphi = new vector(u.size);
            dudphi[0] = u[1];
            dudphi[1] = 1 - u[0] + eps * u[0] * u[0];
            return dudphi;
        };
    }

    // Main function
    public static void Main(string[] args){

        // ---------- PART A ---------- //
        Console.WriteLine("---------- PART A ----------");

        // Debugging/testing the Runge-Kutta stepper with y'' = -y
        Console.WriteLine("Debugging/testing the Runge-Kutta stepper with y'' = -y for x in [0, 10] ...");

        Func<double, vector, vector> f_debug = (x, y) => new vector(y[1], -y[0]);
        (double xi, double xf) interval = (0.0, 10.0);
        vector yi_debug = new vector(5.0, 0.0);

        var (x_debug, y_debug) = RungeKutta.driver(f_debug, interval, yi_debug);
        using(StreamWriter writer = new StreamWriter("simple_HO.txt")){
            for (int i = 0; i < x_debug.Count; i++){
                double x = x_debug[i];
                double y = y_debug[i][0];
                writer.WriteLine($"{x:F4} {y:F4}");
            }
        }
        Console.WriteLine("Runge-Kutta data written to simple_HO.txt.\n");

        // Reproducing the oscillator with friction
        Console.WriteLine("Reproducing the oscillator with friction, θ''(t) + b*θ'(t) + c*sin(θ(t)) = 0 ...");
        Func<double, vector, vector> f_osc = (double x, vector y) => {
            vector dydx = new vector(y.size);
            double b = 0.25;
            double c = 5.0;
            dydx[0] = y[1];
            dydx[1] = -b * y[1] - c * Math.Sin(y[0]);
            return dydx;
        };

        vector yi_osc = new vector(Math.PI - 0.1, 0.0);
        var (x_osc, y_osc) = RungeKutta.driver(f_osc, interval, yi_osc);
        using(StreamWriter writer = new StreamWriter("damped_HO.txt")){
            for (int i = 0; i < x_osc.Count; i++){
                double x = x_osc[i];
                double y = y_osc[i][0];
                writer.WriteLine($"{x:F4} {y:F4}");
            }
        }
        Console.WriteLine("Runge-Kutta data written to damped_HO.txt.\n");
        Console.WriteLine("The plot 'oscillators.pdf' shows the Runge-Kutta method applied to the simple and damped harmonic oscillator.\n");

        // ---------- PART B ---------- //
        Console.WriteLine("---------- PART B ----------");

        // Applying the Runge-Kutta method to planetary orbits
        Console.WriteLine("Applying the Runge-Kutta method to planetary orbits, i.e. u''(φ) + u(φ) = 1 + εu(φ)^2, for φ in [0, 10π] ...\n");
        (double phii, double phif) interval_orb = (0.0, 10.0 * Math.PI);

        // Newtonian circular motion: ε = 0, u(0) = 1, u'(0) = 0
        Console.WriteLine("Newtonian circular motion: ε = 0, u(0) = 1, u'(0) = 0 ...");
        double eps = 0.0;
        Func<double, vector, vector> f_orb1 = orbital_function(eps);
        vector ui_orb1 = new vector(1.0, 0.0);
        var (phi_orb1, u_orb1) = RungeKutta.driver(f_orb1, interval_orb, ui_orb1, h: 0.001, min_steps: 250);
        using(StreamWriter writer = new StreamWriter("circular.txt")){
            for (int i = 0; i < phi_orb1.Count; i++){
                double phi = phi_orb1[i];
                double u = u_orb1[i][0];
                double x = 1 / u * Math.Cos(phi);
                double y = 1 / u * Math.Sin(phi);
                // writer.WriteLine($"{phi:F4} {u:F4}");
                writer.WriteLine($"{x:F4} {y:F4}");
            }
        }
        Console.WriteLine("Runge-Kutta data written to circular.txt.\n");

        // Newtonian elliptical motion: ε = 0, u(0) = 1, u'(0) = -0.5
        Console.WriteLine("Newtonian elliptical motion: ε = 0, u(0) = 1, u'(0) = -0.5 ...");
        Func<double, vector, vector> f_orb2 = orbital_function(eps);
        vector ui_orb2 = new vector(1.0, -0.5);
        var (phi_orb2, u_orb2) = RungeKutta.driver(f_orb2, interval_orb, ui_orb2);
        using(StreamWriter writer = new StreamWriter("elliptical.txt")){
            for (int i = 0; i < phi_orb2.Count; i++){
                double phi = phi_orb2[i];
                double u = u_orb2[i][0];
                double x = 1 / u * Math.Cos(phi);
                double y = 1 / u * Math.Sin(phi);
                // writer.WriteLine($"{phi:F4} {u:F4}");
                writer.WriteLine($"{x:F4} {y:F4}");
            }
        }
        Console.WriteLine("Runge-Kutta data written to elliptical.txt.\n");

        // Relativistic precession: ε = 0.01, u(0) = 1, u'(0) = -0.5
        Console.WriteLine("Relativistic precession: ε = 0.01, u(0) = 1, u'(0) = -0.5 ...");
        eps = 0.01;
        Func<double, vector, vector> f_orb3 = orbital_function(eps);
        vector ui_orb3 = new vector(1.0, -0.5);
        var (phi_orb3, u_orb3) = RungeKutta.driver(f_orb3, interval_orb, ui_orb3);
        using(StreamWriter writer = new StreamWriter("relativistic.txt")){
            for (int i = 0; i < phi_orb3.Count; i++){
                double phi = phi_orb3[i];
                double u = u_orb3[i][0];
                double x = 1 / u * Math.Cos(phi);
                double y = 1 / u * Math.Sin(phi);
                // writer.WriteLine($"{phi:F4} {u:F4}");
                writer.WriteLine($"{x:F4} {y:F4}");
            }
        }
        Console.WriteLine("Runge-Kutta data written to relativistic.txt.\n");
        Console.WriteLine("The plot 'orbits.pdf' shows the Runge-Kutta method applied to the planetary orbits.");

    }
}