using System;
using System.Collections.Generic;

namespace api.Data
{
   // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CirculationSupport
    {
        public List<Item> items { get; set; }
      
    }

    public class Iabp
    {
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string content { get; set; }
      
    }

    public class Pmwires
    {
        public List<Item> items { get; set; }
    }

    public class Root
    {
        public int hospitalNo { get; set; }
        public CirculationSupport circulation_support { get; set; }
        public Iabp iabp { get; set; }
        public Pmwires pmwires { get; set; }
    }

    }
