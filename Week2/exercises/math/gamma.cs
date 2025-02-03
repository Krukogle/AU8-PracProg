using System;

class GammaTest {
    static void Main() {  // Only one entry point remains!
        Console.WriteLine($"sqrt(2) = {Constants.RootTwo}");
        Console.WriteLine($"2^(1/5) = {Constants.FifthRootOfTwo}");
        Console.WriteLine($"e^π = {Constants.EToThePi}");
        Console.WriteLine($"π^e = {Constants.PiToTheE}");

        for (int x = 1; x <= 10; x++) {
            Console.WriteLine($"Gamma({x}) = {Sfuns.fgamma(x)}");
        }
    }
}
