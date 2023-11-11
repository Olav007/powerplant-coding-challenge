namespace PowerLib
{


    public class Fuel
    {
        public double Gas { get; set; }
        public double Kerosine { get; set; }
        public double Co2 { get; set; }
        public double Wind { get; set; }
    }

    public class PowerPlant
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Efficiency { get; set; }
        public int Pmin { get; set; }
        public int Pmax { get; set; }
    }

    public class PowerSystem
    {
        public int Load { get; set; }
        public Fuel Fuels { get; set; }
        public List<PowerPlant> PowerPlants { get; set; }
    }

    public class PowerPlantPlan
    {
        public string Name { get; set; }
        public int P { get; set; }
    }

    public class PowerPlan
    {
        public List<PowerPlantPlan> PowerPlantPlans { get; set; }
    }

}

