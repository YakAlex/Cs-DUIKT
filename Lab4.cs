using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            return Price.CompareTo(other.Price);
        }

        public object Clone()
        {
            return MemberwiseClone();
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

    public class Inventory : IEnumerable<Product>
    {
        private List<Product> _products = new List<Product>();

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public IEnumerable<Product> GetProductsByPriceLimit(decimal maxPrice)
        {
            foreach (var product in _products)
            {
                if (product.Price <= maxPrice)
                {
                    yield return product;
                }
            }
        }

        public IEnumerator<Product> GetEnumerator()
        {
            foreach (var product in _products)
            {
                yield return product;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            inventory.Add(new Smartphone("iPhone 15 Pro", 1200m, "iOS"));
            inventory.Add(new Product("USB-C Cable", 20m));
            inventory.Add(new Smartphone("Samsung S24", 1100m, "Android"));
            inventory.Add(new Product("Laptop Stand", 45m));
            inventory.Add(new Smartphone("Google Pixel 8", 800m, "Android"));

            Console.WriteLine("--- Custom Iterator (Yield): Products under 100$ ---");
            foreach (var item in inventory.GetProductsByPriceLimit(100m))
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("\n--- LINQ: Filtering (Android Smartphones) ---");
            var androidPhones = inventory
                .OfType<Smartphone>()
                .Where(p => p.OperatingSystem == "Android");

            foreach (var phone in androidPhones)
            {
                Console.WriteLine(phone);
            }

            Console.WriteLine("\n--- LINQ: Projection (Only Names, Sorted) ---");
            var productNames = inventory
                .Select(p => p.Name)
                .OrderBy(n => n);

            foreach (var name in productNames)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine("\n--- LINQ: Aggregation ---");
            decimal totalValue = inventory.Sum(p => p.Price);
            decimal averagePrice = inventory.Average(p => p.Price);
            decimal maxPrice = inventory.Max(p => p.Price);

            Console.WriteLine($"Total Inventory Value: {totalValue}$");
            Console.WriteLine($"Average Price: {averagePrice}$");
            Console.WriteLine($"Most Expensive Item: {maxPrice}$");

            Console.ReadKey();
        }
    }
}
