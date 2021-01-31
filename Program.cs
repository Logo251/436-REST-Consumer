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
         JsonResponse openWeatherReport = new JsonResponse();
         JsonResponse uvReport = new JsonResponse();

         //Get city from user.
         if (args.Length > 0)
         {
            city = args[0];
         }

         openWeatherReport = GetFromAPI("http://api.openweathermap.org/data/2.5/", "weather?q=" + city + "&appid=db5a189c9a489caf9967a896deffc4b3", null, null);
         //JsonResponse newsReport = GetFromAPI("http://newsapi.org/v2/", "everything?q" + city + "&from=2020-12-30&sortBy=publishedAt&apiKey=bb92a53fa28844fca5131cd14d24d3f6", null, null);
         //Need an if loop here in case the above call fails, as this call depends on it succeeding.
         if(openWeatherReport.statusCode == 200)
         {
            uvReport = GetFromAPI("https://api.openuv.io/api/v1/uv", "uv?lat=" + openWeatherReport.coord.lat + "&lng=" + openWeatherReport.coord.lon, "x-access-token", "0e2f0178c8d10d971963fd75867a2772");
         }

         //Check if we got valid response from openWeatherMap.
         if (openWeatherReport.statusCode == 200) 
         {
            //Report the openWeather component of the output.
            Console.WriteLine("Information about " + city + ":");
            Console.WriteLine("Current weather is " + openWeatherReport.weather[0].description + ".");
            Console.WriteLine("Current temperature is " + (int)((((openWeatherReport.main.temp) - 273.15) * 1.8) + 32) + " F."); //Converts from Kelvin to Fahrenheit. 
            Console.WriteLine("Current wind speed is " + openWeatherReport.wind.speed + " mph.");
         }
         //If we did not, report.
         else
         {
            Console.WriteLine("An error occured with retrieving data from the APIs.");
            if(openWeatherReport.statusCode == 404)
            {
               Console.WriteLine("OpenWeatherMap returned a 404. Perhaps the city is wrong?");
            }
            else
            {
               Console.Write("OpenWeatherMap returned error code " + openWeatherReport.statusCode + '.');
            } 
         }

         //Check if we got valid responses.
         if(uvReport.statusCode == 200)
         {
            //Report the uvReport component of the output.
            Console.WriteLine("UV index is " + uvReport.result.uv + " with a max of " + uvReport.result.uv_max + " today.");
            Console.WriteLine();
         }
         //If we did not, report.
         else
         {
            if(uvReport.statusCode == -1)
            {
               Console.WriteLine("OpenUV failed due to requiring data from OpenWeatherMap, which failed.");
            }
            else
            {
               Console.Write("OpenUV returned error code " + uvReport.statusCode + '.');
            }
         }
      }

      static private JsonResponse GetFromAPI(string baseURL, string extensionURL, string additionalHeadersBearer, string additionalHeaderAuth)
      {
         var client = new HttpClient();
         JsonResponse returnObject = new JsonResponse();

         client.BaseAddress = new Uri(baseURL);
         if (additionalHeadersBearer != null && additionalHeaderAuth != null)
         {
            client.DefaultRequestHeaders.Add(additionalHeadersBearer, additionalHeaderAuth);
         }
         HttpResponseMessage response = client.GetAsync(extensionURL).Result;

         //Handles when the source is not found.
         try { response.EnsureSuccessStatusCode(); }
         catch(Exception e)
         {
            if((int)response.StatusCode == 404)
            {
               returnObject.statusCode = (int)response.StatusCode;
               return returnObject;
            }
         }
         string result = response.Content.ReadAsStringAsync().Result;
         returnObject = JsonConvert.DeserializeObject<JsonResponse>(result);
         returnObject.statusCode = (int)response.StatusCode;
         return returnObject;
      }
}
public class JsonResponse
{
   public int statusCode = -1;
   public Result result { get; set; }
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

   public class Result
   {
      public int uv { get; set; }
      public DateTime uv_time { get; set; }
      public float uv_max { get; set; }
      public DateTime uv_max_time { get; set; }
      public float ozone { get; set; }
      public DateTime ozone_time { get; set; }
      public Safe_Exposure_Time safe_exposure_time { get; set; }
      public Sun_Info sun_info { get; set; }
   }

   public class Safe_Exposure_Time
   {
      public object st1 { get; set; }
      public object st2 { get; set; }
      public object st3 { get; set; }
      public object st4 { get; set; }
      public object st5 { get; set; }
      public object st6 { get; set; }
   }

   public class Sun_Info
   {
      public Sun_Times sun_times { get; set; }
      public Sun_Position sun_position { get; set; }
   }

   public class Sun_Times
   {
      public DateTime solarNoon { get; set; }
      public DateTime nadir { get; set; }
      public DateTime sunrise { get; set; }
      public DateTime sunset { get; set; }
      public DateTime sunriseEnd { get; set; }
      public DateTime sunsetStart { get; set; }
      public DateTime dawn { get; set; }
      public DateTime dusk { get; set; }
      public DateTime nauticalDawn { get; set; }
      public DateTime nauticalDusk { get; set; }
      public DateTime nightEnd { get; set; }
      public DateTime night { get; set; }
      public DateTime goldenHourEnd { get; set; }
      public DateTime goldenHour { get; set; }
   }

   public class Sun_Position
   {
      public float azimuth { get; set; }
      public float altitude { get; set; }
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