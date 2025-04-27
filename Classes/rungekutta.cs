namespace rungekutta_class{
using System;
using System.Collections.Generic;
using vector_class;

public static class RungeKutta{

    // Runge-Kutta stepper - method one-two
    public static (vector, vector) rkstep12(
        Func<double, vector, vector> f,
        double x,
        vector y,
        double h
    ){
        vector k0 = f(x, y);
        vector k1 = f(x + h / 2, y + k0 * h / 2.0);
        vector yh = y + k1 * h;
        vector δy = (k1 - k0) * h;
        return (yh, δy);
    }

    // Adaptive step size driver routine
    public static (List<double>, List<vector>) driver(
        Func<double, vector, vector> F,
        (double, double) interval,
        vector yinit,
        double h = 0.125,
        double acc = 0.01,
        double eps = 0.01,
        int min_steps = 25
    ){
        var (a, b) = interval;
        double x = a;
        vector y = yinit.copy();
        var xlist = new List<double>();
        xlist.Add(x);
        var ylist = new List<vector>();
        ylist.Add(y);
        double hmax = (b - a) / min_steps;
        do{
            if(x >= b) return (xlist, ylist);
            if(x + h > b) h = b - x;
            var (yh, δy) = rkstep12(F, x, y, h);
            double tol = (acc + eps * yh.norm()) * Math.Sqrt(h / (b - a));
            double err = δy.norm();
            if(err <= tol){
                x += h;
                y = yh;
                xlist.Add(x);
                ylist.Add(y);
            }
            if(h < hmax){
                if(err > 0.0){
                    h *= Math.Min(Math.Pow(tol / err, 0.25) * 0.95, 2.0);
                }
                else h *= 2.0;
            }
            else{
                h = hmax;
            }
        } while(true);
    }

}
}