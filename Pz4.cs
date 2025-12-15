using System;
using System.Collections.Generic;

namespace ProductSystem
{
    public class Product : ICloneable, IComparable<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public int CompareTo(Product other)
        {
            if (other == null) return 1;
            return this.Price.CompareTo(other.Price);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Name} - {Price}$";
        }
    }

    public class Smartphone : Product
    {
        public string OperatingSystem { get; set; }

        public Smartphone(string name, decimal price, string os) : base(name, price)
        {
            OperatingSystem = os;
        }

        public override string ToString()
        {
            return $"{Name} ({OperatingSystem}) - {Price}$";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Product> inventory = new List<Product>
            {
                new Smartphone("iPhone 15", 1200m, "iOS"),
                new Product("USB Cable", 15m),
                new Smartphone("Samsung S24", 1100m, "Android"),
                new Product("Power Bank", 50m)
            };

            Console.WriteLine("--- Inventory Before Sorting ---");
            foreach (var item in inventory)
            {
                Console.WriteLine(item);
            }

            inventory.Sort();

            Console.WriteLine("\n--- Inventory After Sorting (by Price) ---");
            foreach (var item in inventory)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("\n--- Cloning Test ---");
            Smartphone original = new Smartphone("Pixel 8", 700m, "Android");
            Smartphone copy = (Smartphone)original.Clone();

            copy.Price = 650m;
            copy.Name = "Pixel 8 (Used)";

            Console.WriteLine($"Original: {original}");
            Console.WriteLine($"Copy:     {copy}");

            Console.ReadKey();
        }
    }
}
