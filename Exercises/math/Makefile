Out.txt : main.exe
	mono $< > $@

sfuns.dll: sfuns.cs
	mcs -target:library -out:sfuns.dll sfuns.cs

main.exe : main.cs sfuns.dll
	mcs -target:exe -reference:sfuns.dll -out:main.exe main.cs

.PHONY: clean
clean:   # removes all files used in building the project
	$(RM) main.exe Out.txt sfuns.dll constants.dll

test:
	echo $(RM)
