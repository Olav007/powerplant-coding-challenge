﻿using System.Collections.Generic;
/* Classes corresponding to request JSON */
namespace PowerLib
{
    using Fuels = System.Text.Json.JsonElement;
    public enum ResultType { zero, partial, success };

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
        public Fuels fuels { get; set; }
        public List<PowerPlant> powerplants { get; set; }
    }
}
