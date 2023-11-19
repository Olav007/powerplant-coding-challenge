using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace PowerLib
{
    public class Calculator
    {
        private List<PlantPower> plantsMeritOrder;
        private FuelCalculator fuelCalculator;
        public Calculator(PowerSystem req)
        {
            fuelCalculator = new FuelCalculator(req.fuels);
            var plants = new List<PlantPower>();
            foreach (var plant in req.powerplants)
            {
                var price = fuelCalculator.getPrice(plant.type);
                var cost = (0 == price) ? 0 : price / plant.efficiency;
                plants.Add(new PlantPower { Plant = plant, Cost = cost });
            }
            plantsMeritOrder = plants.OrderBy(plant => plant.Cost).ThenBy(plant => plant.Plant.name).ToList();
        }
        
        private double  reducePower(int toIndex, double power2reduceMin, double power2reduceMax)
        {
            if (toIndex < 0 || power2reduceMin <= 0)
                return 0;
            var plant = plantsMeritOrder[toIndex];

            /* Try to reduce with exact value on this plant*/

            double newPowerMax = plant.p.Value - power2reduceMin;
            double newPowerMin = plant.p.Value - power2reduceMax;
            var power = PlantCalculator.GetPower(plant.Plant, newPowerMax, fuelCalculator.windPercentage);
            if(power.result != ResultType.zero && power.p > newPowerMin)
            { 
                double oldValue = plant.p.Value;
                plant.p = power.p;
                return oldValue - plant.p.Value;
            }

            /* Try turn off this plant */
            var res = reducePower(toIndex - 1, power2reduceMin - plant.p.Value, power2reduceMax - plant.p.Value);
            double reduced = plant.p.Value + res;
            if (power2reduceMin <= reduced && reduced < power2reduceMax)
            {
                plant.p = 0;
                return reduced;
            }

            /*   If we are here, we will reduce power on earier nodes, combined with some reduction on this node */
            double planttMaxReduction = plant.p.Value - power.pmin;
            double plantMaxIncrease = power.pmax - plant.p.Value;
            var reduced_before = reducePower(toIndex - 1, power2reduceMin - planttMaxReduction, power2reduceMax + plantMaxIncrease);
            if (reduced_before > 0)
            {
                double p_prev = plant.p.Value;
                double reduced_below_min = power2reduceMin - reduced_before;
                double reduced_above_max = power2reduceMax - reduced_before;
                if (reduced_below_min > 0)
                {
                    plant.p -= reduced_below_min; 
                }
                else if (reduced_above_max > 0)
                {
                    plant.p -= reduced_above_max;
                }
                return reduced_before + (p_prev - plant.p.Value);
            }
            return 0;
        }
        public dynamic Calculate( double required_load )
        {

            double requiredPowerRemaining = required_load;
            foreach (var plant in plantsMeritOrder)
            {
                var ptup = PlantCalculator.GetPower(plant.Plant, requiredPowerRemaining, fuelCalculator.windPercentage);
                var p = ptup.p;
                if (ptup.result == ResultType.zero && requiredPowerRemaining > 0) 
                {        /* Try to reduce power on previous plants so plant tcan be turned on */
                    double reduced = reducePower(plantsMeritOrder.IndexOf(plant) - 1, ptup.pmin - requiredPowerRemaining, ptup.pmax - requiredPowerRemaining);
                    requiredPowerRemaining += reduced;
                    p = (reduced > 0) ? requiredPowerRemaining : 0;
                }
                plant.p = p;
                requiredPowerRemaining -= p;
            }
            return plantsMeritOrder.Select(plant => new { name = plant.Plant.name, p = Math.Round(plant.p.Value, 1) });
        }
    }
}
