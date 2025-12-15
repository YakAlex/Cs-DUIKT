using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeShopDelegates
{
    
    public delegate void ItemAction(MenuItem item);
    
    public delegate bool ItemCheck(MenuItem item);
    
    
    public abstract class MenuItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public MenuItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public abstract void DisplayInfo();
    }

    public class Coffee : MenuItem
    {
        public int VolumeML { get; set; }
        public Coffee(string name, decimal price, int volume) : base(name, price) { VolumeML = volume; }
        public override void DisplayInfo() => Console.WriteLine($"[Кава] {Name}, {VolumeML}мл - {Price} грн.");
    }

    public class Dessert : MenuItem
    {
        public bool IsVegan { get; set; }
        public Dessert(string name, decimal price, bool isVegan) : base(name, price) { IsVegan = isVegan; }
        public override void DisplayInfo() => Console.WriteLine($"[Десерт] {Name} (Веган: {IsVegan}) - {Price} грн.");
    }

    public class Tea : MenuItem
    {
        public string Type { get; set; }
        public Tea(string name, decimal price, string type) : base(name, price) { Type = type; }
        public override void DisplayInfo() => Console.WriteLine($"[Чай] {Name} ({Type}) - {Price} грн.");
    }


    public static class CafeManager
    {
        public static void ProcessMenu(List<MenuItem> items, ItemAction action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static void FindAndShow(List<MenuItem> items, ItemCheck checkCondition)
        {
            Console.WriteLine("--- Результати пошуку: ---");
            foreach (var item in items)
            {
                if (checkCondition(item)) 
                {
                    item.DisplayInfo();
                }
            }
            Console.WriteLine("--------------------------");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Кав'ярня 3.0: Делегати ===\n");

            List<MenuItem> menu = new List<MenuItem>
            {
                new Coffee("Еспресо", 45.00m, 30),
                new Coffee("Латте", 60.00m, 300),
                new Dessert("Чізкейк", 85.00m, false),
                new Dessert("Яблуко запечене", 50.00m, true),
                new Tea("Зелений дракон", 40.00m, "Зелений")
            };

            
            Console.WriteLine("1. Простий список (через іменований метод):");
            
            CafeManager.ProcessMenu(menu, PrintShortInfo); 
            
            Console.WriteLine();


            Console.WriteLine("2. Піднімаємо ціни на 5 грн (через лямбда-вираз)...");
            
            CafeManager.ProcessMenu(menu, item => 
            {
                item.Price += 5.00m; 
            });
            
            CafeManager.ProcessMenu(menu, PrintShortInfo);
            Console.WriteLine();


           
            Console.WriteLine("3. Пошук бюджетних товарів (< 60 грн):");
            
            CafeManager.FindAndShow(menu, item => item.Price < 60);


           
            Console.WriteLine("4. Пошук веганських десертів:");

            CafeManager.FindAndShow(menu, item => 
            {
                
                if (item is Dessert d)
                {
                    return d.IsVegan; 
                }
                return false;
            });
            
        }

        static void PrintShortInfo(MenuItem item)
        {
            Console.WriteLine($" -> {item.Name} коштує {item.Price} грн");
        }
    }
}
