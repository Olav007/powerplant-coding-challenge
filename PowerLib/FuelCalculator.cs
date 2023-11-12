using PowerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Powerlib
{
    //using FuelInfo = (string name, string unit, double price);
    internal class FuelCalculator
    {
        //List<FuelInfo> fuels = new List<FuelInfo>();
        Fuels fuels;
        public FuelCalculator(Fuels fuels_arg) //e.g.  ("gas(euro/MWh)", 13.4)
        {
            fuels = fuels_arg;
            //const string pattern = @"(\w+)\((\w+)\)";
            /*
            foreach (var f in fuels_arg)
            {
                Match match = Regex.Match(f.Item1, pattern);//gas(euro/MWh);
                if (match.Success)
                {
                    var name = match.Groups[1].Value.ToLower(); //gas
                    var unit = match.Groups[2].Value; //euro/MWh
                    fuels.Add((name, unit, f.Item2));
                }
                else
                    fuels.Add((f.Item1, "", f.Item2));
            }
            */

        }


        public double merit( string plantType)
        {

            if (plantType.Contains("wind"))
                return 0;
            if( plantType.Contains("gas" ))
                return fuels.Gas;
            return fuels.Kerosine;
        }


    }
}
