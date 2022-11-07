using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomCollection.Tests
{
    // Do not change the name of this class
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class EventPrice
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int Price { get; set; }
    }
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    //create a dictionary/database/dataset to hold city_pairs and distance
    public static class DataStorage
    {
        public static Dictionary<string, int> city_pairs { get; set; }
    }

    public class Solution
    {
        static void Main(string[] args)
        {
            var events = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
             };
            var customer = new Customer { Name = "John Smith", City = "New York" };
            //Question 1
            GetEventsInCity(customer, events);
            //Question 2
            GetEventsByParameter(customer, events);
            Console.ReadLine();
        }

        static void GetEventsInCity(Customer cust, List<Event> eve)
        {
            List<Event> custEvents = eve.Where(x => x.City == cust.City).ToList();
            foreach (var item in custEvents)
            {
                AddToEmail(cust, item);
            }
        }

        static void GetEventsByParameter(Customer cust, List<Event> events, string sortParameter = "distance")
        {
            if (sortParameter == "price")
            {
                GetEventsByAfforability(cust, events);
            }
            else
            if (sortParameter == "distance")
            {
                GetEventsCloseToCity(cust, events);
            }
            //else if for other conditions.
            //I could use switch case, but I prefer if else considering there's no real difference in performance
        }

        static void GetEventsCloseToCity(Customer cust, List<Event> events)
        {
            int eventCount = 0;
            //first get all events occuring in customer's city
            var custumerCityEvents = events.Where(x => x.City == cust.City).ToList();
            for (int i = 0; i < custumerCityEvents.Count; i++)
            {
                AddToEmail(cust, custumerCityEvents[i]);
                eventCount++;
                if (eventCount == 5)
                    return;
            }
            //if we don't have 5 events, then get all distinct cities with events order than the customer's city
            var cities = events.Where(x => x.City != cust.City).Select(x => x.City).Distinct().ToList();
            Dictionary<string, int> cityDistance = new Dictionary<string, int>();
            //calculate the distance of each city from customer's city
            foreach (var item in cities)
            {
                int distance = GetDistance(cust.City, item);
                cityDistance.Add(item, distance);
            }
            //order by distance
            var sortedCities = cityDistance.OrderBy(x => x.Value).ToList();
            //create a loop to add 5 closest events. 

            for (int i = 0; i < sortedCities.Count; i++)
            {
                var cityEvents = events.Where(x => x.City == sortedCities[i].Key).ToList();
                for (int j = 0; j < cityEvents.Count; j++)
                {
                    AddToEmail(cust, cityEvents[j]);
                    eventCount++;
                    if (eventCount == 5)
                        return;
                }
            }
        }

        static void GetEventsByAfforability(Customer cust, List<Event> events)
        {
            List<EventPrice> eventPrices = new List<EventPrice>();
            foreach (var item in events)
            {
                EventPrice eventPrice = new EventPrice();
                eventPrice.City = item.City;
                eventPrice.Name = item.Name;
                eventPrice.Price = GetPrice(item);
                eventPrices.Add(eventPrice);
            }
            var sortedCities = eventPrices.OrderBy(x => x.Price).ToList().Take(5);

            foreach (var item in sortedCities)
            {
                Event eve = new Event();
                eve.City = item.City;
                eve.Name = item.Name;
                AddToEmail(cust, eve);
            }
        }


        static void ComputeCityDistance(List<Event> events)
        {

            //get all distinct cities
            var cities = events.Select(x => x.City).Distinct().ToList();
   
            for(int i=0;i<cities.Count;i++)
            {
                for (int j=i+1;j<cities.Count;j++)
                {
                    string city1 = cities[i];
                    string city2 = cities[j];
                    //compare cities and concat in a sorted order to avoid duplicated cities e.g CaliforniaTexas and TexasCalifornia
                    string cityConcat = string.Compare(city1, city2, StringComparison.Ordinal) > 0 ? city1 + city2 : city2 + city1;
                    if (!DataStorage.city_pairs.ContainsKey(cityConcat))
                    {
                        try
                        {
                            //add city_pair and distance to dictionary
                            DataStorage.city_pairs.Add(cityConcat,GetDistance(city1,city2));
                        }
                        catch (Exception e)
                        {
                            //catch some exception
                        }     
                    }
                }
            }
        }

        static int GetComputedCityDistance(string city1, string city2)
        {
            string cityConcat = string.Compare(city1, city2, StringComparison.Ordinal) > 0 ? city1 + city2 : city2 + city1;
            if (DataStorage.city_pairs.ContainsKey(cityConcat))
                return DataStorage.city_pairs[cityConcat];
            else
            {
                int distance= GetDistance(city1, city2);
                DataStorage.city_pairs.Add(cityConcat, distance);
                return distance;
            }
        }

        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}
/*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/
