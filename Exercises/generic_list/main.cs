using System;
using static System.Console;

// Class definition
class Program{

    // Main method, taking an array of strings "args" as argument
    public static void Main(string[] args){
        
        // Create an instance of the generic list "list" holding doubles
        var list = new genlist<double[]>();

        // Specify delimiters and options for splitting input lines
        char[] delimeters = {' ', '\t'};
        var options = StringSplitOptions.RemoveEmptyEntries;

        // Iterate over each line in the input file
        for (string line = ReadLine(); line != null; line = ReadLine()){
            // Declare an array "words" with the numbers in "line" as strings, split by "delimeters" and "options"
            var words = line.Split(delimeters, options);
            // Get the number of elements in the array "words", n
            int n = words.Length;
            // Create an empty array "numbers" of size n to store the parsed numbers
            var numbers = new double[n];
            // Iterate over each element in "words", convert it to a double, and store it in "numbers"
            for(int i=0; i<n; i++){
                numbers[i] = double.Parse(words[i]);
                // Save the numbers in the list
            }
            list.add(numbers);
        }

        // Print the table to the console
        for (int i=0; i<list.size; i++){
            var numbers = list[i];
            foreach (var number in numbers){
                Write($"{number: 0.00e+00; -0.00e+00}");
            }
            WriteLine();
        }
    }
}