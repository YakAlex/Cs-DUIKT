using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace CoffeeShopEvents
{
    
    public class PriceArgs : EventArgs
    {
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }
        public PriceArgs(decimal oldPrice, decimal newPrice)
        {
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }
    
    public abstract class MenuItem
    {
        public string Name { get; set; }
        
        private decimal _price;

        
        public event EventHandler<PriceArgs> PriceChanged;

        public decimal Price
        {
            get => _price;
            set
            {
                
                if (_price != value)
                {
                    decimal old = _price;
                    _price = value;

                    OnPriceChanged(new PriceArgs(old, value));
                }
            }
        }

        public MenuItem(string name, decimal price)
        {
            Name = name;
            _price = price; 
        }

        public abstract void DisplayInfo();

        
        protected virtual void OnPriceChanged(PriceArgs e)
        {
            
            if (PriceChanged == null) return;
            
            var subscribers = PriceChanged.GetInvocationList();

            foreach (Delegate sub in subscribers)
            {
                try
                {
                    sub.DynamicInvoke(this, e);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[СИСТЕМНА ПОМИЛКА] Підписник не зміг обробити подію: {ex.InnerException?.Message}");
                    Console.ResetColor();
                }
            }
        }
    }

    public class Coffee : MenuItem
    {
        public Coffee(string name, decimal price) : base(name, price) { }
        public override void DisplayInfo() => Console.WriteLine($"Кава: {Name} - {Price} грн.");
    }

    public class Dessert : MenuItem
    {
        public Dessert(string name, decimal price) : base(name, price) { }
        public override void DisplayInfo() => Console.WriteLine($"Десерт: {Name} - {Price} грн.");
    }

    public class MenuRegistry
    {
        private List<MenuItem> _items = new List<MenuItem>();

        public event Action<MenuItem> ItemAdded;

        public void AddItem(MenuItem item)
        {
            _items.Add(item);
            Console.WriteLine($"[Реєстр] Додано в базу: {item.Name}");

            ItemAdded?.Invoke(item);
        }

        public List<MenuItem> GetAll() => _items;
    }
    
    public class AdminLogger
    {
        
        public void OnItemAddedToMenu(MenuItem item)
        {
            Console.WriteLine($"[LOG] Адмін бачить новий товар: {item.Name}. Треба перевірити наявність продуктів.");
            
            item.PriceChanged += OnItemPriceChanged;
        }

        
        private void OnItemPriceChanged(object sender, PriceArgs e)
        {
            var item = (MenuItem)sender;
            Console.WriteLine($"[LOG] УВАГА! Ціна на '{item.Name}' змінилася: {e.OldPrice} -> {e.NewPrice}");
        }
    }

    
    public class DigitalBoard
    {
        public void SubscribeTo(MenuRegistry registry)
        {
            registry.ItemAdded += item => 
            {
                item.PriceChanged += UpdateScreen;
            };
        }

        private void UpdateScreen(object sender, PriceArgs e)
        {
            var item = (MenuItem)sender;
            
            
            if (e.NewPrice > 1000)
            {
                throw new Exception("Екран перегорів від такої ціни!");
            }

            Console.WriteLine($"[ТАБЛО] Клієнти бачать оновлення: {item.Name} тепер коштує {e.NewPrice} грн!");
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Кав'ярня 4.0: Події та безпека ===\n");

            
            MenuRegistry registry = new MenuRegistry();
            AdminLogger admin = new AdminLogger();
            DigitalBoard board = new DigitalBoard();

            
            registry.ItemAdded += admin.OnItemAddedToMenu;
            
            
            board.SubscribeTo(registry);

            
            Console.WriteLine("--- Додавання товарів ---");
            Coffee espresso = new Coffee("Еспресо", 40);
            Dessert cake = new Dessert("Торт Золотий", 150);
            
            registry.AddItem(espresso); 
            registry.AddItem(cake);   

            Console.WriteLine("\n--- Зміна цін (Тест подій) ---");
            
            espresso.Price = 45; 

            Console.WriteLine("\n--- Тест на стійкість (Exception Handling) ---");
            
            cake.Price = 1200; 

            Console.WriteLine("\n--- Програма продовжує працювати! ---");
            Console.WriteLine("Кінець роботи.");
            Console.ReadKey();
        }
    }
}
