using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static Homies.Data.Constants.DataConstants.Type;

namespace Homies.Data.Models;

[Comment("Type table")]
public class Type
{
    [Key]
    [Comment("Type Identifier")]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("The name of the Type")]
    public string Name { get; set; } = string.Empty;

    public virtual IEnumerable<Event> Events { get; set; } = new List<Event>();
}