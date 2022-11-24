using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

#pragma warning disable CS8618

namespace IPS.UserManagement.Application.Features.Roles.Models;

public class CreatePermissionAssignmentCommandModel
{
    [DataMember(Name = "permission")]
    [Required]
    [Description("Id of a permission assigned")]
    public string Permission { get; set; }
}
