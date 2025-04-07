using System;
using System.IO;
using System.Diagnostics;

// Need a class to interact with the matrix library
public static class MatrixExtensions{

    // Print matrix content instead of just "matrix"
    public static string ToFormattedString(this matrix A) {
        using(var sw = new StringWriter()){
            A.print("", "{0,10:g3} ", sw);
            return sw.ToString();
        }
    }

    // Check if two matrices are equal within a tolerance
    public static bool IsEqual(this matrix A, matrix B) {
        if (A.size1 != B.size1 || A.size2 != B.size2) return false;
        for (int i = 0; i < A.size1; i++) {
            for (int j = 0; j < A.size2; j++) {
                if (Math.Abs(A[i, j] - B[i, j]) > 1e-10) return false;
            }
        }
        return true;
    }

    // Identity matrix
    public static matrix Identity(int size) {
        matrix I = new matrix(size, size);
        for (int i = 0; i < size; i++) {
            I[i, i] = 1.0;
        }
        return I;
    }
}

// Need a class to interact with the vector library
public static class VectorExtensions{

    // Print vector content instead of just "vector"
    public static string ToFormattedString(this vector v) {
        using(var sw = new StringWriter()){
            v.print("", "{0,10:g3} ", sw);
            return sw.ToString();
        }
    }

    // Check if two vectors are equal within a tolerance
    public static bool IsEqual(this vector v1, vector v2) {
        if (v1.size != v2.size) return false;
        for (int i = 0; i < v1.size; i++) {
            if (Math.Abs(v1[i] - v2[i]) > 1e-10) return false;
        }
        return true;
    }
}

public class Program{
    public static void Main(){

        // ---------- PART A ---------- //
        Console.WriteLine("---------- PART A ----------");

        // ----- Check "decomp" ----- //
        Console.WriteLine("----- Check \"decomp\" -----");

        // Generate a random tall matrix A (m x n)
        var rnd = new System.Random(1);
        int m = 7, n = 4;
        matrix A = new matrix(m, n);
        for(int i=0; i<m; i++){
            for(int j=0; j<n; j++){
                A[i,j] = rnd.NextDouble();
            }
        }
        Console.WriteLine($"Random matrix A ({m}x{n}):");
        Console.WriteLine(A.ToFormattedString());

        // Factorize it into QR
        (matrix Q, matrix R) = QR.decomp(A);

        // Check that R is upper triangular
        Console.WriteLine("QR decomposition yielded:");
        Console.WriteLine("Matrix Q:");
        Console.WriteLine(Q.ToFormattedString());
        Console.WriteLine("Matrix R:");
        Console.WriteLine(R.ToFormattedString());

        // Check that Q is orthogonal
        matrix I_check = Q.transpose() * Q;
        Console.WriteLine("The product Q^T * Q yields:");
        Console.WriteLine(I_check.ToFormattedString());
        Console.WriteLine("Is Q^T * Q equal to the identity matrix within a tolerance?");
        Console.WriteLine(I_check.IsEqual(MatrixExtensions.Identity(n)));
        Console.WriteLine("");

        // Check that A = QR
        matrix A_check = Q * R;
        Console.WriteLine("Checking if A = QR. Multiplying Q and R yields:");
        Console.WriteLine(A_check.ToFormattedString());
        Console.WriteLine("Is A - QR = 0 within a tolerance?");
        matrix O_check = A - Q*R;
        Console.WriteLine(O_check.IsEqual(new matrix(m, n)));
        Console.WriteLine("");

        // ----- Check "solve" ----- //
        Console.WriteLine("----- Check \"solve\" -----");

        // Generate a random square matrix A
        A = new matrix(n, n);
        for(int i=0; i<n; i++){
            for(int j=0; j<n; j++){
                A[i,j] = rnd.NextDouble();
            }
        }
        Console.WriteLine($"Random square matrix A ({n}x{n}):");
        Console.WriteLine(A.ToFormattedString());

        // Generate a random vector b
        vector b = new vector(n);
        for(int i=0; i<n; i++){
            b[i] = rnd.NextDouble();
        }
        Console.WriteLine($"Random vector b ({n}):");
        Console.WriteLine(b.ToFormattedString());

        // Factorize A into QR
        (Q, R) = QR.decomp(A);

        // Solve for x in QRx = b
        vector x = QR.solve(Q, R, b);
        Console.WriteLine("Solving QRx = b yields the following x:");
        Console.WriteLine(x.ToFormattedString());

        // Check that Ax = b
        vector b_check = A * x;
        Console.WriteLine("Checking if Ax = b. Multiplying A and x yields:");
        Console.WriteLine(b_check.ToFormattedString());
        Console.WriteLine("Is A*x - b = 0 within a tolerance?");
        vector O_check2 = b_check - b;
        Console.WriteLine(O_check2.IsEqual(new vector(n)));
        Console.WriteLine("");

        // ---------- PART B ---------- //
        Console.WriteLine("---------- PART B ----------");

        // ----- Check "inverse" ----- //
        Console.WriteLine("----- Check \"inverse\" -----");

        // Generate a random square matrix A
        A = new matrix(n, n);
        for(int i=0; i<n; i++){
            for(int j=0; j<n; j++){
                A[i,j] = rnd.NextDouble();
            }
        }
        Console.WriteLine($"Random square matrix A ({n}x{n}):");
        Console.WriteLine(A.ToFormattedString());

        // Calculate the inverse of A using the QR method
        matrix B = QR.inverse(A);
        Console.WriteLine("Calculated inverse of A using QR method:");
        Console.WriteLine(B.ToFormattedString());

        // Check that A * B = I
        matrix I_check2 = A * B;
        Console.WriteLine("Checking if A * B = I. Multiplying A and B yields:");
        Console.WriteLine(I_check2.ToFormattedString());
        Console.WriteLine("Is A*B - I = 0 within a tolerance?");
        matrix O_check3 = I_check2 - MatrixExtensions.Identity(n);
        Console.WriteLine(O_check3.IsEqual(new matrix(n, n)));
        Console.WriteLine("");

        // ---------- PART C ---------- //
        Console.WriteLine("---------- PART C ----------");
        // ----- Time the QR decomposition of varying size square matrices and export the results to a .data file ----- //
        for(int N=5; N<=250; N+=5){
            matrix A_timing = new matrix(N, N);
            for(int i=0; i<N; i++){
                for(int j=0; j<N; j++){
                    A_timing[i,j] = rnd.NextDouble();
                }
            }

            // Start the stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Perform QR decomposition
            (matrix Q_timing, matrix R_timing) = QR.decomp(A_timing);

            // Stop the stopwatch
            stopwatch.Stop();

            // Write the size and time to a .data file
            using(var sw = new StreamWriter("timing.txt", true)){
                sw.WriteLine($"{N} {stopwatch.Elapsed.TotalMilliseconds}");
            }
        }
        Console.WriteLine("Timing data written to timing.txt");

    }
}