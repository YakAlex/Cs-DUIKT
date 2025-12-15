using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeShopAppExtended
{
    
    public abstract class MenuItem
    {
        public string Name { get; set; }
        public decimal Price { get; protected set; } // protected set - ціну можна змінити тільки всередині класу

        public MenuItem(string name, decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Ціна не може бути від'ємною!");

            Name = name;
            Price = price;
        }

        public abstract void DisplayInfo();

        public virtual void Serve()
        {
            Console.WriteLine($"-> Офіціант подає вам: {Name}.");
        }
        
        public virtual void Consume()
        {
            Console.WriteLine($"* Ви споживаєте {Name}. Смачно!");
        }
        
        public virtual void ApplyDiscount(decimal percentage)
        {
            
            if (percentage < 0 || percentage > 100)
            {
                
                throw new ArgumentOutOfRangeException("percentage", "Знижка має бути від 0 до 100 відсотків.");
            }

            decimal discountAmount = Price * (percentage / 100.0m);
            Price -= discountAmount;
            Console.WriteLine($"[Акція] На {Name} застосовано знижку {percentage}%. Нова ціна: {Price:0.00} грн.");
        }
    }

    
    public class Coffee : MenuItem
    {
        public int VolumeML { get; set; }

        public Coffee(string name, decimal price, int volume) : base(name, price)
        {
            VolumeML = volume;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[Напій] {Name} ({VolumeML} мл) - {Price:0.00} грн.");
        }

        public override void Serve()
        {
            Console.WriteLine($"-> Бариста наливає {Name} у фірмовий стаканчик. Обережно, гаряче!");
        }
        
        public override void Consume()
        {
            Console.WriteLine($"* Ви п'єте ароматну каву {Name}, вона бадьорить!");
        }
    }
    
    public class Dessert : MenuItem
    {
        public bool IsVegan { get; set; }

        public Dessert(string name, decimal price, bool isVegan) : base(name, price)
        {
            IsVegan = isVegan;
        }

        public override void DisplayInfo()
        {
            string type = IsVegan ? "Веган" : "Класичний";
            Console.WriteLine($"[Десерт] {Name} ({type}) - {Price:0.00} грн.");
        }
        
        public override void ApplyDiscount(decimal percentage)
        {
            if (percentage > 20)
            {
                throw new Exception($"На свіжі десерти ({Name}) не можна робити знижку більше 20%!");
            }
            
            base.ApplyDiscount(percentage);
        }
    }
    
    public class Tea : MenuItem
    {
        public string TeaType { get; set; }

        public Tea(string name, decimal price, string type) : base(name, price)
        {
            TeaType = type;
        }

        public override void DisplayInfo()
        {
             Console.WriteLine($"[Чай] {Name} (Сорт: {TeaType}) - {Price:0.00} грн.");
        }

        public override void Consume()
        {
             Console.WriteLine($"* Ви робите ковток чаю {Name}. Дуже заспокоює.");
        }
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Кав'ярня 2.0: Обробка виключень ===\n");

            List<MenuItem> order = new List<MenuItem>
            {
                new Coffee("Американо", 40.00m, 200),
                new Dessert("Наполеон", 90.00m, false),
                new Tea("Альпійський луг", 35.00m, "Трав'яний")
            };
            
            Console.WriteLine("--- Ваше замовлення ---");
            foreach (var item in order)
            {
                item.DisplayInfo();
                item.Serve();
                item.Consume();
                Console.WriteLine();
            }

            Console.WriteLine("--- Спроба застосувати знижки (Try-Catch) ---");

            
            ApplySafeDiscount(order[0], 10); // 10% - все ок
            
            ApplySafeDiscount(order[2], 150); // Помилка ArgumentOutOfRangeException
            
            ApplySafeDiscount(order[1], 50); // Помилка Exception (бізнес-логіка десерту)

            Console.ReadKey();
        }

        
        static void ApplySafeDiscount(MenuItem item, decimal percent)
        {
            try
            {
                Console.Write($"Спроба дати знижку {percent}% на {item.Name}... ");
                item.ApplyDiscount(percent); 
            }
            catch (ArgumentOutOfRangeException ex)
            {
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nПОМИЛКА ДАНИХ: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nБІЗНЕС-ПОМИЛКА: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("-----------------------------");
            }
        }
    }
}
