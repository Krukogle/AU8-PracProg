namespace spline_class{
using System;
using vector_class;

public class Spline{

    public static int binsearch(vector x, double z){
        int n = x.size;
        int L = 0;
        int R = n - 1;
        if(z < x[0] || z > x[n - 1]){
            throw new ArgumentOutOfRangeException("z is out of range");
        }
        while(R - L > 1){
            int M = (L + R) / 2;
            if(z > x[M]){
                L = M;
            }
            else{
                R = M;
            }
        }
        return L;
    }

    // ----- Linear spline ----- //
    public class LinearSpline{

        // Initialization of the spline
        vector x, y;
        public LinearSpline(vector xs, vector ys){
            if(xs.size != ys.size){
                throw new ArgumentException("x and y must have the same size");
            }
            this.x = xs.copy();
            this.y = ys.copy();
        }

        // Interpolation of a point z
        public double linterp(double z){
            int i = binsearch(x, z);
            double dx = x[i + 1] - x[i];
            double dy = y[i + 1] - y[i];
            return y[i] + dy / dx * (z - x[i]);
        }

        // Integral from x[0] to a point z
        public double linterpInteg(double z){
            int i = binsearch(x, z);
            double area = 0;
            double dx, dy;
            vector p = new vector(x.size);

            // Area contribution up until index i
            for(int j = 0; j < i; j++){
                dx = x[j + 1] - x[j];
                dy = y[j + 1] - y[j];
                p[j] = dy / dx;
                area += y[j] * dx + p[j] * dx * dx / 2;
            }

            // Area contribution from index i
            dx = x[i + 1] - x[i];
            dy = y[i + 1] - y[i];
            p[i] = dy / dx;
            area += y[i] * (z - x[i]) + p[i] * (z - x[i]) * (z - x[i]) / 2;
            
            return area;
        }
    }

    // ----- Quadratic spline ----- //
    public class QuadraticSpline{

        // Initialization of the spline
        vector x, y, b, c;
        public QuadraticSpline(vector xs, vector ys){
            if(xs.size != ys.size){
                throw new ArgumentException("x and y must have the same size");
            }
            this.x = xs.copy();
            this.y = ys.copy();
            int n = x.size;
            b = new vector(n - 1);
            c = new vector(n - 1);
            vector h = new vector(n - 1);   // Interval lengths
            vector p = new vector(n - 1);   // Interval slopes
            vector cF = new vector(n - 1);    // c vector for forwards recursion
            vector cB = new vector(n - 1);    // c vector for backwards recursion

            // Compute h and p vector
            for(int i = 0; i < n - 1; i++){
                h[i] = x[i + 1] - x[i];
                p[i] = (y[i + 1] - y[i]) / h[i];
            }

            // Forwards recursion
            cF[0] = 0;
            for(int i = 1; i < n - 1; i++){
                cF[i] = 1/h[i] * (p[i] - p[i - 1] - cF[i - 1] * h[i - 1]);
            }

            // Backwards recursion
            cB[n - 2] = 0;
            for(int i = n - 3; i >= 0; i--){
                cB[i] = 1/h[i] * (p[i + 1] - p[i] - cB[i + 1] * h[i + 1]);
            }
            
            // Average c results to get the final c vector. Also, compute b vector
            for(int i = 0; i < n - 1; i++){
                c[i] = (cF[i] + cB[i]) / 2;
                b[i] = p[i] - c[i] * h[i];
            }
        }

        // Evaluate the spline at a point z
        public double evaluate(double z){
            int i = binsearch(x, z);
            double h = z - x[i];
            return y[i] + b[i] * h + c[i] * h * h;
        }

        // Derivative of the spline at a point z
        public double derivative(double z){
            int i = binsearch(x, z);
            double h = z - x[i];
            return b[i] + 2 * c[i] * h;
        }

        // Integral of the spline from x[0] to a point z
        public double integral(double z){

            // Area contribution up until index i
            int i = binsearch(x, z);
            double area = 0;
            double h;
            for(int j = 0; j < i; j++){
                h = x[j + 1] - x[j];
                area += y[j] * h + b[j] * h * h / 2 + c[j] * h * h * h / 3;
            }

            // Area contribution from index i
            h = z - x[i];
            area += y[i] * h + b[i] * h * h / 2 + c[i] * h * h * h / 3;

            return area;
        }
    }

    // ----- Cubic spline ----- //
    public class CubicSpline{

        // Initialization of the spline
        vector x, y, b, c, d;
        public CubicSpline(vector xs, vector ys){
            if(xs.size != ys.size){
                throw new ArgumentException("x and y must have the same size");
            }
            this.x = xs.copy();
            this.y = ys.copy();
            int n = x.size;
            b = new vector(n);
            c = new vector(n - 1);
            d = new vector(n - 1);
            vector h = new vector(n - 1);   // Interval lengths
            vector p = new vector(n - 1);   // Interval slopes

            for(int i = 0; i < n - 1; i++){
                h[i] = x[i + 1] - x[i];
                p[i] = (y[i + 1] - y[i]) / h[i];
            }

            // Build the triangular system to solve via Gaussian elimination
            vector D = new vector(n);
            vector Q = new vector(n - 1);
            vector B = new vector(n);

            // Fill the D, Q, and B vectors
            D[0] = 2.0;
            D[n - 1] = 2.0;
            Q[0] = 1;
            B[0] = 3 * p[0];
            B[n - 1] = 3 * p[n - 2];

            for(int i = 0; i < n - 2; i++){
                D[i + 1] = 2 * h[i] / h[i + 1] + 2;
                Q[i + 1] = h[i] / h[i + 1];
                B[i + 1] = 3 * (p[i] + p[i + 1] * Q[i + 1]);
            }

            // Solve the system using Gaussian elimination
            vector Dtilde = new vector(n);
            vector Btilde = new vector(n);

            Dtilde[0] = D[0];
            Btilde[0] = B[0];

            for(int i = 0; i < n - 1; i++){
                Dtilde[i + 1] = D[i + 1] - Q[i] / Dtilde[i];
                Btilde[i + 1] = B[i + 1] - Btilde[i] / Dtilde[i];
            }

            // Get the b vector by back substitution
            b[n - 1] = Btilde[n - 1] / Dtilde[n - 1];
            for(int i = n - 2; i >= 0; i--){
                b[i] = (Btilde[i] - Q[i] * b[i + 1]) / Dtilde[i];
            }

            // Knowing b, we can compute c and d
            for(int i = 0; i < n - 1; i++){
                c[i] = 1 / h[i] * (-2 * b[i] - b[i + 1] + 3 * p[i]);
                d[i] = 1 / h[i] / h[i] * (b[i] + b[i + 1] - 2 * p[i]);
            }
        }

        // Evaluate the spline at a point z
        public double evaluate(double z){
            int i = binsearch(x, z);
            double h = z - x[i];
            return y[i] + b[i] * h + c[i] * h * h + d[i] * h * h * h;
        }

        // Derivative of the spline at a point z
        public double derivative(double z){
            int i = binsearch(x, z);
            double h = z - x[i];
            return b[i] + 2 * c[i] * h + 3 * d[i] * h * h;
        }

        // Integral of the spline from x[0] to a point z
        public double integral(double z){
            int i = binsearch(x, z);
            double area = 0;
            double h;
            for(int j = 0; j < i; j++){
                h = x[j + 1] - x[j];
                area += y[j] * h + b[j] * h * h / 2.0 + c[j] * h * h * h / 3.0 + d[j] * h * h * h * h / 4.0;
            }
            h = z - x[i];
            area += y[i] * h + b[i] * h * h / 2.0 + c[i] * h * h * h / 3.0 + d[i] * h * h * h * h / 4.0;
            return area;
        }
    }

}
}