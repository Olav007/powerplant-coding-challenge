using System;
namespace PowerLib
{

    static public class PlantCalculator
    {
        static public double GetPower(PowerPlant plant, double requiredPowerRemaining, double percentage)
        {
            bool isWindmill = plant.type == "windturbine";
            var pmax = !isWindmill ? plant.pmax : plant.pmax * percentage / 100;
            var pmin = isWindmill ? pmax : plant.pmin;//They are turned on or not, so pmin is pmax
            if (pmax <= requiredPowerRemaining)
            {
                return pmax;
            }
            else if (requiredPowerRemaining >= pmin)
            {
                return requiredPowerRemaining;
            }
            else
            {
                return 0;
            }
        }
    }
}

