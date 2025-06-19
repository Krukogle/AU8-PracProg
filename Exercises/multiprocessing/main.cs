using System;
using System.Threading;
using System.Linq;

// We wish to calculate the harmonic sum sharing the computation load between several processors

public class Program{

    // Subroutine
    public class data{
        public int a, b;
        public double sum;
    }
    public static void harm(object obj){
        var arg = (data)obj;
        arg.sum = 0;
        for(int i=arg.a; i<arg.b; i++){
            arg.sum += 1.0/i;
        }
    }

    // Main. Reads the number of threads to be created and the number of terms in the harmonic sum
    public static void Main(string[] args){
        int nthreads = 1;
        int nterms = (int)1e8;
        foreach(var arg in args){
            var words = arg.Split(':');
            if(words[0] == "-threads") nthreads = int.Parse(words[1]);
            if(words[0] == "-terms") nterms = (int)float.Parse(words[1]);
        }

    // Data-objects to be used locally in separate threads
    data[] param = new data[nthreads];
    for(int i=0; i<nthreads; i++){
        param[i] = new data();
        param[i].a = 1 + nterms/nthreads*i;
        param[i].b = 1 + nterms/nthreads*(i+1);
    }
    param[param.Length-1].b = nterms+1;

    // Prepare threads
    var threads = new System.Threading.Thread[nthreads];
    for(int i=0; i<nthreads; i++){
        threads[i] = new System.Threading.Thread(harm);   // Create a thread
        threads[i].Start(param[i]);   // run it with param[i] as argument to "harm"
    }

    // Join threads
    foreach(var thread in threads) thread.Join();

    // Calculate total sum
    double total=0;
    foreach(var p in param) total += p.sum;

    // Print result
    Console.WriteLine($"Harmonic sum of {nterms} terms and {nthreads} threads is {total}");

    // Now we calculate the harmonic sum using "Parallel.For"
    double sum = 0;
    System.Threading.Tasks.Parallel.For(1, nterms+1, (int i) => {sum += 1.0/i;});
    Console.WriteLine($"Harmonic sum of {nterms} terms using Parallel.For is {sum}");

    // Now we calculate the harmonic sum using LINQ
    var sum_LINQ = new System.Threading.ThreadLocal<double>( ()=>0, trackAllValues:true);
    System.Threading.Tasks.Parallel.For(1, nterms+1, (int i) => {sum_LINQ.Value += 1.0/i;});
    double total_LINQ = sum_LINQ.Values.Sum();
    Console.WriteLine($"Harmonic sum of {nterms} terms using LINQ is {total_LINQ}");

    }
}