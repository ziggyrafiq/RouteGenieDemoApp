using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure.Entity
{
    public interface IEntityBase
    {

        DateTime? CreatedDate { get; set; }
        DateTime? LastModifiedDate { get; set; }
        bool IsDeleted { get; set; }
        string CreatedBy { get; set; }
        string LastModifiedBy { get; set; }
    }
}
