# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt data_fit.pdf fit_limits.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of data with errorbars ----- #
data_fit.pdf: data.txt fit.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel 'Time (days)'; \
		set ylabel 'Activity (relative units)'; \
		set title 'Activity vs Time with Error Bars'; \
		set grid; \
		plot 'data.txt' using 1:2:3 with yerrorbars title 'Data', \
		 'fit.txt' using 1:2 with lines title 'Fitted values'; \
		"


# ----- Generate plot of fit and fit at uncertainty limits ----- #
fit_limits.pdf: data.txt fit.txt fit_limits.txt
	gnuplot -e "\
		set terminal pdf; \
		set output '$@'; \
		set xlabel 'Time (days)'; \
		set ylabel 'Activity (relative units)'; \
		set title 'Activity vs Time with Fit Limits'; \
		set grid; \
		plot 'data.txt' using 1:2:3 with yerrorbars title 'Data', \
		 'fit.txt' using 1:2 with lines title 'Fitted values', \
		 'fit_limits.txt' using 1:2 with lines title 'Lower limit', \
		 'fit_limits.txt' using 1:3 with lines title 'Upper limit'; \
		"


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log