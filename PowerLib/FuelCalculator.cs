using PowerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowerLib
{
    using Fuels = System.Text.Json.JsonElement;
    internal class FuelCalculator
    {
        double? gas;
        double? kerosine;
        double? wind;
        public FuelCalculator( Fuels fuels)
        {
            foreach( var fuel in fuels.EnumerateObject())
            {
                if( fuel.Name.Contains("gas"))
                    gas = fuel.Value.GetDouble();
                if (fuel.Name.Contains("kerosine"))
                    kerosine = fuel.Value.GetDouble();
                if (fuel.Name.Contains("wind"))
                    wind = fuel.Value.GetDouble();
            }

        }
        public (double price, double percentage) getValue( string plantType)
        {
            if (plantType.Contains("wind"))
                return ( 0, wind.Value);
            if( plantType.Contains("gas" ))
                return (gas.Value, 0);
            return (kerosine.Value, 0);
        }
    }
}
