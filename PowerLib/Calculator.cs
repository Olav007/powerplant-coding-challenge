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
            var plants = new List<PlantPower>();
            var fuelCalculator = new FuelCalculator(req.fuels);
            foreach (var plant in req.powerplants)
            {
                var price = fuelCalculator.getPrice(plant.type);
                var cost = (0 == price) ? 0 : price / plant.efficiency;
                plants.Add(new  PlantPower { Plant = plant, Cost = cost});
            }
            var plantsMeritOrder = plants.OrderBy(plant => plant.Cost).ThenBy(plant => plant.Plant.name).ToList();
            double requiredPowerRemaining = req.load;
            foreach (var plant in plantsMeritOrder)
            {
                var p = PlantCalculator.GetPower(plant.Plant, requiredPowerRemaining, fuelCalculator.windPercentage);
                plant.p = p;
                requiredPowerRemaining -= p;
            }
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                Converters = new List<Newtonsoft.Json.JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() },
                FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue,
                FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Double
            };         
            return Newtonsoft.Json.JsonConvert.SerializeObject(plantsMeritOrder.Select(plant => new { name = plant.Plant.name, p = Math.Round(plant.p.Value, 1) }), settings);
        }
    }
}
