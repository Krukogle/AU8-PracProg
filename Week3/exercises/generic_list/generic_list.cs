// Class definition
public class genlist<T>{

    // Declare an array "data" to store elements of type T
	public T[] data;

    // Property to get the size of the list
	public int size => data.Length;

    // Indexer to access elements of the list
	public T this[int i] => data[i];

    // Constructor. Initializes "data" as an empty array
    public genlist(){
        data = new T[0];
    }

    // Method to add an element "item" to the list
	public void add(T item){
        // Create a new array "newdata" of size (size+1)
		T[] newdata = new T[size+1];
        // Copy elements from "data" to "newdata"
		System.Array.Copy(data, newdata, size);
        // Add "item" to the end of "newdata"
		newdata[size] = item;
        // Assign "newdata" to "data"
		data = newdata;
	}

    // Method to remove an element at index "i" from the list
    public void remove(int i){
        // If "i" is out of bounds, throw an exception
        if(i < 0 || i >= size){
            throw new System.IndexOutOfRangeException($"Index {i} is out of range");
        }
        // Create a new array "newdata" of size (size-1)
        T[] newdata = new T[size-1];
        // Copy elements from "data" to "newdata" up to index "i"
        System.Array.Copy(data, newdata, i);
        // Copy elements from "data" to "newdata" starting from index "i+1"
        System.Array.Copy(data, i+1, newdata, i, size-i-1);
        // Assign "newdata" to "data"
        data = newdata;
    }
}