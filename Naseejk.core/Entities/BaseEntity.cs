using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naseejk.core.Interfaces;
namespace Naseejk.core.Entities
{
    public abstract class BaseEntity:IBaseEntity
    {
       public long Id { get; set; }
    }
}
