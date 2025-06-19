Out.txt : main.exe
	mono $< > $@

main.exe : main.cs
	mcs -target:exe -out:main.exe main.cs

.PHONY: clean
clean:   # removes all files used in building the project
	$(RM) main.exe Out.txt

test:
	echo $(RM)
