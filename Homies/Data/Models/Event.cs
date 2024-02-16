using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Homies.Data.Constants.DataConstants.Event;

namespace Homies.Data.Models;

[Comment("Event table")]
public class Event
{
    [Key]
    [Comment("Event Identifier")]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("The name of the Event")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(DescriptionMaxLength)]
    [Comment("The description of the Event")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Comment("The identifier of the Organiser of the Event")]
    public string OrganiserId { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(OrganiserId))]
    public IdentityUser Organiser { get; set; }

    [Required]
    [Comment("The date and time when the Event was created")]
    public DateTime CreatedOn { get; set; }

    [Required]
    [Comment("The date and time when the Event starts")]
    public DateTime Start { get; set; }

    [Required]
    [Comment("The date and time when the Event ends")]
    public DateTime End { get; set; }

    [Required]
    [Comment("The identifier of the Type of the Event")]
    public int TypeId { get; set; }

    [Required]
    [ForeignKey(nameof(TypeId))]
    public Type Type { get; set; }

    public virtual IEnumerable<EventParticipant> EventsParticipants { get; set; } = new List<EventParticipant>();
}