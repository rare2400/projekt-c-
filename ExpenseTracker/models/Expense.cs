/* 
Programmering i C#.NET
Projektuppgift av Ramona Reinholdz
Konsolapp för att hantera och se sina utgifter

Modell för utgift.
 */

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public decimal Amount { get; set;}  //belopp
        public string? Category { get; set;}    //kategori
        public string? Description { get; set;} //beskrivning av utgift
        public DateTime Date { get; set;} = DateTime.Now;   //datum för utgift med default-datum
    }
}