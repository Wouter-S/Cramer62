using CramerGui.Controllers;
using System.Collections.Generic;

namespace CramerGui.Classes
{
    public class GraphRow
    {
        public List<Reading> Readings { get; internal set; }
        public int Year { get; internal set; }
    }
}
