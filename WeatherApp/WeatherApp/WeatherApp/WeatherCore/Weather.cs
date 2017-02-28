using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.WeatherCore
{
    public class Weather
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
        public string Humidity { get; set; }
        public string Cloudiness { get; set; }
        public string UriImg { get; set; }
        public Location Location { get; set; }
        public Wind Wind { get; set; }
        public string SunRise { get; set; }
        public string SunSet { get; set; }
    }
}
