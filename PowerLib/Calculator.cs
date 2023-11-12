using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLib
{
    static public class Calculator
    {
        public static dynamic Calculate(PowerSystem req)
        {
            var plants = new List<(PowerPlant plant, double cost, double percentage)>();
            var fuelCalculator = new FuelCalculator(req.fuels);
            foreach (var plant in req.powerplants)
            {
                var fuelValue = fuelCalculator.getValue(plant.type);
                var price = fuelValue.price;
                var cost = (0 == price) ? 0 : price / plant.efficiency;
                plants.Add((plant, cost, fuelValue.percentage));
            }
            var plantsMeritOrder = plants.OrderBy(plant => plant.cost).ThenBy(plant => plant.plant.name).ToList();
            var remainingPower = req.load;
            var res = new List<(double p, PowerPlant plant)>();
            double requiredPowerRemaining = req.load;
            foreach( var plant  in plantsMeritOrder)
            {
                double p;
                var pmax = plant.cost > 0 ? plant.plant.pmax : plant.plant.pmax * plant.percentage/100;
                var pmin = plant.cost > 0 ? plant.plant.pmin : plant.plant.pmin * plant.percentage/100;
                if ( requiredPowerRemaining < pmin)
                {
                    p = 0;
                }
                else     if ( pmax < requiredPowerRemaining)
                {
                    p = pmax;
                }
                else if(requiredPowerRemaining > pmin)
                {
                    p = requiredPowerRemaining;
                }
                else
                {
                    p = pmin;
                }  
                res.Add((p, plant.plant));
                requiredPowerRemaining -= p;
            }
            return res.Select(plant => new { name = plant.plant.name, p = plant.p.ToString("F1") }).ToList();
        }
    }
}
