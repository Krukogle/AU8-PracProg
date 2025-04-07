# Libraries
libPath = ../../Classes/
LIBRARIES = $(wildcard $(libPath)*.cs)

# Default target builds Out.txt and timing.pdf
# all: Out.txt timing.pdf

# Export to Out.txt
Out.txt: main.exe
	mono $< > $@

# Compile the main program
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe

# Generate the PDF plot using gnuplot
# timing.pdf: plot.gpi timing.txt
# 	gnuplot plot.gpi

# Clean generated files
clean:
	$(RM) *.dll *.exe *.txt *.data *.pdf *.log
