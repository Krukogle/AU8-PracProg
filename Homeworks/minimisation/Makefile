# ----- Libraries ----- #
LIBPATH = ../../Classes/
LIBRARIES = $(wildcard $(LIBPATH)*.cs)


# ----- Ensure everything is made ----- #
all: higgs.txt main.exe Out.txt higgs.pdf


# ----- Export Higgs data ----- #
higgs.txt:
	@echo "# energy E[GeV], signal σ(E) [certain units], experimental uncertainty Δσ [same units]" > higgs.txt
	@echo "101 -0.25 2.0" >> higgs.txt
	@echo "103 -0.30 2.0" >> higgs.txt
	@echo "105 -0.15 1.9" >> higgs.txt
	@echo "107 -1.71 1.9" >> higgs.txt
	@echo "109 0.81 1.9" >> higgs.txt
	@echo "111 0.65 1.9" >> higgs.txt
	@echo "113 -0.91 1.9" >> higgs.txt
	@echo "115 0.91 1.9" >> higgs.txt
	@echo "117 0.96 1.6" >> higgs.txt
	@echo "119 -2.52 1.6" >> higgs.txt
	@echo "121 -1.01 1.6" >> higgs.txt
	@echo "123 2.01 1.6" >> higgs.txt
	@echo "125 4.83 1.6" >> higgs.txt
	@echo "127 4.58 1.6" >> higgs.txt
	@echo "129 1.26 1.3" >> higgs.txt
	@echo "131 1.01 1.3" >> higgs.txt
	@echo "133 -1.26 1.3" >> higgs.txt
	@echo "135 0.45 1.3" >> higgs.txt
	@echo "137 0.15 1.3" >> higgs.txt
	@echo "139 -0.91 1.3" >> higgs.txt
	@echo "141 -0.81 1.1" >> higgs.txt
	@echo "143 -1.41 1.1" >> higgs.txt
	@echo "145 1.36 1.1" >> higgs.txt
	@echo "147 0.50 1.1" >> higgs.txt
	@echo "149 -0.45 1.1" >> higgs.txt
	@echo "151 1.61 1.1" >> higgs.txt
	@echo "153 -2.21 1.1" >> higgs.txt
	@echo "155 -1.86 0.9" >> higgs.txt
	@echo "157 1.76 0.9" >> higgs.txt
	@echo "159 -0.50 0.9" >> higgs.txt


# ----- Compile main program ----- #
main.exe: main.cs $(LIBRARIES)
	mcs main.cs $(LIBRARIES) -out:main.exe


# ----- Run main program ----- #
Out.txt: main.exe
	mono main.exe < higgs.txt 1> Out.txt


# ----- Generate plot of Higgs signal ----- #
higgs.pdf: higgs.gpi higgs.txt higgs_fit.txt
	gnuplot higgs.gpi


# ----- Clean generated files ----- #
clean:
	rm -f *.exe *.pdf *.txt *.log