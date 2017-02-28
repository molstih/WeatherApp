using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.WeatherCore
{
    public class RequestClass
    {
        public static string GetRequestApi(string city)
        {
            string key = "51ef92cdf49d4917d7aad456373e4def";
            return "http://api.openweathermap.org/data/2.5/weather?q="
                + city + ",&appid=" + key + "&mode=json&units=metric";
        }
        public static async void GetResultInWeather(string req, HttpClient clientHttp, Weather weather)
        {
            weather = await ReturnWeather(req);
        }
        public static async Task<Weather> ReturnWeather(string req)
        {
            var results = await GetDataFromService(req).ConfigureAwait(false);
            if (results["weather"] != null)
            {
                Weather weaTher = new Weather();
                weaTher.Country = results["sys"]["country"].ToString();

                weaTher.Temperature = results["main"]["temp"].ToString();
                weaTher.Pressure = results["main"]["pressure"].ToString();
                weaTher.Humidity = results["main"]["humidity"].ToString();
                string iconId = results["weather"][0]["icon"].ToString();
                weaTher.UriImg = "http://openweathermap.org/img/w/" + iconId + ".png";
                weaTher.Wind = new Wind(results["wind"]["speed"].ToString(), results["wind"]["deg"].ToString());
                weaTher.Cloudiness = results["weather"][0]["description"].ToString();
                weaTher.Location = new Location(results["coord"]["lon"].ToString(), results["coord"]["lat"].ToString());
                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                var interval1 = new TimeSpan(sunrise.Hour, sunrise.Minute, sunrise.Second);
                weaTher.SunRise = interval1.ToString("g");
                var interval2 = new TimeSpan(sunset.Hour, sunset.Minute, sunset.Second);
                weaTher.SunSet = interval2.ToString("g");
                return weaTher;
            }
            else
            {
                return null;
            }
        }
        public static async Task<JContainer> GetDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            JContainer data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = (JContainer)JsonConvert.DeserializeObject(json);
            }
            return data;
        }
    }
}
