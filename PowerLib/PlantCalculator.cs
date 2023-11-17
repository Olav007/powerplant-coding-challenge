using System;

namespace PowerLib { 


    //using PowerResult = (ResultType result, double p, double pmin);
    public record PowerResult(ResultType result, double p, double pmin, double pmax);

    static public class PlantCalculator {


        static public PowerResult GetPower(PowerPlant plant, double requiredPowerRemaining, double percentage)
        {
            bool isWindmill = plant.type == "windturbine";
            var pmaxEffective = !isWindmill ? plant.pmax : plant.pmax * percentage / 100;
            var pminEffective = isWindmill ? pmaxEffective : plant.pmin;//They are turned on or not, so pmin is pmax
            if (pmaxEffective <= requiredPowerRemaining)
            {
                return new PowerResult(ResultType.partial, pmaxEffective, pminEffective, pmaxEffective);
            }
            else if (requiredPowerRemaining >= pminEffective)
            {
                return new PowerResult(ResultType.success, requiredPowerRemaining, pminEffective, pmaxEffective);
            }
            else
            {
                return new PowerResult(ResultType.zero, 0, pminEffective, pmaxEffective);
            }
        }
    }
}
