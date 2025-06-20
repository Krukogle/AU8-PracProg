using System;
using System.IO;
using integrator_class;
using minimiser_class;
using ann_class;
using vector_class;

public static class Program
{

    // Linspace method as in NumPy to train the neural network
    public static vector linspace(double xi, double xf, int n_samples)
    {
        vector xs = new vector(n_samples);
        double dx = (xf - xi) / (n_samples - 1);
        for (int i = 0; i < n_samples; i++)
        {
            xs[i] = xi + i * dx;
        }
        return xs;
    }

    // Defining the function g(x) that can take either a double or a vector
    public static double g(double x) => Math.Cos(5 * x - 1) * Math.Exp(-x * x);
    public static vector g(vector xs) => xs.map(g);

    // Defining the function g'(x) that can take either a double or a vector
    public static double dg(double x) => (-5 * Math.Sin(5*x - 1) - 2*x * Math.Cos(5*x - 1)) * Math.Exp(-x * x);
    public static vector dg(vector xs) => xs.map(dg);

    // Defining the function g''(x) that can take either a double or a vector
    public static double d2g(double x) => ((4*x*x - 27) * Math.Cos(5*x - 1) + 20*x * Math.Sin(5*x - 1)) * Math.Exp(-x * x);
    public static vector d2g(vector xs) => xs.map(d2g);

    // Defining the function G(x) that can take either a double or a vector
    public static double G(double x) => Integrator.integrate(g, -1.0, x).Item1;
    public static vector G(vector xs) => xs.map(G);

    public static void Main(string[] args)
    {

        // ---------- PART A ---------- //
        Console.WriteLine("----------- PART A ----------");

        // Training network to approximate the function g(x) = cos(5x-1)exp(-x²) on [-1,1]
        Console.WriteLine("Training a neural network to approximate the function g(x) = cos(5x-1)exp(-x²) on [-1,1]...");

        // Get training data
        int n_points = 500;
        vector xs_train = linspace(-1.0, 1.0, n_points);
        vector ys_train = g(xs_train);

        // Initialize and train ANN
        ANN network = new ANN(3);
        network.train(xs_train, ys_train);
        Console.WriteLine("Training completed.");

        // Export correct and predicted values
        using (StreamWriter writer = new StreamWriter("values.txt"))
        {
            for (int i = 0; i < n_points; i++)
            {
                double x = xs_train[i];
                double y_true = ys_train[i];
                double y_pred = network.response(x);
                writer.WriteLine($"{x} {y_true} {y_pred}");
            }
        }
        Console.WriteLine("Comparison values written to 'values.txt'.");
        Console.WriteLine("The plot 'values.pdf' shows the comparison of the true function and the ANN response.\n");

        // ---------- PART B ---------- //
        Console.WriteLine("----------- PART B ----------");

        // Training on the first derivative of g(x)
        Console.WriteLine("Testing the neural network on the first derivative of g(x)...");
        vector dys = dg(xs_train);
        using (StreamWriter writer = new StreamWriter("derivatives.txt"))
        {
            for (int i = 0; i < n_points; i++)
            {
                double x = xs_train[i];
                double y_true = dys[i];
                double y_pred = network.derivative(x);
                writer.WriteLine($"{x} {y_true} {y_pred}");
            }
        }
        Console.WriteLine("Comparison of derivatives written to 'derivatives.txt'.");
        Console.WriteLine("The plot 'derivatives.pdf' shows the comparison of the true derivative and the ANN response.\n");

        // Testing the second derivative of g(x)
        Console.WriteLine("Testing the neural network on the second derivative of g(x)...");
        vector d2ys = d2g(xs_train);
        using (StreamWriter writer = new StreamWriter("second_derivatives.txt"))
        {
            for (int i = 0; i < n_points; i++)
            {
                double x = xs_train[i];
                double y_true = d2ys[i];
                double y_pred = network.second_derivative(x);
                writer.WriteLine($"{x} {y_true} {y_pred}");
            }
        }
        Console.WriteLine("Comparison of second derivatives written to 'second_derivatives.txt'.");
        Console.WriteLine("The plot 'second_derivatives.pdf' shows the comparison of the true second derivative and the ANN response.\n");

        // Testing the antiderivative of g(x)
        Console.WriteLine("Testing the neural network on the antiderivative of g(x)...");
        vector Gs = G(xs_train);
        using (StreamWriter writer = new StreamWriter("antiderivatives.txt"))
        {
            for (int i = 0; i < n_points; i++)
            {
                double x = xs_train[i];
                double y_true = Gs[i];
                double y_pred = network.antiderivative(x);
                writer.WriteLine($"{x} {y_true} {y_pred}");
            }
        }
        Console.WriteLine("Comparison of antiderivatives written to 'antiderivatives.txt'.");
        Console.WriteLine("The plot 'antiderivatives.pdf' shows the comparison of the true antiderivative and the ANN response.");

    }
}