using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Areas.Admin.Models
{
    public class RoleAddViewModel
    {
        [Required(ErrorMessage ="Role Name field cannot be blank.")]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
    }
}
