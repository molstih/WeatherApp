using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.WeatherCore
{
    public class Location
    {
        public Location(string lon, string lat)
        {
            Longtitude = Convert.ToSingle(lon);
            Latitude = Convert.ToSingle(lat);
        }
        public float Longtitude { get; set; }
        public float Latitude { get; set; }
    }
}
