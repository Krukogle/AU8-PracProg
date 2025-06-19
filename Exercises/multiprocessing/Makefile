# Source files
CODE = $(filter %.cs,$^)

# Compilation rules
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

# Generate Out.txt by running main.exe.
# Measure the processor times running Main with different number of threads
N = 1e8
TIME = time --portability --append --output $@
Out.txt: main.exe
	>$@
	$(TIME) mono $< -terms:$N -threads:1 >> $@
	$(TIME) mono $< -terms:$N -threads:2 >> $@
	$(TIME) mono $< -terms:$N -threads:3 >> $@
	$(TIME) mono $< -terms:$N -threads:4 >> $@

# Compile the main program
main.exe: main.cs
	$(MKEXE)

# Clean generated files
clean:
	$(RM) *.exe Out.txt