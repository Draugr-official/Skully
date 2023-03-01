using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Wrote text to file!");
        File.WriteAllText("hello.txt", "Hello, World! rah.");
    }
}