# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt linear.pdf quadratic.pdf cubic.pdf cubic_compare.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of linear spline interpolation ----- #
linear.pdf: linear.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic x}'; \
		set ylabel '{/:Italic y}'; \
		set title 'Linear spline interpolation of cos({/:Italic x})'; \
		set grid; \
		plot cos(x) with lines title 'cos({/:Italic x})', \
		 'linear.txt' using 1:2 with lines title 'Linear splines', \
		 'linear.txt' using 1:3 with lines title 'Integral'; \
		"


# ----- Generate plot of quadratic spline interpolation ----- #
quadratic.pdf: quadratic.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic x}'; \
		set ylabel '{/:Italic y}'; \
		set title 'Quadratic spline interpolation of sin({/:Italic x})'; \
		set grid; \
		plot sin(x) with lines title 'sin({/:Italic x})', \
		 'quadratic.txt' using 1:2 with lines title 'Quadratic splines', \
		 'quadratic.txt' using 1:3 with lines title 'Derivative', \
		 'quadratic.txt' using 1:4 with lines title 'Integral'; \
		"


# ----- Generate plot of cubic spline interpolation ----- #
cubic.pdf: cubic.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic x}'; \
		set ylabel '{/:Italic y}'; \
		set title 'Cubic spline interpolation of sin({/:Italic x})'; \
		set grid; \
		plot sin(x) with lines title 'sin({/:Italic x})', \
		 'cubic.txt' using 1:2 with lines title 'Cubic splines', \
		 'cubic.txt' using 1:3 with lines title 'Derivative', \
		 'cubic.txt' using 1:4 with lines title 'Integral'; \
		"


# ----- Generate plot of cubic spline interpolation comparison ----- #
cubic_compare.pdf: cubic_compare.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel '{/:Italic x}'; \
		set ylabel '{/:Italic y}'; \
		set title 'Cubic spline comparison of sin({/:Italic x})'; \
		set grid; \
		plot 'cubic_compare.txt' using 1:3 with lines title 'My cspline', \
		 'cubic_compare.txt' using 1:2 smooth cspline with lines title 'Gnuplot cspline'; \
		"


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log