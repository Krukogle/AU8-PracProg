Out.txt: main.exe
	@echo "Running main.exe and directing output to Out.txt..."
	@mono main.exe > Out.txt

main.exe: main.cs
	@mcs -target:exe -reference:System.Numerics.dll -out:main.exe main.cs

clean:
	rm -f System.Numerics.dll main.exe Out.txt