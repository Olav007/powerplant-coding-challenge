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
        
        private bool reducePower(int toIndex, double power2reduce, double? minCost)
        {
            if (toIndex < 0 || power2reduce <= 0)
                return false;
            var plant = plantsMeritOrder[toIndex];
            if (minCost.HasValue && minCost.Value > plant.Cost )
            {
                return false; 
            }
            /* Try to reduce with exact value */
            double newPowerThisPlant = plant.p.Value - power2reduce;
            if(newPowerThisPlant >= 0 && (PlantCalculator.GetPower(plant.Plant, newPowerThisPlant, fuelCalculator.windPercentage)).result == ResultType.success  ) 
            {
                plant.p = newPowerThisPlant;
                return true;
            }
            /*   If we are here, we will have to try to reduce power on earlier nodes */  
                   /* First with turning off thisplant */
            if(newPowerThisPlant < 0 && reducePower(toIndex - 1, -power2reduce, minCost))
            {
                plant.p = 0;
                return true;
            }
            return false;
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
                    if(reducePower(plantsMeritOrder.IndexOf(plant) - 1, ptup.pmin - requiredPowerRemaining, plant.Cost))
                        p = ptup.pmin;
                }
                plant.p = p;
                requiredPowerRemaining -= p;
            }
            /* ReturnTypeEncoder with correct formatting */
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                Converters = new List<Newtonsoft.Json.JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() },
                FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue,
                FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Double
            };
            return plantsMeritOrder.Select(plant => new { name = plant.Plant.name, p = Math.Round(plant.p.Value, 1) });
        }
    }
}
