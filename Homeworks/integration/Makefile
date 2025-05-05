# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt erf.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot error function comparison ----- #
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


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log