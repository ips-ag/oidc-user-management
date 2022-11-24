using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

#pragma warning disable CS8618

namespace IPS.UserManagement.Application.Features.Users.Models;

[DataContract]
public class CreateRoleAssignmentCommandModel
{
    [DataMember(Name = "roleId")]
    [Required]
    public string RoleId { get; set; }
}
