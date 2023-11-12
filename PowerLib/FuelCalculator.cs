using PowerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Powerlib
{
    internal static class FuelCalculator
    {
        static public double price( Fuels fuels,string plantType)
        {
            if (plantType.Contains("wind"))
                return 0;
            if( plantType.Contains("gas" ))
                return fuels.Gas;
            return fuels.Kerosine;
        }
    }
}
