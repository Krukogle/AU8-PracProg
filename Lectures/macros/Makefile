CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 

Out.txt: main.exe main
	mono main.exe > $@
	./main > $@

main.exe: main.cs ; $(MKEXE)