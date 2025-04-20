using System;
using System.IO;
using vector_class;
using static spline_class.Spline;

public class Program{

    public static void Main(string[] args){

        // ---------- PART A ---------- //
        Console.WriteLine("---------- Part A ----------");

        // Linear interpolation and integration of cos(x) from 0 to 10
        Console.WriteLine("Doing linear interpolation and integration of cos(x) from 0 to 10 ...");

        vector x = new vector(11);
        vector y = new vector(11);
        for(int i = 0; i <= 10; i++){
            x[i] = i;
            y[i] = Math.Cos(i);
        }

        // Interpolation and integration
        // Create a linear spline object
        LinearSpline linearSpline = new LinearSpline(x, y);
        using(StreamWriter writer = new StreamWriter("linear.txt")){
            for(double z = x[0]; z <= x[x.size - 1]; z += 0.1){
                double y_interp = linearSpline.linterp(z);
                double area = linearSpline.linterpInteg(z);
                writer.WriteLine($"{z:F2} {y_interp:F6} {area:F6}");
            }
        }

        Console.WriteLine("Linear interpolation data written to linear.txt.");
        Console.WriteLine("The plot 'linear.pdf' shows the linear spline interpolation and integration of cos(x) from 0 to 10.\n");

        // ---------- PART B ---------- //
        Console.WriteLine("---------- Part B ----------");

        // Quadratic interpolation, derivative, and integration of sin(x) from 0 to 10
        Console.WriteLine("Doing quadratic interpolation, derivative, and integration of sin(x) from 0 to 10 ...");

        x = new vector(11);
        y = new vector(11);
        for(int i = 0; i <= 10; i++){
            x[i] = i;
            y[i] = Math.Sin(i);
        }

        // Create a quadratic spline object
        QuadraticSpline quadraticSpline = new QuadraticSpline(x, y);
        using(StreamWriter writer = new StreamWriter("quadratic.txt")){
            for(double z = x[0]; z <= x[x.size - 1]; z += 0.1){
                double y_interp = quadraticSpline.evaluate(z);
                double y_derivative = quadraticSpline.derivative(z);
                double area = quadraticSpline.integral(z);
                writer.WriteLine($"{z:F2} {y_interp:F6} {y_derivative:F6} {area:F6}");
            }
        }

        Console.WriteLine("Quadratic interpolation data written to quadratic.txt.");
        Console.WriteLine("The plot 'quadratic.pdf' shows the quadratic spline interpolation, derivative, and integration of sin(x) from 0 to 10.\n");

        // ---------- PART C ---------- //
        Console.WriteLine("---------- Part C ----------");

        // Cubic interpolation, derivative, and integration of sin(x) from 0 to 10
        Console.WriteLine("Doing cubic interpolation, derivative, and integration of sin(x) from 0 to 10 ...");

        x = new vector(11);
        y = new vector(11);
        for(int i = 0; i <= 10; i++){
            x[i] = i;
            y[i] = Math.Sin(i);
        }

        // Create a cubic spline object
        CubicSpline cubicSpline = new CubicSpline(x, y);
        using(StreamWriter writer = new StreamWriter("cubic.txt")){
            for(double z = x[0]; z <= x[x.size - 1]; z += 0.1){
                double y_interp = cubicSpline.evaluate(z);
                double y_derivative = cubicSpline.derivative(z);
                double area = cubicSpline.integral(z);
                writer.WriteLine($"{z:F2} {y_interp:F6} {y_derivative:F6} {area:F6}");
            }
        }
        Console.WriteLine("Cubic interpolation data written to cubic.txt.");
        Console.WriteLine("The plot 'cubic.pdf' shows the cubic spline interpolation, derivative, and integration of sin(x) from 0 to 10.\n");

        // Compare my cubic spline with the built-in cubic spline from gnuplot
        Console.WriteLine("Comparing my cubic spline with the built-in cubic spline from gnuplot ...");

        using(StreamWriter writer = new StreamWriter("cubic_compare.txt")){
            for(double z = x[0]; z <= x[x.size - 1]; z += 0.1){
                double y_actual = Math.Sin(z);
                double y_interp = cubicSpline.evaluate(z);
                writer.WriteLine($"{z:F2} {y_actual:F6} {y_interp:F6}");
            }
        }
        Console.WriteLine("Cubic spline comparison data written to cubic_compare.txt.");
        Console.WriteLine("The plot 'cubic_compare.pdf' shows the comparison of my cubic spline with the built-in cubic spline from gnuplot.");

    }
}