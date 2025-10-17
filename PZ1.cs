using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication2
{
    public class Transport
    {
        public string Model { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }

        public Transport(string model, string color, int year)
        {
            Model = model;
            Color = color;
            Year = year;
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"ТИП: Транспорт");
            Console.WriteLine($"Модель: {Model}; Колір: {Color}; Рік: {Year}");
        }
    }
    
    public class Car : Transport
    {
        public string FuelType { get; set; }

        public Car(string model, string color, int year, string fuelType)
            : base(model, color, year)
        {
            FuelType = fuelType;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Особливість: Легковий автомобіль (Паливо: {FuelType})");
        }
    }

    public class Truck : Transport
    {
        public float Capacity { get; set; }

        public Truck(string model, string color, int year, float capacity)
            : base(model, color, year)
        {
            Capacity = capacity;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Особливість: Вантажівка (Вантажопідйомність: {Capacity} кг)");
        }
    }

    public class Motorcycle : Transport
    {
        public string Engine { get; set; }

        public Motorcycle(string model, string color, int year, string engine)
            : base(model, color, year)
        {
            Engine = engine;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Особливість: Мотоцикл (Тип двигуна: {Engine})");
        }
    }
    
    public class Bus : Transport
    {
        public int NumberOfSeats { get; set; }

        public Bus(string model, string color, int year, int numberOfSeats)
            : base(model, color, year)
        {
            NumberOfSeats = numberOfSeats;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Особливість: Автобус (Кількість місць: {NumberOfSeats})");
        }
    }

    public class Bicycle : Transport
    {
       
        public bool HasGears { get; set; } 

        public Bicycle(string model, string color, int year, bool hasGears)
            : base(model, color, year)
        {
            HasGears = hasGears;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            string gearStatus = HasGears ? "Має передачі" : "Не має передач";
            Console.WriteLine($"Особливість: Велосипед ({gearStatus})");
        }
    }
    
    
    public class TransportService
    {
        private List<Transport> transports;

        public TransportService()
        {
            transports = new List<Transport>();
        }

        public void AddTransport(Transport t)
        {
            transports.Add(t);
        }

        public void RemoveTransport(string model)
        {
            var transport = transports.Find(t => t.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
            if (transport != null)
            {
                transports.Remove(transport);
                Console.WriteLine($"\n--> Транспорт '{model}' успішно видалено.");
            }
            else
            {
                Console.WriteLine($"\n--> Транспорт '{model}' не знайдено.");
            }
        }

        public void PrintTransports()
        {
            Console.WriteLine("\n*** СПИСОК УСІХ ТРАНСПОРТНИХ ЗАСОБІВ ***");
            foreach (var transport in transports)
            {
                
                transport.PrintInfo(); 
            }
            Console.WriteLine("------------------------------------------");
        }

        public int GetTotal()
        {
            return transports.Count;
        }

        public Transport GetTransport(string model)
        {
            var transport = transports.Find(t => t.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
            if (transport == null)
            {
                Console.WriteLine("Транспорт не знайдено.");
            }
            return transport;
        }
        
    }
    
    internal class PZ1
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            // Створення існуючих об'єктів
            Car car = new Car("BMW X5", "Black", 2010, "Petrol");
            Truck truck = new Truck("Ford F-150", "White", 2015, 1000);
            Motorcycle motorcycle = new Motorcycle("Honda Rebel", "Red", 2018, "Electric");

            // Створення НОВИХ об'єктів
            Bus bus = new Bus("Mercedes Citaro", "Yellow", 2022, 55);
            Bicycle bicycle = new Bicycle("Trek Marlin", "Blue", 2023, true);

            TransportService transportService = new TransportService();
            
            // Додавання всіх об'єктів до сервісу
            transportService.AddTransport(car);
            transportService.AddTransport(truck);
            transportService.AddTransport(motorcycle);
            transportService.AddTransport(bus);      // Новий клас
            transportService.AddTransport(bicycle);  // Новий клас

            // 1. Демонстрація поліморфізму
            transportService.PrintTransports();

            // 2. Виведення загальної кількості
            Console.WriteLine($"\n*** ПІДСУМКИ ***");
            Console.WriteLine($"Загальна кількість транспортних засобів: {transportService.GetTotal()}");
            
            // 3. Пошук існуючих і нових об'єктів
            Transport foundCar = transportService.GetTransport("BMW X5");
            Transport foundBus = transportService.GetTransport("Mercedes Citaro");
            Console.WriteLine($"Знайдено автомобіль: {foundCar?.Model} ({foundCar?.Color})");
            Console.WriteLine($"Знайдено автобус: {foundBus?.Model} ({foundBus?.Color})");

            // 4. Видалення об'єкта
            transportService.RemoveTransport("Ford F-150");
            Console.WriteLine($"Загальна кількість транспортних засобів після видалення: {transportService.GetTotal()}");
            
            // 5. Повторне виведення для перевірки видалення
            transportService.PrintTransports();
        }
    }
}