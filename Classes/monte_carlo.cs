namespace monte_carlo_class{
using System;
using System.Collections.Generic;
using vector_class;

// Helper class for Halton sequence generation
public class Halton{

    // Initialization of the Halton sequence
    List<int> bases;
    public Halton(int dim){
        bases = prime_numbers(dim);
    }

    // Routine that generates a list of prime numbers
    public static List<int> prime_numbers(int n){
        List<int> primes = new List<int>();
        int candidate = 2;
        while(primes.Count < n){
            bool candidate_is_prime = true;
            foreach(int p in primes){
                if(p * p > candidate) break;
                if(candidate % p == 0){
                    candidate_is_prime = false;
                    break;
                }
            }
            if(candidate_is_prime){
                primes.Add(candidate);
            }
            candidate++;
        }
        return primes;
    }

    // Routine that generates a Van der Corput sequence
    public static double corput(int n, int b = 2){
        double q = 0.0;
        double bk = 1.0 / b;
        while(n > 0){
            q += (n % b) * bk;
            n /= b;
            bk /= b;
        }
        return q;
    }

    // Routine that generates a Halton sequence
    public vector get(int n){
        int dim = bases.Count;
        vector x = new vector(dim);
        for(int i = 0; i < dim; i++){
            x[i] = corput(n, bases[i]);
        }
        return x;
    }

}


// Class for Monte Carlo integration methods
public class MonteCarlo{

    // Plain Monte Carlo integration method
    public static (double, double) plainmc(Func<vector,double> f, vector a, vector b, int N){
        
        // Defining the hypercube
        int dim = a.size;
        double V = 1.0;
        for(int i = 0; i < dim; i++){
            V *= b[i] - a[i];
        }

        // Sampling points in the hypercube
        double sum = 0.0;
        double sum2 = 0.0;
        var x = new vector(dim);
        var rnd = new Random();
        for(int i = 0; i < N; i++){
            for(int j = 0; j < dim; j++){
                x[j] = a[j] + rnd.NextDouble() * (b[j] - a[j]);
            }
            double fx = f(x);
            sum += fx;
            sum2 += fx * fx;
        }

        // Calculate mean and standard deviation
        double mean = sum / N;
        double sigma = Math.Sqrt((sum2 / N - mean * mean));
        var result = (mean * V, sigma * V / Math.Sqrt(N));
        return result;
    }

    // Multidimensional Monte-Carlo integrator that uses low-discrepancy (quasi-random) sequences
    public static (double, double) quasimc(Func<vector,double> f, vector a, vector b, int N){

        // Defining the hypercube
        int dim = a.size;
        double V = 1.0;
        for(int i = 0; i < dim; i++){
			V *= b[i] - a[i];
		}

        // Sampling points in the hypercube using Halton sequences
        Halton h1 = new Halton(dim);
        Halton h2 = new Halton(dim);
        double sum = 0.0;
        double sum2 = 0.0;
        for(int i = 0; i < N; i++){
            vector x1 = h1.get(i);
            vector x2 = h2.get(i);
            for(int j = 0; j < dim; j++){
                x1[j] = a[j] + x1[j] * (b[j] - a[j]);
                x2[j] = a[j] + x2[j] * (b[j] - a[j]);
            }
            double fx1 = f(x1);
            double fx2 = f(x2);
            sum += fx1 + fx2;
            sum2 += fx1 * fx1 + fx2 * fx2;
        }

        // Calculate mean and standard deviation
        double mean = sum / (2.0 * N);
        double sigma = Math.Sqrt((sum2 / (2.0 * N) - mean * mean));
        var result = (mean * V, sigma * V / Math.Sqrt(2.0 * N));
        return result;
    }

    // Implementation of recursive stratified sampling
    // obtained by asking ChatGPT to convert the provided C code to C# :))
    public static (double, double) stratifiedmc(
        Func<vector,double> f, vector a, vector b,
        double acc = 0.001, double eps = 0.001, int nReuse = 0, double meanReuse = 0.0){

            // number of new samples
            int dim = a.size;
            int N = 16 * dim;

            // volume of the hypercube
            double V = 1.0;
            for (int k = 0; k < dim; k++)
                V *= (b[k] - a[k]);

            // storage for sample counts and partial means
            var nLeft   = new int[dim];
            var nRight  = new int[dim];
            vector meanLeft  = new vector(dim);
            vector meanRight = new vector(dim);
            vector x       = new vector(dim);

            double mean = 0.0;

            // draw N samples
            var rnd = new Random();
            for (int i = 0; i < N; i++)
            {
                // random point in the box
                for (int k = 0; k < dim; k++)
                    x[k] = a[k] + rnd.NextDouble() * (b[k] - a[k]);

                // evaluate f
                double fx = f(x);
                mean += fx;

                // assign to left/right stratifiedmc
                for (int k = 0; k < dim; k++)
                {
                    double midpoint = 0.5 * (a[k] + b[k]);
                    if (x[k] > midpoint)
                    {
                        nRight[k]++;
                        meanRight[k] += fx;
                    }
                    else
                    {
                        nLeft[k]++;
                        meanLeft[k] += fx;
                    }
                }
            }

            // finalize mean of new samples
            mean /= N;
            for (int k = 0; k < dim; k++)
            {
                if (nLeft[k]  > 0) meanLeft[k]  /= nLeft[k];
                if (nRight[k] > 0) meanRight[k] /= nRight[k];
            }

            // find best dimension to split (max variance)
            int    kDiv   = 0;
            double maxVar = 0.0;
            for (int k = 0; k < dim; k++)
            {
                double var = Math.Abs(meanRight[k] - meanLeft[k]);
                if (var > maxVar)
                {
                    maxVar = var;
                    kDiv   = k;
                }
            }

            // combined mean (reuse old samples + new ones)
            double integ = (mean * N + meanReuse * nReuse) / (N + nReuse) * V;
            double error = Math.Abs(meanReuse - mean) * V;
            double tol   = acc + Math.Abs(integ) * eps;

            // if within tolerance, return
            if (error < tol)
                return (integ, error);

            // otherwise subdivide along dimension kDiv
            vector a2 = a.copy();
            vector b2 = b.copy();
            // split the box at midpoint in dim kDiv
            double mid = 0.5 * (a[kDiv] + b[kDiv]);
            b2[kDiv] = mid;
            a2[kDiv] = mid;

            // recursive calls with tighter tolerances
            (double integLeft, double errorLeft)   = stratifiedmc(f, a,  b2, acc/Math.Sqrt(2), eps, nLeft[kDiv],  meanLeft[kDiv]);
            (double integRight, double errorRight) = stratifiedmc(f, a2, b,  acc/Math.Sqrt(2), eps, nRight[kDiv], meanRight[kDiv]);

            return (integLeft + integRight, Math.Sqrt(errorLeft * errorLeft + errorRight * errorRight));

        }

}
}