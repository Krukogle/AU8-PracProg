# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt erf.pdf erf1.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of error function comparison ----- #
erf.pdf: erf.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic x}'; \
		set ylabel 'erf({/:Italic x})'; \
		set title 'Plot of error function approximation compared to actual value'; \
		set grid; \
		plot 'erf.txt' using 1:2 with lines title 'Approximation', \
		'erf.txt' every 5 using 1:3 with points pt 7 ps 0.5 lc rgb 'red' title 'Exact'; \
		"


# ----- Generate plot of erf(1) convergence ----- #
erf1.pdf: erf1.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set logscale x; \
		set format x '%.0e'; \
		set xtics rotate by -45; \
		set xlabel 'Tolerance (acc)'; \
		set ylabel 'Δerf(1)'; \
		set title 'Plot of erf(1) error with varying tolerance'; \
		set grid; \
		plot 'erf1.txt' using 1:3 with lines title 'Convergence'; \
		"


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log