using System;
using System.Collections.Generic;

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
            Console.WriteLine($"Model: {Model}; Color: {Color}; Year: {Year}");
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
            Console.WriteLine($"FuelType: {FuelType}");
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
            Console.WriteLine($"Capacity: {Capacity}");
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
            Console.WriteLine($"Engine: {Engine}");
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
            var transport = transports.Find(t => t.Model == model);
            if (transport != null)
            {
                transports.Remove(transport);
                Console.WriteLine($"Transport {model} removed.");
            }
            else
            {
                Console.WriteLine($"Transport {model} not found.");
            }
        }

        public void PrintTransports()
        {
            foreach (var transport in transports)
            {
                transport.PrintInfo();
            }
        }

        public int GetTotal()
        {
            return transports.Count;
        }

        public Transport GetTransport(string model)
        {
            var transport = transports.Find(t => t.Model == model);
            if (transport == null)
            {
                Console.WriteLine("Transport not found");
            }
            return transport;
        }
        
    }
    
    internal class PZ1
    {
        public static void Main(string[] args)
        {
            Car car = new Car("BMW", "Black", 2010, "Petrol");
            Truck truck = new Truck("Ford", "White", 2015, 1000);
            Motorcycle motorcycle = new Motorcycle("Honda", "Red", 2018, "Electric");

            TransportService transportService = new TransportService();
            transportService.AddTransport(car);
            transportService.AddTransport(truck);
            transportService.AddTransport(motorcycle);

            transportService.PrintTransports();

            Console.WriteLine($"Total transports: {transportService.GetTotal()}");
            Console.WriteLine($"Found transport: {transportService.GetTransport("BMW")?.Model}");

            transportService.RemoveTransport("Ford");
            Console.WriteLine($"Total transports after removal: {transportService.GetTotal()}");
        }
    }
}
