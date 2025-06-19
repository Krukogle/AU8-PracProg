using System;

static class Program{

    static void Main(){

        // Calculate constants
        double root_two = Math.Sqrt(2);
        double fifth_root_of_two = Math.Pow(2.0, 0.2);
        double e_to_the_pi = Math.Pow(Math.E, Math.PI);
        double pi_to_the_e = Math.Pow(Math.PI, Math.E);
        
        // Print constants
        Console.WriteLine($"sqrt(2) = {root_two}");
        Console.WriteLine($"2^(1/5) = {fifth_root_of_two}");
        Console.WriteLine($"e^π = {e_to_the_pi}");
        Console.WriteLine($"π^e = {pi_to_the_e}");
        Console.WriteLine();

        // Calculate gamma function and print each result
        for (int i=1; i<=10; i++){
            double Gamma = sfuns.fgamma(i);
            Console.WriteLine($"Γ({i}) = {Gamma}");
        }
        Console.WriteLine();

        // Calculate log gamma function and print each result
        for (int i=1; i<=10; i++){
            double log_Gamma = sfuns.lnfgamma(i);
            Console.WriteLine($"ln Γ({i}) = {log_Gamma}");
        }
    }
}