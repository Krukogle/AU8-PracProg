using System; // Import the System namespace

class Program
{
    static void Main()
        {
            string username = Environment.UserName; // Automatically get the system username
            Console.WriteLine($"Hello, {username}!");
        }
}