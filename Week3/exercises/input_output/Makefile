all: Out1.txt Out2.txt

Out1.txt: main1.exe inputfile1.txt
	mono main1.exe $$(cat inputfile1.txt) > Out1.txt

Out2.txt: main2.exe inputfile2.txt
	mono main2.exe < inputfile2.txt 2> Out2.txt


inputfile1.txt:
	echo "-numbers:1,2,3,4,5" > $@

inputfile2.txt:
	echo "1 2 3 4 5 6" > $@	


main1.exe: main1.cs
	mcs -target:exe -out:$@ $<

main2.exe: main2.cs
	mcs -target:exe -out:$@ $<


clean:
	rm -f main1.exe main2.exe Out1.txt Out2.txt inputfile1.txt inputfile2.txt