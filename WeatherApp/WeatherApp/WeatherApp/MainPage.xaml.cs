using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using WeatherApp.WeatherCore;
using Xamarin.Forms.Maps;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private HttpClient client;
        private string requestApi;
        private Weather weather;
        private DateTime dateTimeStart;
        public MainPage()
        {
            InitializeComponent();
            client = new HttpClient();
            weather = new Weather();
        }
        #region SearchWeather
        public void SearchButtonClick()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (CitySearchText.Text != null)
                {
                    weather.City = CitySearchText.Text;
                    requestApi = GetRequestApi(weather.City);
                    GetResultInWeather(requestApi, client);
                    if (weather.Location != null)
                    {
                        ReturnRezultForXaml();
                    }
                }
                else CitySearchText.Text = "Lipetsk";
            });

        }
        private void ReturnRezultForXaml()
        {
            TextCity.Text = weather.City;
            TextCountry.Text = weather.Country;
            TempTextBlock.Text = weather.Temperature + "°C";
            SunriseTextBlock.Text = weather.SunRise;
            SunsetTextBlock.Text = weather.SunSet;
            LabelCouldiness.Text = weather.Cloudiness;
            CloudinessTextBlock.Text = weather.Cloudiness;
            LabelGetTimeRequest.Text = "Get at " + DateTime.Now;
            WindTextBlock.Text = weather.Wind.Speed + "m/s \n" + StringDegreesWind(Convert.ToSingle(weather.Wind.Degrees));
            IconImage.Source = new UriImageSource
            {
                Uri = new System.Uri(weather.UriImg)
            };
            MapWeather.MapType = MapType.Street;
            Pin pin = new Pin();
            Location loc = weather.Location;
            Position position = new Position(loc.Latitude, loc.Longtitude);
            pin.Position = position;
            pin.Label = weather.Country;
            MapWeather.Pins.Clear();
            MapWeather.Pins.Insert(0, pin);
            MapWeather.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(15)));
            HumidityTextBlock.Text = weather.Humidity + " %";
            PressureTextBlock.Text = weather.Pressure + " hpa";

        }
        public string StringDegreesWind(float degrees)
        {
            bool north = (degrees >= 0 && degrees < 15) || (degrees > 345),
                northnorthwest = degrees >= 15 && degrees < 30,
                northwest = degrees >= 30 && degrees < 60,
                northwestwest = degrees >= 60 && degrees < 75,
                west = degrees >= 75 && degrees < 105,
                westwestsouth = degrees >= 105 && degrees < 120,
                westsouth = degrees >= 120 && degrees < 150,
                westsouthsouth = degrees >= 150 && degrees < 165,
                south = degrees >= 165 && degrees < 195,
                southsoutheast = degrees >= 195 && degrees < 210,
                southeast = degrees >= 210 && degrees < 240,
                southEastEast = degrees >= 240 && degrees < 255,
                east = degrees >= 255 && degrees < 285,
                easteastnorth = degrees >= 285 && degrees < 300,
                eastnorth = degrees >= 300 && degrees < 330;
            if (north) return "North" + "(" + degrees + ")";
            else if (northnorthwest) return "North-northwest " + "(" + degrees + ")";
            else if (northwest) return "Northwest " + "(" + degrees + ")";
            else if (northwestwest) return "Northwest-west " + "(" + degrees + ")";
            else if (west) return "West " + "(" + degrees + ")";
            else if (westwestsouth) return "Southwest-west " + "(" + degrees + ")";
            else if (westsouth) return "Southwest " + "(" + degrees + ")";
            else if (westsouthsouth) return "South-southwest " + "(" + degrees + ")";
            else if (south) return "South " + "(" + degrees + ")";
            else if (southsoutheast) return "South-southeast " + "(" + degrees + ")";
            else if (southeast) return "Southeast " + "(" + degrees + ")";
            else if (southEastEast) return "Southeast-east " + "(" + degrees + ")";
            else if (east) return "East " + "(" + degrees + ")";
            else if (easteastnorth) return "Northeast-east " + "(" + degrees + ")";
            else if (eastnorth) return "Northeast " + "(" + degrees + ")";
            else return "North-northeast " + "(" + degrees + ")";
        }
        private void ButtonSearch_OnClicked(object sender, EventArgs e)
        {
            SearchButtonClick();
        }
        public string GetRequestApi(string city)
        {
            string key = "51ef92cdf49d4917d7aad456373e4def";
            return "http://api.openweathermap.org/data/2.5/weather?q="
                + city + ",&appid=" + key + "&mode=json&units=metric";
        }
        public async void GetResultInWeather(string req, HttpClient clientHttp)
        {
            weather = await ReturnWeather(req);
        }
        public async Task<Weather> ReturnWeather(string req)
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
        public async Task<JContainer> GetDataFromService(string queryString)
        {
            var response = await client.GetAsync(queryString);

            JContainer data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = (JContainer)JsonConvert.DeserializeObject(json);
            }
            return data;
        }
        #endregion

    }
}
