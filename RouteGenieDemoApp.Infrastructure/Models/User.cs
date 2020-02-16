using RouteGenieDemoApp.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure.Models
{
    public class User : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserID { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }


       
        [StringLength(255)]
        public string FirstName { get; set; }

        
        [StringLength(255)]
        public string LastName { get; set; }

     
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(255)]
        public string Salt { get; set; }


        public bool IsActive { get; set; }

        public virtual Role Roles { get; set; }
    }
}
