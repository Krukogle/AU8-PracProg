namespace jacobi_class{
using System;
using vector_class;
using matrix_class;

public static class Jacobi{
    
    // Method that multiplies A with the Jacobi matrix from the right
    public static void timesJ(matrix A, int p, int q, double theta){
        double c = Math.Cos(theta);
        double s = Math.Sin(theta);
        for(int i=0; i<A.size1; i++){
            double a_ip = A[i, p];
            double a_iq = A[i, q];
            A[i, p] = c * a_ip - s * a_iq;
            A[i, q] = s * a_ip + c * a_iq;
        }
    }

    // Method that multiplies A with the Jacobi matrix from the left
    public static void Jtimes(matrix A, int p, int q, double theta){
        double c = Math.Cos(theta);
        double s = Math.Sin(theta);
        for(int j=0; j<A.size1; j++){
            double a_pj = A[p, j];
            double a_qj = A[q, j];
            A[p, j] = c * a_pj + s * a_qj;
            A[q, j] = -s * a_pj + c * a_qj;
        }
    }

    // Method that implements the Jacobi eigenvalue algorithm for real symmetric matrices
    public static (vector, matrix) cyclic(matrix M){
        matrix A = M.copy();
        int n = A.size1;
        matrix V = matrix.id(n);
        vector w = new vector(n);

        bool changed;
        do{
            changed = false;
            for(int p=0; p<n-1; p++)
            for(int q=p+1; q<n; q++){
                double a_pq = A[p, q];
                double a_pp = A[p, p];
                double a_qq = A[q, q];
                double theta = 0.5 * Math.Atan2(2 * a_pq, a_qq - a_pp);
                double c = Math.Cos(theta);
                double s = Math.Sin(theta);
                double a_pp_new = c * c * a_pp - 2 * s * c * a_pq + s * s * a_qq;
                double a_qq_new = s * s * a_pp + 2 * s * c * a_pq + c * c * a_qq;

                // Do rotation if the change is significant
                if(a_pp_new != a_pp || a_qq_new != a_qq){
                    changed = true;
                    timesJ(A, p, q, theta);
                    Jtimes(A, p, q, -theta);
                    timesJ(V, p, q, theta);
                }
            }
        } while(changed);

        // Extract eigenvalues and eigenvectors
        for(int i=0; i<n; i++){
            w[i] = A[i, i];
        }
        return (w, V);
    }

}
}