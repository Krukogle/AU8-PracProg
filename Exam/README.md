-----------------------------------------------------------------------------------------------------------------
          Practical programming and numerical methods 2025 examination project by Jakob Krukow Mogensen
          Project 11: Symmetric row/column update of a size-n symmetric eigenvalue problem
-----------------------------------------------------------------------------------------------------------------

PROJECT DESCRIPTION:
Given the diagonal elements of the matrix D, the vector u, and the integer p,
calculate the eigenvalues of the matrix A using O(n²) operations.

The matrix A is defined as:
A = D + e u^T + u e^T, where
    D is an (n x n) diagonal matrix,
    e is a size-n unit vector in index p,
    and u is a size-n vector with the p'th element equal to zero.

The secular function is defined as:
    f(λ) = -(d_p - λ) + ∑_{k ≠ p} u_k^2 / (d_k - λ).
The eigenvalues of the matrix A are the roots of the secular function.
There are n - 1 poles of the secular function: λ ∈ {d_k} for k ≠ p.
Since f(λ) is continuous and strictly increasing between the poles,
there must be exactly one root in each interval between the poles (n - 2 roots), as well as one to the left of the leftmost pole and one to the right of the rightmost pole.

However, when doing this numerically, there is a risk of the eigenvalues being very close to the poles.
This means that the Newton step size easily overshoots into the next interval, and the eigenvalue is not found.
To avoid this, an optional upper and lower bound argument was added to the Newton root-finding method.

Also, the convergence of the root-finder turned out to be quite parameter-sensitive.
Stuff like the step size, the tolerance, the maximum number of iterations, and so on, had to be tweaked a lot.
When pushing this, the whole thing worked ~10 times in a row, producing a nice result at the end, so I hope it stays that way on other machines.

The method constructed and saved in 'eigenupdate.cs' takes the diagonal elements of the matrix D, the vector u, and the integer p as input.
I decided to generate them randomly for the sake of demonstration and to test various different combinations.
The plot timing.pdf clearly shows the O(n²) behavior.
There is nothing left to do from the examination project description, so I'd give this project 10 points.