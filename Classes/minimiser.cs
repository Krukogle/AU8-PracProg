namespace minimiser_class
{
    using System;
    using vector_class;
    using matrix_class;
    using qr_class;

    // Minimisation class
    public static class Minimiser
    {

        // Numerical gradients of objective function φ(x)
        public static vector gradient(Func<vector, double> φ, vector x)
        {
            int n = x.size;
            double φx = φ(x);
            vector gφ = new vector(n);
            for (int i = 0; i < n; i++)
            {
                double dxi = (1 + Math.Abs(x[i])) * Math.Pow(2, -26);
                x[i] += dxi;
                gφ[i] = (φ(x) - φx) / dxi;
                x[i] -= dxi;
            }
            return gφ;
        }
        public static vector gradient_central(Func<vector, double> φ, vector x)
        {
            int n = x.size;
            double φx = φ(x);
            vector gφ = new vector(n);
            for (int i = 0; i < n; i++)
            {
                double dxi = (1 + Math.Abs(x[i])) * Math.Pow(2, -26);
                x[i] += dxi;
                double φp = φ(x);
                x[i] -= 2 * dxi;
                double φm = φ(x);
                gφ[i] = (φp - φm) / (2 * dxi);
                x[i] += dxi;
            }
            return gφ;
        }

        // Numerical Hessian matrices of objective function φ(x)
        public static matrix hessian(Func<vector, double> φ, vector x)
        {
            int n = x.size;
            matrix H = new matrix(n, n);
            vector gφ = gradient(φ, x);
            for (int j = 0; j < n; j++)
            {
                double dxj = (1 + Math.Abs(x[j])) * Math.Pow(2, -13);
                x[j] += dxj;
                vector dgφ = gradient(φ, x) - gφ;
                for (int i = 0; i < n; i++)
                {
                    H[i, j] = dgφ[i] / dxj;
                }
                x[j] -= dxj;
            }
            return H;
        }
        public static matrix hessian_central(Func<vector, double> φ, vector x)
        {
            int n = x.size;
            matrix H = new matrix(n, n);
            vector gφ = gradient(φ, x);
            for (int j = 0; j < n; j++)
            {
                double dxj = (1 + Math.Abs(x[j])) * Math.Pow(2, -13);
                x[j] += dxj;
                vector dgφp = gradient(φ, x) - gφ;
                x[j] -= 2 * dxj;
                vector dgφm = gradient(φ, x) - gφ;
                for (int i = 0; i < n; i++)
                {
                    H[i, j] = (dgφp[i] - dgφm[i]) / (2 * dxj);
                }
                x[j] += dxj;
            }
            return H;
        }

        // Newton's method for minimisation with numerical gradient, Hessian matrix and back-tracking line-search
        public static (vector, int) newton(Func<vector, double> φ, vector x, double acc = 1e-3, int max_iter = 1000, bool central = false)
        {
            int iter = 0;
            for (iter = 0; iter < max_iter; iter++)
            {
                vector g = central
                    ? gradient_central(φ, x)
                    : gradient(φ, x);
                if (g.norm() < acc) return (x, iter);   // job done
                matrix H = central
                    ? hessian_central(φ, x)
                    : hessian(φ, x);
                (matrix Q, matrix R) = QR.decomp(H);
                vector dx = QR.solve(Q, R, -g);
                double λ = 1.0;
                while (λ >= 1 / 1024.0)
                {
                    if (φ(x + λ * dx) < φ(x)) break;   // good step
                    λ /= 2.0;   // backtrack
                }
                x += λ * dx;
            }
            throw new Exception($"Newton's method did not converge within {max_iter} iterations.");
        }

    }
}