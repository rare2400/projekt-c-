/* 
Programmering i C#.NET
Projektuppgift av Ramona Reinholdz
Konsolapp för att hantera och se sina utgifter

Service-klass för att spara och hämta data från en JSON-fil
 */

using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using ExpenseTracker.Models;    //importerar Expense-klassen

namespace ExpenseTracker.Services
{

    //serivce-klass som hanterar utgifter
    public class ExpenseStorage
    {
        private readonly string filePath = @"expenses.json"; //filväg där utgifter sparas i JSON-format

        //hämtar utgifter från JSON-fil
        public List<Expense> GetExpenses()
        {
            if (!File.Exists(filePath)) return []; //om filen inte finns returneras en tom lista

            string jsonString = File.ReadAllText(filePath);     //läser in JSON-strängen ur filen

            if (string.IsNullOrWhiteSpace(jsonString)) return [];   //kontrollerar om filen är tom returneras en tom lista

            return JsonSerializer.Deserialize<List<Expense>>(jsonString) ?? [];    //deserialiserar JSON-strängen
        }


        //sparar lista med utgifter till JSON-fil
        public void SaveExpenses(List<Expense> expenses)
        {
            var jsonString = JsonSerializer.Serialize(expenses, new JsonSerializerOptions   //serialiserar listan med utgifter till en JSON-sträng
            {
                WriteIndented = true   //formaterar JSON-strängen för bättre läsbarhet
            });   
            File.WriteAllText(filePath, jsonString);    //skriver JSON-strängen till filen
        }
    }
}
