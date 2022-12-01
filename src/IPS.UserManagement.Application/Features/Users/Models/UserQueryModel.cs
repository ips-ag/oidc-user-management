using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

#pragma warning disable CS8618

namespace IPS.UserManagement.Application.Features.Users.Models;

public class UserQueryModel
{
    [DataMember(Name = "id")]
    [Required]
    public string Id { get; set; }

    [DataMember(Name = "userName")]
    [Required]
    public string UserName { get; set; }

    [DataMember(Name = "email")]
    [Required]
    public string Email { get; set; }
}
