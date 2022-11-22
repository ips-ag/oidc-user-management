using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

#pragma warning disable CS8618

namespace IPS.UserManagement.Application.Features.Roles.Models;

[DataContract]
public class RoleQueryModel
{
    [DataMember(Name = "Id")]
    [Required]
    public string Id { get; set; }
}
