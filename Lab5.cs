using System;
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

        public Product(string name, decimal price) { Name = name; Price = price; }
        public int CompareTo(Product other) => other == null ? 1 : Price.CompareTo(other.Price);
        public object Clone() => MemberwiseClone();
        public override string ToString() => $"{Name} - {Price}$";
    }

    public class Smartphone : Product
    {
        public string OperatingSystem { get; set; }
        public Smartphone(string name, decimal price, string os) : base(name, price) { OperatingSystem = os; }
        public override string ToString() => $"{Name} ({OperatingSystem}) - {Price}$";
    }

    public class Inventory
    {
        private List<Product> _products = new List<Product>();
        private readonly object _syncLock = new object();

        public void Add(Product product)
        {
            lock (_syncLock)
            {
                _products.Add(product);
                Console.WriteLine($"[Склад] Додано: {product.Name}");
            }
        }

        public List<Product> GetSnapshot()
        {
            lock (_syncLock)
            {
                return new List<Product>(_products);
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Inventory inventory = new Inventory();
            CancellationTokenSource cts = new CancellationTokenSource();

            Console.WriteLine("--- Система запущена ---");
            Console.WriteLine("Натисніть 'C', щоб скасувати аудит, коли він почнеться.\n");

           
            Task restockingTask = RestockInventoryAsync(inventory);

           
            await Task.Delay(2000);

            
            Task auditTask = PerformLongAuditAsync(inventory, cts.Token);

            
            Task.Run(() =>
            {
                if (Console.ReadKey(true).Key == ConsoleKey.C)
                {
                    Console.WriteLine("\n[Увага] Отримано команду скасувати аудит!");
                    cts.Cancel();
                }
            });

            try
            {
               
                await Task.WhenAll(restockingTask, auditTask);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n[Система] Операцію аудиту було примусово зупинено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Помилка] {ex.Message}");
            }
            finally
            {
                cts.Dispose();
            }

            Console.WriteLine("\n--- Роботу програми завершено ---");
            Console.ReadLine();
        }

        static async Task RestockInventoryAsync(Inventory inventory)
        {
            Console.WriteLine("[Постачання] Вантажівка виїхала...");
            var newProducts = new List<Product>
            {
                new Smartphone("iPhone 16", 1300m, "iOS"),
                new Product("MacBook Stand", 80m),
                new Smartphone("Samsung S25", 1200m, "Android"),
                new Product("Mechanical Keyboard", 150m),
                new Smartphone("Google Pixel 9", 900m, "Android")
            };

            foreach (var item in newProducts)
            {
                await Task.Delay(800); 
                inventory.Add(item);
            }
            Console.WriteLine("[Постачання] Розвантаження завершено.");
        }

        
        static async Task PerformLongAuditAsync(Inventory inventory, CancellationToken token)
        {
            Console.WriteLine("\n[Аудит] Починаємо повну перевірку складу...");
            
            var itemsToCheck = inventory.GetSnapshot();

            int progress = 0;
            foreach (var item in itemsToCheck)
            {
                
                token.ThrowIfCancellationRequested();

                await Task.Delay(1500, token); 

                progress++;
                Console.WriteLine($"[Аудит] Перевірено ({progress}/{itemsToCheck.Count}): {item.Name} - ОК");
            }

            Console.WriteLine("[Аудит] Перевірку успішно завершено!");
        }
    }
}
