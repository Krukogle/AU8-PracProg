// Practical programming and numerical methods 2025 examination project
// Project 11: Symmetric row/column update of a size-n symmetric eigenvalue problem
// Given the diagonal elements of the matrix D, the vector u, and the integer p, calculate the eigenvalues of the matrix A using O(n²) operations

namespace eigenupdate_class
{
    using System;
    using vector_class;
    using matrix_class;
    using root_class;

    public static class EigenUpdate
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

        public static (matrix, Func<vector, vector>, vector, vector, vector, vector, vector) eigenvalueUpdate(
            vector d, vector u, int p, double dmin = -100.0, double dmax = 100.0, bool print = false
        )
        {

            // Construct the diagonal matrix D and unit vector e
            int n = d.size;
            matrix D = constructD(d);
            vector e = unitVector(n, p);
            if (print) {e.print($"Unit vector e for randomly chosen p = {p}:\n"); Console.WriteLine();}

            // Construct the outer product matrices
            matrix outer1 = outerProduct(e, u);
            matrix outer2 = outerProduct(u, e);
            matrix outer = outer1 + outer2;

            // Construct the updated matrix A
            matrix A = D + outer;

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
            if (print) {poles.print("Poles of the secular function f(λ):\n"); Console.WriteLine();}

            // Define the bounds of all n intervals
            double R = 2 * u.norm();   // safe bracket radius
            vector bounds = new vector(n + 1);
            bounds[0] = poles[0] - R;   // leftmost interval: (−∞, poles[0])
            for (int i = 1; i < n; i++)
            {
                bounds[i] = poles[i - 1];   // intervals
            }
            bounds[n] = poles[n - 2] + R;   // rightmost interval: (poles[n - 1], +∞)
            if (print) {bounds.print("Bounds of the intervals for the eigenvalues:\n"); Console.WriteLine();}

            // Initial guesses for the eigenvalues
            vector λ0s = new vector(n);
            for (int i = 0; i < n; i++)
            {
                λ0s[i] = 0.5 * (bounds[i] + bounds[i + 1]);   // midpoints of the intervals
            }
            if (print) {λ0s.print("Initial guesses for the eigenvalues λ0s:\n"); Console.WriteLine();}

            // Find the eigenvalues using Newton's method
            vector λs = new vector(n);
            for (int i = 0; i < n; i++)
            {
                vector λ0 = new vector(λ0s[i]);
                vector root = RootFinder.newton(
                    secular,
                    λ0,
                    acc: 1e-12,   // absolute tolerance
                    max_iter: (int)1e4,
                    lower_bound: new vector(bounds[i]),   // lower bound of the interval
                    upper_bound: new vector(bounds[i + 1])   // upper bound of the interval
                );
                λs[i] = root[0];
            }
            if (print) {λs.print("Eigenvalues of the updated matrix A:\n"); Console.WriteLine();}

            return (A, secular, e, poles, bounds, λ0s, λs);
        }
        
    }
}