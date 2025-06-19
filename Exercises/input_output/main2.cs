using System;
using static System.Console;
using static System.Math;

public class Program{
    public static void Main(string[] args){
        char[] split_delimeters = {' ', '\t', '\n'};
        var split_options = StringSplitOptions.RemoveEmptyEntries;
        for(string line = ReadLine(); line != null; line = ReadLine()){
            var numbers = line.Split(split_delimeters, split_options);
            foreach(var number in numbers){
                double x = double.Parse(number);
                Error.WriteLine($"x = {x}, sin(x) = {Sin(x)}, cos(x) = {Cos(x)}");
            }
        }
    }
}