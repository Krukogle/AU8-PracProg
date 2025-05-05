import numpy as np
from scipy.integrate import quad

interval = (0, 1)
acc = 0.001
eps = 0.001
f1 = lambda x: 1/np.sqrt(x)
f2 = lambda x: np.log(x)/np.sqrt(x)
funcs = [f1, f2]
integral_strings = ["∫ 1/√(x) dx", "∫ ln(x)/√(x) dx"]

for func, integral_string in zip(funcs, integral_strings):
    result, error, header = quad(func, *interval, epsabs=acc, epsrel=eps, full_output=1)
    N_calls = header['neval']
    print(f"    {integral_string} = {result:.3f} (SciPy)")
    print(f"      - Number of calls: {N_calls}\n")