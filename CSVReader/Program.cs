using Spectre.Console;

namespace CSVReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CSVReader reader = new CSVReader();
            reader.NavigateOptions();
            Console.WriteLine("not what I think you wanted me to do but alas..");
            Console.ReadKey(true);

        }
    }
}