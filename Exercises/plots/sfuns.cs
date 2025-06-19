using System;
using System.Numerics;
using static System.Math;

public static class sfuns{

    // Error function
    public static double erf(double x){
        if(x < 0) return -erf(-x);
        double[] a = {0.254829592, -0.284496736, 1.421413741, -1.453152027, 1.061405429};
        double t = 1/(1 + 0.3275911*x);
        double sum = t*(a[0] + t*(a[1] + t*(a[2] + t*(a[3] + t*a[4]))));
        return 1 - sum*Exp(-x*x);
    }

    // Gamma function
    public static double Gamma(double x){
        if(x < 0) return PI/Sin(PI*x)/Gamma(1-x);
        if(x < 9) return Gamma(x+1)/x;
        double lnGamma = Log(2*PI)/2 + (x-0.5)*Log(x) - x + (1.0/12)/x - (1.0/360)/(x*x*x) + (1.0/1260)/(x*x*x*x*x);
        return Exp(lnGamma);
    }

    // Log Gamma function
    public static double lnGamma(double x){
        if(x <= 0) throw new System.ArgumentException("Argument must be positive", "x");
        if(x < 9) return lnGamma(x+1) - Log(x);
        return x*Log(x+1/(12*x-1/x/10)) - x + Log(2*PI/x)/2;
    }

    // Complex Gamma function
    public static readonly double gamma = 0.5772156649015328606;
    public static Complex cGamma(Complex z){
        // if(z == Complex.Zero) throw new System.ArgumentException("Gamma function is undefined at z = 0.");
        Complex product = Complex.One;
        for(int n=1; n<=100; n++){
            product *= Complex.Pow(1+z/n, -1)*Complex.Exp(z/n);
        }
        return Complex.Exp(-gamma*z)/z*product;
    }

}