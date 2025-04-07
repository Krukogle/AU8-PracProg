class main{

public static int Main(){
	for(double x=-3;x<=3;x+=1.0/8){
		System.Console.WriteLine($"{x} {sfuns.erf(x)}");
		}
return 0;
}

}