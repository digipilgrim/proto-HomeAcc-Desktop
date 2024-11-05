using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersAcc
{
    public partial class Bucket
    {
        public int Id { get; set; }

        public string? UnitName { get; set; }

        public double? Score { get; set; }

        public double? Multiplier { get; set; }
    }
}
