Out.txt: main.exe input.txt
	@echo "Running main.exe with input.txt"
	@echo "Output is redirected to Out.txt"
	@mono main.exe < input.txt > Out.txt

main.exe: main.cs generic_list.cs
	@mcs -target:exe -out:main.exe main.cs generic_list.cs

input.txt:
	@printf "129.24 24.8             4.847\n\t\t88.6   33.745 7.78\n30.39  99.811              6.723\n\t\t-1.33   96.3   2.23\n" > input.txt

clean:
	rm -f Out.txt main.exe input.txt