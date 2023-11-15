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
        public double getPrice( string plantType)
        {
            if (plantType.Contains("wind"))
                return 0;
            if( plantType.Contains("gas" ))
                return gas.Value;
            return kerosine.Value;
        }
        public double  windPercentage { get { return wind.Value; } }


    }
}
