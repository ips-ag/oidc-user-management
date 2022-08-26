using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

#pragma warning disable CS8618

namespace IPS.UserManagement.Application.Features.Resources.Models;

[DataContract]
public class CreateResourceCommandModel
{
    [DataMember(Name = "name")]
    [Required]
    public string Name { get; set; }

    [DataMember(Name = "description")]
    public string? Description { get; set; }

    [DataMember(Name = "location")]
    [Required]
    public string Location { get; set; }
}
