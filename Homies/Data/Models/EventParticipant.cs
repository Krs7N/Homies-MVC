using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Homies.Data.Models;

[Comment("EventParticipant table, mapping table between the participants that are in current event")]
public class EventParticipant
{
    [Required]
    [Comment("User Identifier")]
    public string HelperId { get; set; } = string.Empty;

    [ForeignKey(nameof(HelperId))]
    public IdentityUser Helper { get; set; }

    [Required]
    [Comment("Event Identifier")]
    public int EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    public Event Event { get; set; }
}