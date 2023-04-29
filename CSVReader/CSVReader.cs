using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CSVReader;

public class CSVReader
{
    public Table CSVTable { get; set; }
    public string FilePath { get; set; }
    public List<string[]> Data { get; set; } 
    public CSVReader()
    {
        CSVTable = new Table();
        Data = new List<string[]>();
    }

    public void NavigateOptions()
    {
        var option = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("What would you like to do?")
        .PageSize(10)
        .MoreChoicesText("[blue](Move up and down to navigate the categories.)[/]")
        .AddChoices(new[]
         {
                                "Read CSV File", "Create Table", "Display Table", "Add Row", "Delete Row", "Save File", "Exit"

         }));

        if (option == "Read CSV File")
        {
            Console.WriteLine("Absolute file paths are written like: \"C:\\Users\\YourUserName\\Documents\\example.csv\"");
            string file = AnsiConsole.Ask<string>("Please enter the absolute file path here without quotation marks: ");
            string extension = Path.GetExtension(file);

            if (extension == ".csv")
            {
                FilePath = @file;
                Console.Clear();
                ReadCSVTable();
            }
            else
            {
                Console.WriteLine("This is not a CSV file. Please try again.");
                NavigateOptions();
            }
        }
        if (option == "Create Table")
        {
            WriteCSVTable();

        }
       if(option == "Add Row") { AddToTable(); }
       if(option == "Display Table") { DisplayTable(); }
       if(option == "Delete Row") { DeleteRow(); }
       if(option == "Save File") { CSVTableSave(); }
       if(option == "Exit") { return; }
    }
    public void ReadCSVTable()
    {
        StreamReader reader = new StreamReader(FilePath);

        string header = reader.ReadLine();
        string[] columns = header.Split(',');
        Data.Add(columns);

        foreach(string  column in columns)
        {
            CSVTable.AddColumn(column);
        }
        while(!reader.EndOfStream)
        {
            string totalValues = reader.ReadLine();
            string[] rows = totalValues.Split(',');
           
            CSVTable.AddRow(rows);
        
            Data.Add(rows);
        }
        Console.WriteLine("Table successfully loaded");
        NavigateOptions();
    }

    public void ReturnToMenu()
    {
        var option = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("Would you like to go back to the main menu?")
        .PageSize(10)
        .MoreChoicesText("[blue](Move up and down to navigate the categories.)[/]")
        .AddChoices(new[]
         {
                                            "Yes", "No"

        }));

        if (option == "Yes") { Console.Clear(); NavigateOptions(); }
    }
    public void DisplayTable()
    {
        if (Data.Count <= 0) { Console.WriteLine("Cannot display empty table."); }
        else { AnsiConsole.Write(CSVTable); }
        ReturnToMenu();
    }
    public void WriteCSVTable()
    {
        int num = AnsiConsole.Ask<int>("How many columns would you like to have in your table (remember: one column needs to be an ID)?");
        List<string> columns = new List<string>();
        for (int i = 0; i < num; i++)
        {
            string column = AnsiConsole.Ask<string>("Please enter a column title: ");
            columns.Add(column);
            CSVTable.AddColumn(column);
        }
        Data.Add(columns.ToArray());

        if (Data.Count > 0)
        { 
        Console.WriteLine("Table has been created!");
        ReturnToMenu();
        }

    }

    public void AddToTable()
    {
        if (Data.Count <= 0) { Console.WriteLine("Unable to add to empty table."); ReturnToMenu(); }
        else
        {
            bool answer = true;

            while (answer)
            {
                AnsiConsole.Write(CSVTable);
                string column = AnsiConsole.Ask<string>($"Please enter the values for the row seperating each input with a space");
                string[] inputs = column.Split(" ");
                Data.Add(inputs);
                CSVTable.AddRow(inputs);
                Console.WriteLine("Here is the updated table: ");
                AnsiConsole.Write(CSVTable);
                var option = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
               .Title("Would you like to continue?")
               .PageSize(10)
               .MoreChoicesText("[blue](Move up and down to navigate the categories.)[/]")
               .AddChoices(new[]
               {
                                    "Yes", "No"

               }));

                if (option == "No") { answer = false; }
                Console.Clear();
            }
            NavigateOptions();
        }
    }

    public void DeleteRow()
    {
        int num = AnsiConsole.Ask<int>("What number ID corresponds to the row you would like to delete?");
        string[] desiredRow = Data.FirstOrDefault(p => p[0] == num.ToString());
        if (desiredRow != null)
        {
            int index = Data.FindIndex(p => p[0] == num.ToString());
            Data.Remove(desiredRow);
            CSVTable.RemoveRow(index - 1);
            Console.WriteLine("Table row successfully deleted");
        }
        ReturnToMenu();
    }

    public void CSVTableSave()
    {
        string fileName = AnsiConsole.Ask<string>("What do you want to name the CSV file?"); ;

        using (StreamWriter writer = new StreamWriter($"{fileName}.csv"))
        {
            foreach (string[] row in Data)
            {
                writer.WriteLine(string.Join(",", row));
            }
        }

        Console.WriteLine("File created successfully."); 
        ReturnToMenu();
    }

}
