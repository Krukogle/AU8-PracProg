# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt circle_errors.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of error function comparison ----- #
circle_errors.pdf: circle_errors.gpi circle_errors.txt
	gnuplot circle_errors.gpi


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log