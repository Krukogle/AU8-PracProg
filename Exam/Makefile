# ----- Libraries ----- #
LIBPATH = ../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt secular.pdf timing.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of secular function ----- #
secular.pdf: secular.gpi
	gnuplot secular.gpi


# ----- Generate plot of O(n^2) timing ----- #
timing.pdf: timing.gpi
	gnuplot timing.gpi


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log