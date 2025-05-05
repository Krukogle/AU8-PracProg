namespace integrator_class{
using System;

public class Integrator{

    // Recursive open-quadrature adaptive integrator of f(x) on [a,b] with the required absolute or relative goal
    // The integrator has to use a higher order quadrature to estimate the integral and an imbedded lower order quadrature to estimate the local error.
    public static double integrate(
        Func<double, double> f,
        double a,
        double b,
        double acc = 0.001,
        double eps = 0.001,
        double f2 = double.NaN,
        double f3 = double.NaN   // NaN is a placeholder for the first call
    ){
        double h = b - a;
        if(double.IsNaN(f2)){
            f2 = f(a + 2 * h / 6);
            f3 = f(a + 4 * h / 6);
        }
        double f1 = f(a + h / 6);
        double f4 = f(a + 5 * h / 6);
        double Q = (2 * f1 + f2 + f3 + 2 * f4) / 6 * h;   // higher order rule
        double q = (f1 + f2 + f3 + f4) / 4 * h;   // lower order rule
        double err = Math.Abs(Q - q);
        if(err <= acc + eps * Math.Abs(Q)){
            return Q;
        }
        else{
            return integrate(f, a, (a + b) / 2, acc / Math.Sqrt(2), eps, f1, f2) +
                   integrate(f, (a + b) / 2, b, acc / Math.Sqrt(2), eps, f3, f4);
        }
    }

    // Error function via its integral representation
    public static double erf(double x, double acc = 0.001, double eps = 0.001){
        if(x < 0){
            return -erf(-x, acc, eps);
        }
        else if(0 <= x && x <= 1){
            Func<double, double> integrand = (double t) => Math.Exp(-t * t);
            return 2 / Math.Sqrt(Math.PI) * integrate(integrand, 0, x, acc, eps);
        }
        else{
            Func<double, double> exponent = (double t) => -(x + (1 - t) / t) * (x + (1 - t) / t);
            Func<double, double> integrand = (double t) => Math.Exp(exponent(t)) / t / t;
            return 1 - 2 / Math.Sqrt(Math.PI) * integrate(integrand, 0, 1, acc, eps);
        }
    }

}
}