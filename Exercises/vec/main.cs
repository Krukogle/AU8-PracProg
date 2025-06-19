using static System.Console;
using static System.Math;

static class Program{

    public static void print(this double x, string s=""){
        Write(s);
        WriteLine(x);
    }

    static void Main(){
        
        // Create two random vectors
        var rnd = new System.Random();
        var u = new vec(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());
        var v = new vec(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());

        // Print the vectors
        u.print("u = ");
        v.print("v = ");
        WriteLine($"u = {u}");
        WriteLine($"v = {v}");
        WriteLine();

        // Define t as -u and check if it is correct
        vec t;
        t = new vec(-u.x, -u.y, -u.z);
        (-u).print("-u = ");
        t.print("t = ");
        if(vec.approx(t, -u)) WriteLine("Test 't = -u' passed");
        else WriteLine("Test 't = -u' failed");
        WriteLine();

        // Define t as u-v and check if it is correct
        t = new vec(u.x-v.x, u.y-v.y, u.z-v.z);
        (u-v).print("u-v = ");
        t.print("t = ");
        if(vec.approx(t, u-v)) WriteLine("Test 't = u-v' passed");
        else WriteLine("Test 't = u-v' failed");
        WriteLine();

        // Define t as u+v and check if it is correct
        t = new vec(u.x+v.x, u.y+v.y, u.z+v.z);
        (u+v).print("u+v = ");
        t.print("t = ");
        if(vec.approx(t, u+v)) WriteLine("Test 't = u+v' passed");
        else WriteLine("Test 't = u+v' failed");
        WriteLine();

        // Define t as u*c and check if it is correct
        double c = rnd.NextDouble();
        t = new vec(c*u.x, c*u.y, c*u.z);
        var tmp = c*u;
        tmp.print("c*u = ");
        t.print("t = ");
        if(vec.approx(t, c*u)) WriteLine("Test 't = c*u' passed");
        else WriteLine("Test 't = c*u' failed");
        WriteLine();

        // Define d as the dot product of u and v and check if it is correct
        double d1 = u.x*v.x + u.y*v.y + u.z*v.z;
        double d2 = u.dot(v);
        d1.print("u % v = ");
        d2.print("d = ");
        if(vec.approx(d1, d2)) WriteLine("Test 'd = u % v' passed");
        else WriteLine("Test 'd = u % v' failed");

    }
}