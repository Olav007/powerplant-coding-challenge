namespace PowerLib
{

    public class Fuels
    {
        public double Gas { get; set; }
        public double Kerosine { get; set; }
        public double Co2 { get; set; }
        public double Wind { get; set; }
    }


    public class PowerPlant
    {
        public string name { get; set; }
        public string type { get; set; }
        public double efficiency { get; set; }
        public int pmin { get; set; }
        public int pmax { get; set; }
    }

    public class PowerSystem
    {
        public int load { get; set; }
        //public List<(string, double)> fuels { get; set; }
        public Fuels fuels { get; set; }
        public List<PowerPlant> powerplants { get; set; }
    }

    public class PowerPlantPlan
    {
        public string name { get; set; }
        public int p { get; set; }
    }

    public class PowerPlan
    {
        public List<PowerPlantPlan> PowerPlantPlans { get; set; }
    }

}
