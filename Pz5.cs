using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly object _syncLock = new object();

        public void AddSafe(Product product)
        {
            lock (_syncLock)
            {
                Console.WriteLine($"[Потік {Thread.CurrentThread.ManagedThreadId}] Додавання товару: {product.Name}...");
                _products.Add(product);
                Thread.Sleep(200); 
                Console.WriteLine($"[Потік {Thread.CurrentThread.ManagedThreadId}] --> Успішно додано: {product.Name}. Всього товарів: {_products.Count}");
            }
        }

        public void SellLastItemSafe()
        {
            lock (_syncLock)
            {
                Console.WriteLine($"[Потік {Thread.CurrentThread.ManagedThreadId}] Спроба продажу...");
                
                if (_products.Count > 0)
                {
                    var productToSell = _products.Last();
                    _products.Remove(productToSell);
                    Console.WriteLine($"[Потік {Thread.CurrentThread.ManagedThreadId}] <-- Продано: {productToSell.Name}. Залишилось: {_products.Count}");
                }
                else
                {
                    Console.WriteLine($"[Потік {Thread.CurrentThread.ManagedThreadId}] (!) Помилка продажу: Склад порожній!");
                }
                Thread.Sleep(300); 
            }
        }

        public IEnumerator<Product> GetEnumerator()
        {
            lock (_syncLock)
            {
                return new List<Product>(_products).GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Inventory inventory = new Inventory();

            Console.WriteLine("--- Початок симуляції магазину ---");

            Task supplierTask = Task.Run(() =>
            {
                string[] newModels = { "iPhone 16", "Samsung S25", "Pixel 9", "Xiaomi 14", "Motorola Edge" };
                foreach (var model in newModels)
                {
                    var p = new Smartphone(model, 1000m, "Android/iOS");
                    inventory.AddSafe(p);
                    Thread.Sleep(100); 
                }
            });

            Task sellerTask = Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    inventory.SellLastItemSafe();
                    Thread.Sleep(250);
                }
            });

            await Task.WhenAll(supplierTask, sellerTask);

            Console.WriteLine("\n--- Підсумковий стан складу (через ітератор) ---");
            foreach (var item in inventory)
            {
                Console.WriteLine(item);
            }
            
        }
    }
}
