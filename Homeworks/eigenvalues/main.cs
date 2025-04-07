using System;
using System.IO;
using vector_class;
using matrix_class;
using jacobi_class;

public class Program{
    public static void Main(){

        // ---------- PART A ---------- //
        Console.WriteLine("---------- PART A ----------");
        // Generate a random symmetric matrix A
        int n = 5;
        matrix A = new matrix(n, n);
        Random rand = new Random();
        for(int i = 0; i < n; i++){
            for(int j = 0; j < n; j++){
                A[i, j] = rand.NextDouble();
            }
        }
        for(int i = 0; i < n; i++){
            for(int j = i + 1; j < n; j++){
                A[j, i] = A[i, j];
            }
        }
        A.print("Matrix A:");
        Console.WriteLine();

        // Extract the orthogonal matrix V of eigenvectors
        matrix V = matrix.id(n);
        vector w = new vector(n);
        (w, V) = Jacobi.cyclic(A);

        // Check V^T * A * V = D
        matrix D_check = V.transpose() * A * V;
        D_check.print("The product V^T * A * V yields:"); 
        Console.WriteLine();

        // Print the eigenvalues and check if they match the diagonal of D
        w.print("Eigenvalue vector w from Jacobi:");
        Console.WriteLine();
        Console.WriteLine("Are the eigenvalues in w equal to the diagonal of D within a tolerance?");
        bool areEqual = true;
        for(int i = 0; i < n; i++){
            if(Math.Abs(D_check[i, i] - w[i]) > 1e-10){
                areEqual = false;
                break;
            }
        }
        Console.WriteLine(areEqual);
        Console.WriteLine();

        // Check that V * D * V^T = A
        matrix A_check = V * D_check * V.transpose();
        A_check.print("The product V * D * V^T yields:");
        Console.WriteLine();
        Console.WriteLine("Is V * D * V^T equal to A within a tolerance?");
        Console.WriteLine(A_check.approx(A));

        // Check that V^T * V = V * V^T = I
        matrix I_check1 = V.transpose() * V;
        matrix I_check2 = V * V.transpose();
        I_check1.print("The product V^T * V yields:");
        Console.WriteLine();
        Console.WriteLine("Is V^T * V equal to I within a tolerance?");
        Console.WriteLine(I_check1.approx(matrix.id(n)));
        Console.WriteLine();

        I_check2.print("The product V * V^T yields:");
        Console.WriteLine();
        Console.WriteLine("Is V * V^T equal to I within a tolerance?");
        Console.WriteLine(I_check2.approx(matrix.id(n)));
        Console.WriteLine();

    }
}