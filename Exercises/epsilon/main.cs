using System;

static class main{

    public static bool approx(double a, double b, double acc=1e-9, double eps=1e-9){
            if(Math.Abs(b-a) <= acc) return true;
            if(Math.Abs(b-a) <= Math.Max(Math.Abs(a),Math.Abs(b))*eps) return true;
            return false;
        }

    static void Main(){
        
        // Exercise 1: Maximum/minimum representable integers
        int i_max = 0;
        while(1+i_max>i_max) {i_max++;}
        Console.WriteLine($"Max representable int = {i_max}");
        Console.WriteLine($"Built-in max int      = {int.MaxValue}\n");

        int i_min = 0;
        while(i_min-1 < i_min) {i_min--;}
        Console.WriteLine($"Min representable int = {i_min}");
        Console.WriteLine($"Built-in min int      = {int.MinValue}\n");

        // Exercise 2: Machine epsilon
        double x = 1;
        while(1+x != 1) {x /= 2;}
        x *= 2;
        Console.WriteLine($"Machine epsilon (double) = {x}");
        Console.WriteLine($"Logical epsilon 2^(-52)  = {Math.Pow(2,-52)}\n");

        float y = 1F;
        while((float)(1F+y) != 1F) {y /= 2;}
        y*=2F;
        Console.WriteLine($"Machine epsilon (float)  = {y}");
        Console.WriteLine($"Logical epsilon 2^(-23)  = {Math.Pow(2,-23)}\n");

        // Exercise 3: Comparison of tiny numbers
        double epsilon = Math.Pow(2,-52);
        double tiny = epsilon/2;
        double a = 1+tiny+tiny;
        double b = tiny+tiny+1;

        Console.WriteLine($"The constants defined are: \n - ε = {epsilon}, \n - ε/2 = {tiny}, \n - a = 1+ε/2+ε/2 = {a}, \n - b = ε/2+ε/2+1 = {b}. \n");
        Console.WriteLine($"Is a = b? {a==b} \nIs a > 1? {a>1} \nIs b > 1? {b>1 }\n");
        Console.WriteLine("The result is due to the order of operations. Since ε is the smallest representable double, 1+ε/2 is rounded to 1 each time.");
        Console.WriteLine("On the other hand, ε/2+ε/2=ε, which is the smallest representable number. Therefore, adding it to 1 gives something larger than 1. \n");

        // Exercise 4: Comparing doubles
        double d1 = 0.1+0.1+0.1+0.1+0.1+0.1+0.1+0.1;
        double d2 = 8*0.1;

        Console.WriteLine($"The constants defined are: \n - d1 = 0.1+0.1+0.1+0.1+0.1+0.1+0.1+0.1 = {d1:e15}, \n - d2 = 8*0.1 = {d2:e15}. \n");
        Console.WriteLine($"Is d1 = d2? {d1==d2}");
        Console.WriteLine($"Is d1 = d2 approximately? {approx(d1,d2)}");

    }
}