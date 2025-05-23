using static System.Console;
using static System.Math;

public class Program{
    public static void Main(string[] args){
        string infile = null, outfile = null;
        foreach(var arg in args){
            var words = arg.Split(':');
            if(words[0] == "-input") infile=words[1];
            if(words[0] == "-output") outfile=words[1];
            }
        if(infile == null || outfile == null) {
            Error.WriteLine("Wrong filename argument.");
            }
        var instream = new System.IO.StreamReader(infile);
        var outstream = new System.IO.StreamWriter(outfile, append:false);
        for(string line = instream.ReadLine(); line!=null; line = instream.ReadLine()){
            double x = double.Parse(line);
            outstream.WriteLine($"x = {x}, sin(x) = {Sin(x)}, cos(x) = {Cos(x)}");
                }
        instream.Close();
        outstream.Close();
    }
}