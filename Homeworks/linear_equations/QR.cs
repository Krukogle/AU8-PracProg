using System;
using static System.Console;

// ---------- PART A ---------- //
// Implement a static class "QR" with functions "decomp", "solve", and "det".
public static class QR{

    // "decomp": Gram-Schmidt QR decomposition
    public static (matrix,matrix) decomp(matrix A){
        matrix Q = A.copy();
        matrix R = new matrix(A.size2, A.size2);

        for(int i=0; i<A.size2; i++){
            R[i,i] = Q[i].norm();
            Q[i] = Q[i]/R[i,i];
            for(int j=i+1; j<A.size2; j++){
                R[i,j] = Q[i].dot(Q[j]);
                Q[j] = Q[j] - Q[i]*R[i,j];
            }
        }
        return (Q,R);
    }

    // "solve": Solve QRx=b for b given
    public static vector solve(matrix Q, matrix R, vector b){
        // We solve Rx=y, where y=Q^Tb by back-substitution
        vector y = Q.transpose()*b;
        vector x = new vector(R.size1);
        for(int i=R.size1-1; i>=0; i--){
            x[i] = y[i];
            for(int j=i+1; j<R.size1; j++){
                x[i] = x[i] - R[i,j]*x[j];
            }
            x[i] = x[i]/R[i,i];
        }
        return x;
    }

    // "det": Determinant of the matrix R
    public static double det(matrix R){
        double determinant = 1.0;
        for(int i=0; i<R.size1; i++){
            determinant = determinant*R[i,i];
        }
        return determinant;
    }

    // ---------- PART B ---------- //
    // Add the method "inverse" to your "QR" class that returns the inverse of the matrix A.
    public static matrix inverse(matrix A){
        (matrix Q, matrix R) = decomp(A);

        // We first calculate the upper triangular inverse of R
        matrix R_inv = new matrix(Q.size1, Q.size2);
        for(int i=R.size2-1; i>=0; i--){
            // The diagonal entries are just inversed
            R_inv[i,i] = 1.0/R[i,i];

            // The off-diagonal entries require more work
            for(int j=i-1; j>=0; j--){
                double sum=0;
                for(int k=j+1; k<i+1; k++){
                    sum = sum + R[j,k] * R_inv[k,i];
                }
                R_inv[j,i] = -sum/R[j,j];
            }
        }
        return R_inv*Q.transpose();
    }
}