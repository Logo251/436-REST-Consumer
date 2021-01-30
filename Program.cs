using System;
using System.Net.Http;
using Newtonsoft.Json;


namespace Program_2_REST
{
   class Program
   {
      static void Main(string[] args)
      {
         //Local variables
         string city = null;

         //Get city from user.
         if (args.Length > 0)
         {
            city = args[0];
         }

         JsonResponse openWeatherReport = GetFromAPI("http://api.openweathermap.org/data/2.5/", "weather?q=" + city + "&appid=db5a189c9a489caf9967a896deffc4b3");
         //JsonResponse uvReport = GetFromAPI("http://newsapi.org/v2/", "everything?q=tesla&from=2020-12-30&sortBy=publishedAt&apiKey=bb92a53fa28844fca5131cd14d24d3f6");

         //Report the openWeather component of the output.
         Console.WriteLine("Information about " + city + ":");
         Console.WriteLine("Current weather is " + openWeatherReport.weather[0].description + ".");
         Console.WriteLine("Current temperature is " + (int)((((openWeatherReport.main.temp) - 273.15) * 1.8) + 32) + " F."); //Converts from Kelvin to Fahrenheit. 
         Console.WriteLine("Current wind speed is " + openWeatherReport.wind.speed + " mph.");

         //Report the uvReport component of the output.

      }

      static private JsonResponse GetFromAPI(string baseURL, string extensionURL)
      {
         var client = new HttpClient();

         client.BaseAddress = new Uri(baseURL);
         HttpResponseMessage response = client.GetAsync(extensionURL).Result;

         response.EnsureSuccessStatusCode();
         string result = response.Content.ReadAsStringAsync().Result;
         JsonResponse returnObject = JsonConvert.DeserializeObject<JsonResponse>(result);

         return returnObject;
      }
   }
   public class JsonResponse
   {
      public Coord coord { get; set; }
      public Weather[] weather { get; set; }
      public string _base { get; set; }
      public Main main { get; set; }
      public int visibility { get; set; }
      public Wind wind { get; set; }
      public Clouds clouds { get; set; }
      public int dt { get; set; }
      public Sys sys { get; set; }
      public int timezone { get; set; }
      public int id { get; set; }
      public string name { get; set; }
      public int cod { get; set; }
   }

   public class Coord
   {
      public float lon { get; set; }
      public float lat { get; set; }
   }

   public class Main
   {
      public float temp { get; set; }
      public float feels_like { get; set; }
      public float temp_min { get; set; }
      public float temp_max { get; set; }
      public int pressure { get; set; }
      public int humidity { get; set; }
   }

   public class Wind
   {
      public float speed { get; set; }
      public int deg { get; set; }
   }

   public class Clouds
   {
      public int all { get; set; }
   }

   public class Sys
   {
      public int type { get; set; }
      public int id { get; set; }
      public string country { get; set; }
      public int sunrise { get; set; }
      public int sunset { get; set; }
   }

   public class Weather
   {
      public int id { get; set; }
      public string main { get; set; }
      public string description { get; set; }
      public string icon { get; set; }
   }

}


/*
 * openweathermap API key
 * db5a189c9a489caf9967a896deffc4b3
 */