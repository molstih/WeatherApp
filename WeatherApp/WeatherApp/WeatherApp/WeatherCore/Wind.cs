using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.WeatherCore
{
    public class Wind
    {
        public Wind(string _speed, string _degrees)
        {
            Speed = _speed;
            Degrees = _degrees;
        }
        public string Speed { get; set; }
        public string Degrees { get; set; }

    }
}
