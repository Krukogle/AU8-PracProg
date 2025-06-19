# Source files
CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) -r:System.Numerics.dll

# Compilation rules
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

# List of functions to plot
PLOTS = erf.pdf gamma.pdf lngamma.pdf cgamma.pdf  # Add more plots as needed

# Generate all plots
all: $(PLOTS)

# Generate data.txt with all functions
data.txt: main.exe
	mono $< > $@

# Rule for individual plots
%.pdf: %.gpi data.txt
	gnuplot $<

# Compile the shared library
sfuns.dll: sfuns.cs
	$(MKLIB)

# Compile the main program
main.exe: main.cs sfuns.dll
	$(MKEXE)

# Clean generated files
clean:
	$(RM) *.dll *.exe *.pdf data.txt