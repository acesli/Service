using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechArt.Base.Interfaces
{
    public interface IJob
    {
        Guid ID { get; set; }

        Object Data { get; set; }
    }
}
