using System;


public class vec{
    // The three components of the vector
    public double x, y, z;

    // Default constructor: sets all components to zero
    public vec() { x = y = z = 0; }

    // Parameterized constructor: sets the components to the given values
    public vec(double x, double y, double z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // Overload the scalar multiplication operator
    public static vec operator *(vec v, double c) {
        return new vec(c*v.x, c*v.y, c*v.z);
    }
    public static vec operator *(double c, vec v) {
        return v*c;
    }

    // Overload the division operator
    public static vec operator /(vec v, double c) {
        return new vec(v.x/c, v.y/c, v.z/c);
    }
    public static vec operator /(double c, vec v) {
        return v/c;
    }

    // Overload the addition operator
    public static vec operator +(vec u, vec v) {
        return new vec(u.x+v.x, u.y+v.y, u.z+v.z);
    }

    // Overload the subtraction operator
    public static vec operator -(vec v) {
        return new vec(-v.x, -v.y, -v.z);
    }
    public static vec operator -(vec u, vec v) {
        return u + (-v);
    }

    // Dot product of two vectors being called in two different ways
    public double dot(vec other) {   // to be called as u.dot(v)
        return this.x*other.x + this.y*other.y + this.z*other.z;
    }
    public static double dot(vec u, vec v) {   // to be called as vec.dot(u, v)
        return u.x*v.x + u.y*v.y + u.z*v.z;
    }

    // Aproximate equality of two vectors
    public static bool approx(double a, double b, double acc=1e-9, double eps=1e-9) {
        if(Math.Abs(a-b) < acc) return true;
        if(Math.Abs(a-b) < eps*(Math.Abs(a) + Math.Abs(b))) return true;
        return false;
    }
    public bool approx(vec other) {
        if(!approx(this.x, other.x)) return false;
        if(!approx(this.y, other.y)) return false;
        if(!approx(this.z, other.z)) return false;
        return true;
    }
    public static bool approx(vec u, vec v) {
        return u.approx(v);
    }

    // Overload the ToString method
    public override string ToString() {
        return $"({x}, {y}, {z})";
    }

    // Print method for debugging
    public void print(string s="") {
        Console.Write(s);
        Console.WriteLine($"({x}, {y}, {z})");
    }
}