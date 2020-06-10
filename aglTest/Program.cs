using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;


namespace aglTest
{
    class Program
    {
        private static List<T> DownloadSerializedJsonData<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }

                catch (Exception) { }

                // if string with JSON data is not empty, deserialize it to class and return its instance         
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<List<T>>(json_data) : new List<T>();
            }
        }

        private static void PrintAllCatsByGender(List<People> people, string searchStr)
        {
            var queryGenders = from p in people
                               group p by p.gender into newGroup
                               select newGroup;

            foreach (var genderGroup in queryGenders)
            {
                Console.WriteLine($"{genderGroup.Key}");
                var allCats = new List<Pet>();
                foreach (var p in genderGroup)
                {
                    var pets = p.pets;
                    if (pets != null)
                    {
                        var cats = from c in pets
                                   where c.type.ToLower() == searchStr
                                   select c;

                        if (cats != null)
                        {
                            allCats.AddRange(cats);
                        }
                    }
                }

                if (allCats != null)
                {
                    allCats = allCats.OrderBy(p => p.name).ToList();
                    foreach (var c in allCats)
                    {
                        Console.WriteLine($"\t. {c.name}");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var url = "http://agl-developer-test.azurewebsites.net/people.json";

            var people = DownloadSerializedJsonData<People>(url);

            var petType = "cat";

            PrintAllCatsByGender(people, petType);

            Console.ReadLine();
        }
    }
}



