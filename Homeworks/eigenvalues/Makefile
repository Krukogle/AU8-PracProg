# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt varying_dr.pdf varying_rmax.pdf radial_n1.pdf radial_n2.pdf radial_n3.pdf timing.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of varying dr ----- #
varying_dr.pdf: varying_dr.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel 'Δ{/:Italic r}'; \
		set ylabel 'ε_0'; \
		set title 'Lowest eigenvalue of varying Δ{/:Italic r}'; \
		set grid; \
		plot '$<' using 1:2 with linespoints title 'ε_0 vs Δr'; \
		" 


# ----- Generate plot of varying rmax ----- #
varying_rmax.pdf: varying_rmax.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic r}_{max}'; \
		set ylabel 'ε_0'; \
		set title 'Lowest eigenvalue of varying {/:Italic r}_{max}'; \
		set grid; \
		plot '$<' using 1:2 with linespoints title 'ε_0 vs {/:Italic r}_{max}'; \
	"


# ----- Generate plot of n=1 radial wave function ----- #
radial_n1.pdf: radial_n1.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic r}'; \
		set ylabel 'R_{1,0}'; \
		set title 'Radial wavefunction approximation vs. analytical result for n=1'; \
		set grid; \
		plot '$<' using 1:2 with lines title 'Approximation', \
			'$<' using 1:3 with lines title 'Analytical'; \
	"


# ----- Generate plot of n=2 radial wave function ----- #
radial_n2.pdf: radial_n2.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic r}'; \
		set ylabel 'R_{2,0}'; \
		set title 'Radial wavefunction approximation vs. analytical result for n=2'; \
		set grid; \
		plot '$<' using 1:2 with lines title 'Approximation', \
			'$<' using 1:3 with lines title 'Analytical'; \
	"


# ----- Generate plot of n=3 radial wave function ----- #
radial_n3.pdf: radial_n3.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic r}'; \
		set ylabel 'R_{3,0}'; \
		set title 'Radial wavefunction approximation vs. analytical result for n=3'; \
		set grid; \
		plot '$<' using 1:2 with lines title 'Approximation', \
			'$<' using 1:3 with lines title 'Analytical'; \
	"


# ----- Generate plot of Jacobi factorization time ----- #
timing.pdf: timing.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel 'Matrix size n'; \
		set ylabel 'Time [ms]'; \
		set title 'Jacobi factorization time'; \
		set grid; \
		f(x) = a*x**3; \
		fit f(x) 'timing.txt' using 1:2 via a; \
		plot 'timing.txt' using 1:2 with points pt 7 ps 0.5 lc rgb 'red' title 'Timing data', \
		     'timing.txt' using (\$$1):(f(\$$1)) with lines title sprintf('Fit: %g * n^3', a); \
	"

# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log