namespace integrator_class{
using System;

public class Integrator{

    // Recursive open-quadrature adaptive integrator of f(x) on [a,b] with the required absolute or relative goal
    // The integrator has to use a higher order quadrature to estimate the integral and an imbedded lower order quadrature to estimate the local error.
    public static (double, int, double) integrate(
        Func<double, double> f,
        double a,
        double b,
        double acc = 0.001,
        double eps = 0.001,
        double f2 = double.NaN,
        double f3 = double.NaN   // NaN is a placeholder for the first call
    ){
        // Checking for infinite limits
        if(a == double.NegativeInfinity && b == double.PositiveInfinity){
            Func<double, double> f_transformed = (double t) => f(t / (1 - t * t)) * (1 + t * t) / (1 - t * t) / (1 - t * t);
            return integrate_cc(f_transformed, -1.0, 1.0, acc, eps);
        }
        else if(a == double.NegativeInfinity){
            Func<double, double> f_transformed = (double t) => f(b - (1 - t) / t) / t / t;
            return integrate_cc(f_transformed, 0.0, 1.0, acc, eps);
        }
        else if(b == double.PositiveInfinity){
            Func<double, double> f_transformed = (double t) => f(a + (1 - t) / t) / t / t;
            return integrate_cc(f_transformed, 0.0, 1.0, acc, eps);
        }

        int N_calls = 0;
        double h = b - a;
        if(double.IsNaN(f2)){
            f2 = f(a + 2 * h / 6);
            f3 = f(a + 4 * h / 6);
            N_calls += 2;
        }
        double f1 = f(a + h / 6);
        double f4 = f(a + 5 * h / 6);
        N_calls += 2;

        double Q = (2 * f1 + f2 + f3 + 2 * f4) / 6 * h;   // higher order rule
        double q = (f1 + f2 + f3 + f4) / 4 * h;   // lower order rule
        double err = Math.Abs(Q - q);
        if(err <= acc + eps * Math.Abs(Q)){
            return (Q, N_calls, err);
        }
        else{
            var (leftResult, leftCalls, leftErr) = integrate(f, a, (a + b) / 2, acc / Math.Sqrt(2), eps, f1, f2);
            var (rightResult, rightCalls, rightErr) = integrate(f, (a + b) / 2, b, acc / Math.Sqrt(2), eps, f3, f4);
            return (leftResult + rightResult, leftCalls + rightCalls, Math.Sqrt(leftErr * leftErr + rightErr * rightErr));
        }
    }

    // Error function via its integral representation
    public static double erf(double x, double acc = 0.001, double eps = 0.001){
        if(x < 0){
            return -erf(-x, acc, eps);
        }
        else if(0 <= x && x <= 1){
            Func<double, double> integrand = (double t) => Math.Exp(-t * t);
            var integral = integrate(integrand, 0, x, acc, eps);
            return 2 / Math.Sqrt(Math.PI) * integral.Item1;
        }
        else{
            Func<double, double> exponent = (double t) => -(x + (1 - t) / t) * (x + (1 - t) / t);
            Func<double, double> integrand = (double t) => Math.Exp(exponent(t)) / t / t;
            var integral = integrate(integrand, 0, 1, acc, eps);
            return 1 - 2 / Math.Sqrt(Math.PI) * integral.Item1;
        }
    }

    // Adaptive integrator with the Clenshaw–Curtis variable transformation
    public static (double, int, double) integrate_cc(
        Func<double, double> f,
        double a,
        double b,
        double acc = 0.001,
        double eps = 0.001
    ){
        Func<double, double> f_transformed = (double theta) => f(0.5 * (a + b) + 0.5 * (b - a) * Math.Cos(theta));
        Func<double, double> weight = (double theta) => 0.5 * (b - a) * Math.Sin(theta);
        Func<double, double> integrand = (double theta) => f_transformed(theta) * weight(theta);
        return integrate(integrand, 0, Math.PI, acc, eps);
    }

}
}