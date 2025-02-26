Out1.txt: main1.exe inputfile1.txt
	mono $< $$(cat inputfile1.txt) > $@

inputfile1.txt:
	echo "-numbers:1,2,3,4,5" > $@

main1.exe: main1.cs
	mcs -target:exe -out:$@ $<

clean:
	rm -f main1.exe Out1.txt inputfile1.txt

test:
	echo rm -f main.exe Out.txt