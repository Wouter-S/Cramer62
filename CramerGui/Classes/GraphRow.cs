using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CramerAlexa.Controllers;

namespace CramerAlexa.Classes
{
    public class GraphRow
    {
        public List<Reading> Readings { get; internal set; }
        public int Year { get; internal set; }
    }
}
