namespace ols_class{
using System;
using vector_class;
using matrix_class;
using qr_class;

public static class OLS{

    public static (vector, matrix) lsfit(Func<double, double>[] fs, vector x, vector y, vector dy){
        
        // Create the design matrix A and vector b
        matrix A = new matrix(x.size, fs.Length);
        vector b = new vector(x.size);

        for(int i = 0; i < x.size; i++){
            for(int j = 0; j < fs.Length; j++){
                A[i,j] = fs[j](x[i]) / dy[i];
            }
            b[i] = y[i] / dy[i];
        }

        // Solve the OLS system using QR factorization
        (matrix Q, matrix R) = QR.decomp(A);
        vector c = QR.solve(Q, R, b);

        // Calculate the covariance matrix
        matrix R_inv = QR.inverse(R);
        matrix cov = R_inv * R_inv.transpose();

        return (c, cov);
    }

}
}