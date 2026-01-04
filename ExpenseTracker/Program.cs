/* 
Programmering i C#.NET
Projektuppgift av Ramona Reinholdz
Konsolapp för att hantera och se sina utgifter
 */


using System;
using System.Collections.Generic;
using System.Globalization;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

CultureInfo.CurrentCulture = new CultureInfo("sv-SE"); //sätter kultur till svenska för korrekt valutahantering

ExpenseService service = new ExpenseService();

while (true)    //loop för meny
{

    //display header with title and instructions
    Console.ForegroundColor = ConsoleColor.DarkBlue;    //header color
    Console.WriteLine("==============================================================");
    Console.WriteLine("               Välkommen till ExpenseTracker!                 ");
    Console.WriteLine("\n          Här kan du se och hantera dina utgifter, \n          eller varför inte skapa din egen budget?");
    Console.WriteLine("==============================================================\n");
    Console.ResetColor();   //reset color
    Console.WriteLine("1. Lägg till utgift");
    Console.WriteLine("2. Visa dina utgifter");
    Console.WriteLine("3. Ändra utgift");
    Console.WriteLine("4. Ta bort utgift\n");
    Console.WriteLine("X. Avsluta\n");  //menyval för att avsluta programmet

    Console.WriteLine("\nVälj ett alternativ: ");
    string? choice = Console.ReadLine();

    Console.Clear();

    //switch-sats för meny-valen
    switch (choice)
    {
        case "1":
            AddExpense(service);    //lägga till utgift
            break;
        case "2":
            ShowSortedExpenses(service);    //visa utgifter i sorterad ordning
            break;
        case "3":
            UpdateExpense(service);   //ändra utgift
            break;
        case "4":
            RemoveExpense(service);  //ta bort utgift
            break;
        case "x":
        case "X":
            return;     //avsluta programmet
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt val, försök igen.");
            Console.ResetColor();
            break;
    }

    Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka till menyn...");
    Console.ReadKey();
    Console.Clear();
}

//funktion för att lägga till utgift
void AddExpense(ExpenseService service)
{
    Console.WriteLine("======================== Lägg till utgift ========================");
    Console.Write("Belopp: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal amount))  //kontrollerar att input är ett giltigt tal
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ogiltigt belopp. Utgiften kunde inte läggas till.\n");
        Console.ResetColor();
        return;
    }

    //läser in kategori, beskrivning och datum
    Console.Write("Kategori: ");
    string? category = Console.ReadLine();

    Console.Write("Beskrivning: ");
    string? description = Console.ReadLine();

    Console.Write("Datum (YYYY-MM-DD, lämna tomt för dagens datum): ");
    string? dateInput = Console.ReadLine();

    DateTime date;

    if (string.IsNullOrWhiteSpace(dateInput))   //anges inget datum används dagens datum
    {
        date = DateTime.Now;
    }
    else if (!DateTime.TryParse(dateInput, out date))   //kontrollerar att datumet är giltigt
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ogiltigt datumformat. Använd YYYY-MM-DD.\n");
        Console.ResetColor();
        return;
    }

    //skapar ny utgift att lägga till i listan
    Expense expense = new Expense
    {
        Amount = amount,
        Category = category,
        Description = description,
        Date = date
    };

    //lägger till utgiften i listan med service-klassen
    service.AddExpense(expense);
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("Utgift tillagd!\n");
    Console.ResetColor();
}

//funktion för att visa utgifter i sorterad ordning
void ShowSortedExpenses(ExpenseService service)
{
    List<Expense> expenses = service.GetAllExpenses().OrderBy(e => e.Date).ToList();    //sorterar lista efter datum

    if (expenses.Count == 0)    //kontrollerar om listan är tom
    {
        Console.WriteLine("Inga utgifter är registrerade.\n");
        return;
    }

    Console.WriteLine("\n============================= Dina utgifter: =============================\n");

    //visar utgifter i tabellformat
    Console.ForegroundColor = ConsoleColor.DarkBlue;
    Console.WriteLine("--------------------------------------------------------------------------");
    Console.WriteLine($"{"Nr",-4}{"Datum",-12}{"Kategori",-15}{"Beskrivning",-30}{"Belopp (SEK)",12}");
    Console.WriteLine("--------------------------------------------------------------------------");
    Console.ResetColor();

    //loopar igenom varje utgift och visar dem i tabellformat
    for (int i = 0; i < expenses.Count; i++)
    {
        Expense e = expenses[i];    //hämtar utgift

        string formattedDate = $"{e.Date:yyyy-MM-dd}";      //formaterar datum
        string amount = $"{e.Amount:C}";        //formaterar belopp

        Console.WriteLine($"{i + 1,-4}{formattedDate,-12}{e.Category,-15}{e.Description,-30}{amount,12}");
        Console.WriteLine("--------------------------------------------------------------------------");
    }

    //visar den totala summan av alla utgifter
    decimal total = service.GetTotalAmount();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\nTotal summa: {total:C}\n");
    Console.ResetColor();
}

//funktion för att visa alla utgifter
void ShowExpenses(ExpenseService service)
{
    List<Expense> expenses = service.GetAllExpenses();      //hämtar alla utgifter

    if (expenses.Count == 0)    //kontrollerar om listan är tom
    {
        Console.WriteLine("Inga utgifter är registrerade.\n");
        return;
    }

    //visar utgifter i tabellformat
    Console.ForegroundColor = ConsoleColor.DarkBlue;
    Console.WriteLine("--------------------------------------------------------------------------");
    Console.WriteLine($"{"Nr",-4}{"Datum",-12}{"Kategori",-15}{"Beskrivning",-30}{"Belopp (SEK)",12}");
    Console.WriteLine("--------------------------------------------------------------------------");
    Console.ResetColor();

    //loopar igenom varje utgift och visar dem i tabellformat
    for (int i = 0; i < expenses.Count; i++)
    {
        Expense e = expenses[i];    //hämtar utgift

        string formattedDate = $"{e.Date:yyyy-MM-dd}";      //formaterar datum
        string amount = $"{e.Amount:N2} kr";        //formaterar belopp

        Console.WriteLine($"{i + 1,-4}{formattedDate,-12}{e.Category,-15}{e.Description,-30}{amount,12}");
        Console.WriteLine("--------------------------------------------------------------------------");
    }
}

//funktion för att uppdatera utgift
void UpdateExpense(ExpenseService service)
{
    List<Expense> expenses = service.GetAllExpenses();      //hämtar alla utgifter

    if (expenses.Count == 0)    //kontrollerar om listan är tom
    {
        Console.WriteLine("Inga utgifter är registrerade.\n");
        return;
    }

    ShowExpenses(service);  //anropar funktion som visar alla utgifter

    Console.Write("Ange numret på utgiften du vill ändra: ");
    if (!int.TryParse(Console.ReadLine(), out int index))   //kontrollerar att input är ett giltigt tal
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ogiltigt index.\n");
        Console.ResetColor();
        return;
    }

    Console.WriteLine("\n============================ Uppdatera utgift ============================\n");
    Console.WriteLine("Vill du inte uppdatera ett fält, tryck bara Enter för att behålla det gamla värdet.\n");

    index--;    //justera för listindex
    var oldExpense = expenses[index];   //hämtar utgiften som ska uppdateras

    //skriver ut de gamla värdena och läser in nya värden, om inga nya värden anges behålls de gamla
    Console.Write($"Nytt belopp [{oldExpense.Amount}]: ");
    string? input = Console.ReadLine();
    decimal amount = string.IsNullOrWhiteSpace(input) ? oldExpense.Amount : decimal.Parse(input);

    Console.Write($"Ny kategori [{oldExpense.Category}]: ");
    string? category = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(category)) category = oldExpense.Category;

    Console.Write($"Ny beskrivning [{oldExpense.Description}]: ");
    string? description = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(description)) description = oldExpense.Description;

    Console.Write($"Nytt Datum (YYYY-MM-DD) [{oldExpense.Date:yyyy-MM-dd}]: ");
    string? dateInput = Console.ReadLine();

    DateTime date;

    if (string.IsNullOrWhiteSpace(dateInput))
    {
        date = oldExpense.Date;  //behåll det gamla datumet
    }
    else if (!DateTime.TryParse(dateInput, out date))   //kontrollerar att datumet är giltigt
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ogiltigt datumformat. Använd YYYY-MM-DD.\n");
        Console.ResetColor();
        return;
    }

    //uppdaterar utgift i listan
    Expense updatedExpense = new Expense
    {
        Amount = amount,
        Category = category,
        Description = description,
        Date = date
    };

    //uppdaterar uppgiften med service-klassen och visar resultatet för användaren
    if (service.UpdateExpense(index, updatedExpense))
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nUtgiften är nu uppdaterad!");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nOgiltigt index angivet.");
        Console.ResetColor();
    }
}

//funktion för att ta bort utgift
void RemoveExpense(ExpenseService service)
{
    List<Expense> expenses = service.GetAllExpenses();  //hämtar alla utgifter

    if (expenses.Count == 0)    //kontrollerar om listan är tom
    {
        Console.WriteLine("Inga utgifter är registrerade.\n");
        return;
    }

    //visar alla utgifter
    ShowExpenses(service);

    Console.WriteLine("\n============================== Radera utgift =============================\n");
    Console.Write("Ange numret på utgiften du vill ta bort: ");

    if (!int.TryParse(Console.ReadLine(), out int index))   //kontrollerar att input är giltigt
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nOgiltigt index angivet.");
        Console.ResetColor();
        return;
    }

    index--;    //justera för listindex

    //tar bort uppgiften med service-klassen och visar resultatet för användaren
    if (service.RemoveExpense(index))
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nUtgift är nu borttagen!");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nOgiltigt index angivet.");
        Console.ResetColor();
    }
}