using System.ComponentModel.DataAnnotations;
using static Homies.Data.Constants.DataConstants;

namespace Homies.Models;

public class EventFormModel
{
    [Required]
    [StringLength(Event.NameMaxLength, MinimumLength = Event.NameMinLength, ErrorMessage = ErrorMessages.StringLengthError)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(Event.DescriptionMaxLength, MinimumLength = Event.DescriptionMinLength, ErrorMessage = ErrorMessages.StringLengthError)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Start { get; set; } = string.Empty;

    [Required]
    public string End { get; set; } = string.Empty;

    [Required]
    public int TypeId { get; set; }

    public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
}