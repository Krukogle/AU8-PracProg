using System;
using System.Numerics;
using static System.Math;

class Program{
    public static void Main(){

        // Error function values from -3 to 3 used in the plot
        double dx = 1.0/8;
        for(double x=-3; x<=3; x+=dx){
            Console.WriteLine($"{x} {sfuns.erf(x)}");
        }
        Console.WriteLine();
        Console.WriteLine();

        // Gamma function values from -5 to 5 used in the plot
        dx = 1.0/64;
        for(double x=-5+dx/2; x<=5; x+=dx){
            Console.WriteLine($"{x} {sfuns.Gamma(x)}");
        }
        Console.WriteLine();
        Console.WriteLine();

        // Log Gamma function values from 0 to 5 used in the plot
        for(double x=dx; x<=10; x+=dx){
            Console.WriteLine($"{x} {sfuns.lnGamma(x)}");
        }
        Console.WriteLine();
        Console.WriteLine();

        // Absolute value of the complex Gamma function values from -5 to 5 used in the plot
        dx = 1.0/64;
        for(double x=-5+dx; x<=5; x+=dx){
            for(double y=-5+dx; y<=5; y+=dx){
                Complex z = new Complex(x, y);
                double absGamma = Complex.Abs(sfuns.cGamma(z));
                Console.WriteLine($"{x} {y} {absGamma}");
            }
            Console.WriteLine();
        }
    }
}