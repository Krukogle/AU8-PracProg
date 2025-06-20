# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: main.exe Out.txt values.pdf derivatives.pdf second_derivatives.pdf antiderivatives.pdf


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono $< > $@


# ----- Generate plot of ANN prediction (part A) ----- #
values.pdf: values.gpi values.txt
	gnuplot values.gpi


# ----- Generate plot of ANN derivative prediction (part B) ----- #
derivatives.pdf: derivatives.gpi derivatives.txt
	gnuplot derivatives.gpi


# ----- Generate plot of ANN second derivative prediction (part B) ----- #
second_derivatives.pdf: second_derivatives.gpi second_derivatives.txt
	gnuplot second_derivatives.gpi


# ----- Generate plot of ANN antiderivative prediction (Part B) ----- #
antiderivatives.pdf: antiderivatives.gpi antiderivatives.txt
	gnuplot antiderivatives.gpi


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log