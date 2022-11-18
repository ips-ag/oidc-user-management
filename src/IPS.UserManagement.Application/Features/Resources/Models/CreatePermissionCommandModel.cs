using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

#pragma warning disable CS8618

namespace IPS.UserManagement.Application.Features.Resources.Models;

[DataContract]
public class CreatePermissionCommandModel
{
    [DataMember(Name = "resource")]
    [Description("Id of exististing Resource entity")]
    [Required]
    public string Resource { get; set; }

    [DataMember(Name = "name")]
    [Required]
    public string Name { get; set; }

    [DataMember(Name = "description")]
    public string? Description { get; set; }
}
