# Source files
CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) -r:System.Numerics.dll

# Compilation rules
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

# Default target builds Out.txt and timing.pdf
all: Out.txt timing.pdf

# Export to Out.txt
Out.txt: main.exe
	mono $< > $@

# Compile libraries
vector.dll: vector.cs
	$(MKLIB)
matrix.dll: matrix.cs vector.dll
	$(MKLIB)
QR.dll: QR.cs matrix.dll vector.dll
	$(MKLIB)

# Compile the main program
main.exe: main.cs matrix.dll vector.dll QR.dll
	$(MKEXE)

# Generate the PDF plot using gnuplot
timing.pdf: plot.gpi timing.txt
	gnuplot plot.gpi

# Clean generated files
clean:
	$(RM) *.dll *.exe *.txt *.data *.pdf *.log