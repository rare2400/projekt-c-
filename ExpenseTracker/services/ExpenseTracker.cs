/* 
Programmering i C#.NET
Projektuppgift av Ramona Reinholdz
Konsolapp för att hantera och se sina utgifter
 */

using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{

    //serivce-klass som hanterar utgifter
    public class ExpenseService
    {
        private string filePath = @"expenses.json"; //filväg där utgifter sparas i JSON-format

        private readonly List<Expense> expenses;    //listan med utgifter

        public ExpenseService()
        {
            if (File.Exists(filePath) == true)  //om JSON-filen finns läses existerande inlägg in
            {
                string jsonString = File.ReadAllText(filePath);     //läser in JSON-strängen ur filen

                //kontrollerar om filen är tom innan deserialisering av JSON-strängen
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    expenses = JsonSerializer.Deserialize<List<Expense>>(jsonString) ?? [];    //deserialiserar JSON-strängen
                }
                else
                {
                    expenses = [];  //tom lista är påbörjas om filen tom
                }
            }
            else
            {
                expenses = [];  //finns inte någon fil initieras en tom lista
            }
        }

        //lägger till en ny utgift
        public void AddExpense(Expense expense)
        {

            expenses.Add(expense);  //lägger till utgift i listan
            SaveToFile();   //sparar utgift i JSON-filen
        }

        //hämtar alla utgifter
        public List<Expense> GetAllExpenses()
        {
            if (expenses.Count == 0)    //kontrollerar om listan är tom
            {
                Console.WriteLine("Inga utgifter är tillagda.");
            }
            return expenses;    //returnerar alla utgifter
        }

        //räknar ut den totala summan av alla utgifter
        public decimal GetTotalAmount()
        {
            return expenses.Sum(e => e.Amount); //returnerar den totala summan av utgifter
        }

        //uppdaterar utgift med hjälp av index
        public bool UpdateExpense(int index, Expense updatedExpense)
        {
            if (index >= 0 && index < expenses.Count)   //kontrollerar att index är giltigt
            {
                expenses[index] = updatedExpense;
                SaveToFile(); //sparar utgift i JSON-filen
                return true;
            }
            return false;
        }

        //raderar en utgift baserat på index
        public bool RemoveExpense(int index)
        {
            if (index >= 0 && index < expenses.Count)   //kontrollerar att index är giltigt
            {
                expenses.RemoveAt(index);
                SaveToFile(); //sparar utgift i JSON-filen
                return true;
            }
            return false;
        }

        //sparar lista med utgifter till JSON-fil
        private void SaveToFile()
        {
            var jsonString = JsonSerializer.Serialize(expenses);   //serialiserar listan med utgifter till en JSON-sträng
            File.WriteAllText(filePath, jsonString);    //skriver JSON-strängen till filen
        }
    }
}
