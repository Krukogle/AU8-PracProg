import numpy as np
from scipy.integrate import quad

intervals = [(-np.inf, np.inf), (0, np.inf)]
acc = 0.001
eps = 0.001
f1 = lambda x: np.exp(-x**2)
f2 = lambda x: (1 + x**2)**(-1)
funcs = [f1, f2]
integral_strings = ["∫ exp(-x²) dx", "∫ 1/(1 + x²) dx"]

for func, interval, integral_string in zip(funcs, intervals, integral_strings):
    result, error, header = quad(func, *interval, epsabs=acc, epsrel=eps, full_output=1)
    N_calls = header['neval']
    print(f"    {integral_string} = {result:.3f} (SciPy)")
    print(f"      - Number of calls: {N_calls}\n")