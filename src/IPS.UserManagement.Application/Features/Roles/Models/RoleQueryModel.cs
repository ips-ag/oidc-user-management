using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

#pragma warning disable CS8618

namespace IPS.UserManagement.Application.Features.Roles.Models;

[DataContract]
public class RoleQueryModel
{
    [DataMember(Name = "id")]
    [Required]
    public string Id { get; set; }

    [DataMember(Name = "name")]
    [Required]
    public string Name { get; set; }

    [DataMember(Name = "description")]
    public string? Description { get; set; }
}
