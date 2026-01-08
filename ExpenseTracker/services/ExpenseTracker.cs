/* 
Programmering i C#.NET
Projektuppgift av Ramona Reinholdz
Konsolapp för att hantera och se sina utgifter

Service-klass för hantering av utgifter
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{

    //serivce-klass som hanterar utgifter
    public class ExpenseService
    {

        private readonly List<Expense> expenses;    //listan med utgifter
        private readonly ExpenseStorage storageService;

        public ExpenseService()
        {
            storageService = new ExpenseStorage();      //skapar instans av ExpenseStorage-klassen
            expenses = storageService.GetExpenses();     //hämtar utgifter från JSON-filen genom ExpenseStorage-klassen
        }

        //lägger till en ny utgift med hjälp av objektet i Expense-klassen
        public void AddExpense(Expense expense)
        {

            expenses.Add(expense);  //lägger till utgift i listan
            storageService.SaveExpenses(expenses);   //sparar utgift i JSON-filen
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

        //uppdaterar utgift med hjälp av index och uppdaterat Expense-objekt
        public bool UpdateExpense(int index, Expense updatedExpense)
        {
            if (index >= 0 && index < expenses.Count)   //kontrollerar att index är giltigt
            {
                expenses[index] = updatedExpense;
                storageService.SaveExpenses(expenses);   //sparar utgift i JSON-filen
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
                storageService.SaveExpenses(expenses); //sparar utgift i JSON-filen
                return true;
            }
            return false;
        }
    }
}
