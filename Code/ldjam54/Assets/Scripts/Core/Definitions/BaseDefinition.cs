using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Definitions
{
    public class BaseDefinition
    {
        public String Reference { get; set; }
        public Boolean IsReferenced { get; set; }
        public Boolean IsValueOverride { get; set; }
    }
}
