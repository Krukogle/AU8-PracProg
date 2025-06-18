namespace root_class{
using System;
using vector_class;
using matrix_class;
using qr_class;

// Root finding class
public static class RootFinder
{

    // Jacobian function
    public static matrix jacobian(Func<vector, vector> f, vector x, vector fx = null, vector dx = null)
    {
        if (dx == null) dx = x.map(xi => Math.Abs(xi) * Math.Pow(2, -26));
        if (fx == null) fx = f(x);
        matrix J = new matrix(x.size);
        for (int j = 0; j < x.size; j++)
        {
            x[j] += dx[j];
            vector df = f(x) - fx;
            for (int i = 0; i < x.size; i++)
            {
                J[i, j] = df[i] / dx[j];
            }
            x[j] -= dx[j];
        }
        return J;
    }

    // Newton's method with numerical Jacobian and back-tracking line-search
        public static vector newton(Func<vector, vector> f, vector start, double acc = 1e-2, vector δx = null)
    {
        vector x = start.copy();
        vector fx = f(x);
        if (δx == null) δx = x.map(xi => Math.Abs(xi) * Math.Pow(2, -26));

        vector z = null;
        vector fz = null;
        vector step;

        double λmin = 1.0 / 128;
        int max_iter = 100;
        int iter = 0;

        do {
            if (fx.norm() < acc) break;
            if (iter++ >= max_iter) break;

            matrix J = jacobian(f, x, fx, δx);
            (matrix Q, matrix R) = QR.decomp(J);
            vector Dx = QR.solve(Q, R, -fx);

            double λ = 1.0;
            do {
                z = x + λ * Dx;
                step = z - x;
                fz = f(z);

                if (step.norm() < δx.norm()) break;
                if (fz.norm() < (1 - λ / 2) * fx.norm()) break;
                if (λ < λmin) break;

                λ /= 2;
            } while (true);

            x = z;
            fx = fz;
        } while (true);

        return x;
    }


}
}