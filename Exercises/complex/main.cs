using static System.Console;
using static System.Math;
using System.Numerics;

class Program{

    public static bool approx(double a, double b, double acc=1e-9, double eps=1e-9){
            if(Abs(b-a) <= acc) return true;
            if(Abs(b-a) <= Max(Abs(a), Abs(b))*eps) return true;
            return false;
        }

    static void Main(){
        
        Complex i = Complex.ImaginaryOne;

        // sqrt(-1)
        Complex a = Complex.Sqrt(-1);
        bool test_a = approx(a.Real, 0) && approx(a.Imaginary, 1);
        WriteLine($"sqrt(-1) = {a}");
        WriteLine($"Real and imaginary parts are correct: {test_a}\n");

        // sqrt(i)
        Complex b = Complex.Sqrt(i);
        bool test_b = approx(b.Real, 1/Sqrt(2)) && approx(b.Imaginary, 1/Sqrt(2));
        WriteLine($"sqrt(i)  = {b}");
        WriteLine($"Real and imaginary parts are correct: {test_b}\n");

        // exp(i)
        Complex c = Complex.Exp(i);
        bool test_c = approx(c.Real, Cos(1)) && approx(c.Imaginary, Sin(1));
        WriteLine($"exp(i)   = {c}");
        WriteLine($"Real and imaginary parts are correct: {test_c}\n");

        // exp(iπ)
        Complex d = Complex.Exp(i * PI);
        bool test_d = approx(d.Real, -1) && approx(d.Imaginary, 0);
        WriteLine($"exp(iπ)  = {d}");
        WriteLine($"Real and imaginary parts are correct: {test_d}\n");

        // i^i
        Complex e = Complex.Pow(i, i);
        bool test_e = approx(e.Real, Exp(-PI/2)) && approx(e.Imaginary, 0);
        WriteLine($"i^i      = {e}");
        WriteLine($"Real and imaginary parts are correct: {test_e}\n");

        // ln(i)
        Complex f = Complex.Log(i);
        bool test_f = approx(f.Real, 0) && approx(f.Imaginary, PI/2);
        WriteLine($"ln(i)    = {f}");
        WriteLine($"Real and imaginary parts are correct: {test_f}\n");

        // sin(iπ)
        Complex g = Complex.Sin(i * PI);
        bool test_g = approx(g.Real, 0) && approx(g.Imaginary, Sinh(PI));
        WriteLine($"sin(iπ)  = {g}");
        WriteLine($"Real and imaginary parts are correct: {test_g}\n");

        // sinh(i)
        Complex h = Complex.Sinh(i);
        bool test_h = approx(h.Real, 0) && approx(h.Imaginary, Sin(1));
        WriteLine($"sinh(i)  = {h}");
        WriteLine($"Real and imaginary parts are correct: {test_h}\n");

        // cosh(i)
        Complex j = Complex.Cosh(i);
        bool test_j = approx(j.Real, Cos(1)) && approx(j.Imaginary, 0);
        WriteLine($"cosh(i)  = {j}");
        WriteLine($"Real and imaginary parts are correct: {test_j}\n");
    }
}