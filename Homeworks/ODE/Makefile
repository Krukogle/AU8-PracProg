# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt oscillators.pdf orbits.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of simple and damped harmonic oscillators ----- #
oscillators.pdf: simple_HO.txt damped_HO.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic x}'; \
		set ylabel '{/:Italic y}'; \
		set title 'Plot of simple and damped harmonic oscillators, solved by Runge-Kutta'; \
		set grid; \
		plot 'simple_HO.txt' using 1:2 with lines title 'Simple', \
		'damped_HO.txt' using 1:2 with lines title 'Damped'; \
		"


# ----- Generate plot of Newtonian circular motion, elliptical motion, and motion with relativistic precession ----- #
orbits.pdf: circular.txt elliptical.txt relativistic.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic x}'; \
		set ylabel '{/:Italic y}'; \
		set title 'Plot of orbits: circular, elliptical, and relativistic precession'; \
		set grid; \
		set size ratio -1; \
		plot 'circular.txt' using 1:2 with lines title 'Circular', \
		'elliptical.txt' using 1:2 with lines title 'Elliptical', \
		'relativistic.txt' using 1:2 with lines title 'Relativistic'; \
		"


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log