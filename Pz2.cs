using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeShopApp
{
    
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

        
        public virtual void Serve()
        {
            Console.WriteLine($"-> Офіціант подає вам: {Name}. Смачного!");
        }
    }

    
    public class Coffee : MenuItem
    {
        public int VolumeML { get; set; } 

        public Coffee(string name, decimal price, int volume) 
            : base(name, price) 
        {
            VolumeML = volume;
        }

        
        public override void DisplayInfo()
        {
            Console.WriteLine($"[Напій] {Name} ({VolumeML} мл) - {Price} грн.");
        }

        
        public override void Serve()
        {
            Console.WriteLine($"-> Бариста готує та подає вам гарячий {Name}. Обережно, гаряче!");
        }
    }

    
    public class Dessert : MenuItem
    {
        public bool IsVegan { get; set; } 

        public Dessert(string name, decimal price, bool isVegan) 
            : base(name, price)
        {
            IsVegan = isVegan;
        }

        
        public override void DisplayInfo()
        {
            string veganInfo = IsVegan ? "(Веганський)" : "(Містить молоко/яйця)";
            Console.WriteLine($"[Їжа] {Name} {veganInfo} - {Price} грн.");
        }
        
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Ласкаво просимо до кав'ярні 'Sharp Coffee' ===\n");

            
            List<MenuItem> orderList = new List<MenuItem>
            {
                new Coffee("Еспресо", 45.00m, 30),
                new Dessert("Чізкейк", 85.00m, false),
                new Coffee("Латте", 60.00m, 300),
                new Dessert("Фруктовий салат", 70.00m, true)
            };

            
            foreach (var item in orderList)
            {
                item.DisplayInfo(); 
                item.Serve();     
                Console.WriteLine("-----------------------------");
            }
            
        }
    }
}
