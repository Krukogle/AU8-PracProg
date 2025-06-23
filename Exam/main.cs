// Practical programming and numerical methods 2025 examination project
// Project 11: Symmetric row/column update of a size-n symmetric eigenvalue problem
// Given the diagonal elements of the matrix D, the vector u, and the integer p, calculate the eigenvalues of the matrix A using O(n^2) operations

using System;
using System.IO;
using vector_class;
using matrix_class;
using root_class;

public static class Program
{

    // Method that sorts a vector in ascending order
    public static void sortVector(vector v)
    {
        int n = v.size;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (v[i] > v[j])
                {
                    double temp = v[i];
                    v[i] = v[j];
                    v[j] = temp;
                }
            }
        }
    }

    // Method that constructs a vector of n random elements from a to b in ascending order
    public static vector randomVector(int n, double a, double b)
    {
        vector v = new vector(n);
        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            v[i] = a + (b - a) * rand.NextDouble();
        }
        // Sort the vector in ascending order
        sortVector(v);
        return v;
    }

    // Method that constructs the matrix D
    public static matrix constructD(vector d)
    {
        int n = d.size;
        matrix D = new matrix(n, n);
        for (int i = 0; i < n; i++)
        {
            D[i, i] = d[i];
        }
        return D;
    }

    // Method that constructs the unit vector e for some p
    public static vector unitVector(int n, int p)
    {
        vector e = new vector(n);
        e[p] = 1.0;
        return e;
    }

    // Method that calculates the outer product of two vectors
    public static matrix outerProduct(vector a, vector b)
    {
        int m = a.size;
        int n = b.size;
        matrix C = new matrix(m, n);
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                C[i, j] = a[i] * b[j];
            }
        }
        return C;
    }

    public static void Main(string[] args)
    {

        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
        Console.WriteLine("          Practical programming and numerical methods 2025 examination project by Jakob Krukow Mogensen");
        Console.WriteLine("          Project 11: Symmetric row/column update of a size-n symmetric eigenvalue problem");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------\n");

        // Description:
        Console.WriteLine("PROJECT DESCRIPTION:");
        Console.WriteLine("Given the diagonal elements of the matrix D, the vector u, and the integer p,");
        Console.WriteLine("calculate the eigenvalues of the matrix A using O(n^2) operations.\n");

        Console.WriteLine("The matrix A is defined as:");
        Console.WriteLine("A = D + e u^T + u e^T, where");
        Console.WriteLine("    D is an (n x n) diagonal matrix,");
        Console.WriteLine("    e is a size-n unit vector in index p,");
        Console.WriteLine("    and u is a size-n vector with the p'th element equal to zero.\n");

        Console.WriteLine("The secular function is defined as:");
        Console.WriteLine("    f(λ) = -(d_p - λ) + ∑_{k ≠ p} u_k^2 / (d_k - λ).");
        Console.WriteLine("The eigenvalues of the matrix A are the roots of the secular function.");
        Console.WriteLine("There are n - 1 poles of the secular function: λ ∈ {d_k} for k ≠ p.");
        Console.WriteLine("Since f(λ) is continuous and strictly increasing between the poles,");
        Console.WriteLine("there must be exactly one root in each interval between the poles (n - 2 roots), as well as one to the left of the leftmost pole and one to the right of the rightmost pole.\n");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------\n");

        // Construct the diagonal matrix D and unit vector e
        var rand = new Random();
        int minSize = 2; // Minimum size of the matrix
        int maxSize = 10; // Maximum size of the matrix
        // int n = rand.Next(minSize, maxSize + 1);
        int n = 3;
        int p = rand.Next(0, n);
        Console.WriteLine($"Random matrix size n ∈ [{minSize}, {maxSize}]:  {n}");
        Console.WriteLine($"Random index p ∈ [0, {n - 1}]:         {p}\n");

        double λmin = -100.0; // Minimum value for the diagonal elements
        double λmax = 100.0; // Maximum value for the diagonal elements

        vector d = randomVector(n, λmin, λmax);
        d.print($"Randomly generated diagonal elements of {n}x{n} matrix D:\n");
        Console.WriteLine();
        matrix D = constructD(d);

        vector e = unitVector(n, p);
        e.print($"Unit vector e for randomly chosen p = {p}:\n");
        Console.WriteLine();

        // Construct the vector u whose p'th element is zero
        vector u = randomVector(n, 0.95*λmin, 0.95*λmax);
        u[p] = 0.0;
        u.print("Random vector u:\n");
        Console.WriteLine();

        // Construct the outer product matrices
        matrix outer1 = outerProduct(e, u);
        matrix outer2 = outerProduct(u, e);
        matrix outer = outer1 + outer2;

        // Construct the updated matrix A
        matrix A = D + outer;
        A.print("Updated matrix A:");
        Console.WriteLine();

        // Secular function
        Func<vector, vector> secular = (vector λ) =>
        {
            double λ_val = λ[0];
            double sum = 0.0;
            for (int k = 0; k < n; k++)
            {
                if (k == p) continue;
                sum += u[k] * u[k] / (d[k] - λ_val);
            }
            return new vector(-(d[p] - λ_val) + sum);
        };

        // Extract the poles for k ≠ p
        vector poles = new vector(n - 1);
        int idx = 0;
        for (int k = 0; k < n; k++)
        {
            if (k == p) continue;
            poles[idx++] = d[k];
        }
        poles.print("Poles of the secular function f(λ):\n");
        Console.WriteLine();

        // Initial guesses for the eigenvalues
        vector λ0s = new vector(n);
        double R = u.norm();   // safe bracket radius
        λ0s[0] = poles[0] - R;   // leftmost interval: (−∞, poles[0])
        for (int i = 1; i < n - 1; i++)
        {
            λ0s[i] = 0.5 * (poles[i] + poles[i - 1]);   // intervals
        }
        λ0s[n - 1] = poles[n - 2] + R;   // rightmost interval: (poles[n - 2], +∞)
        λ0s.print("Initial guesses for the eigenvalues λ0s:\n");
        Console.WriteLine();

        // Find the eigenvalues using Newton's method
        vector λs = new vector(n);
        for (int i = 0; i < n; i++)
        {
            vector λ0 = new vector(λ0s[i]);
            vector root = RootFinder.newton(secular, λ0, acc: 1e-8);
            λs[i] = root[0];
        }
        λs.print("Eigenvalues of the updated matrix A:\n");

        // Export the secular function, eigenvalues, and poles to a file
        using (StreamWriter writer = new StreamWriter("secular.txt"))
        {
            writer.WriteLine("# λ\tSecular Function Value");
            for (double λ = λs[0] - 0.2*Math.Abs(λs[0]); λ <= λs[n - 1] + 0.2*Math.Abs(λs[n - 1]); λ += 0.01)
            {
                vector result = secular(new vector(λ));
                writer.WriteLine($"{λ}\t{result[0]}");
            }
        }
        using (StreamWriter writer = new StreamWriter("eigenvalues.txt"))
        {
            writer.WriteLine("# λ\tf(λ)");
            for (int i = 0; i < n; i++)
            {
                writer.WriteLine($"{λs[i]}\t{0.0}");
            }
        }
        using (StreamWriter writer = new StreamWriter("poles.txt"))
        {
            writer.WriteLine("# x\ty");
            for (int i = 0; i < n - 1; i++)
            {
                writer.WriteLine($"{poles[i]}\t{0.0}");
            }
        }

    }
}