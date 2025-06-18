# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt convergence_rmax.pdf convergence_rmin.pdf convergence_acc.pdf convergence_eps.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of rmax convergence test ----- #
convergence_rmax.pdf: convergence_rmax.gpi convergence_rmax.txt
	gnuplot convergence_rmax.gpi


# ----- Generate plot of rmin convergence test ----- #
convergence_rmin.pdf: convergence_rmin.gpi convergence_rmin.txt
	gnuplot convergence_rmin.gpi


# ----- Generate plot of acc convergence test ----- #
convergence_acc.pdf: convergence_acc.gpi convergence_acc.txt
	gnuplot convergence_acc.gpi


# ----- Generate plot of eps convergence test ----- #
convergence_eps.pdf: convergence_eps.gpi convergence_eps.txt
	gnuplot convergence_eps.gpi


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log