using Homies.Data.Constants;

namespace Homies.Models;

public class EventDetailsModel : EventViewModel
{
    public EventDetailsModel(int id, 
        string name, 
        string description,
        DateTime createdOn, 
        DateTime start, 
        DateTime end, 
        string type, 
        string organiser) : base(id, name, start, type, organiser)
    {
        Description = description;
        CreatedOn = createdOn.ToString(DataConstants.DateTimeFormat);
        End = end.ToString(DataConstants.DateTimeFormat);
    }

    public string Description { get; set; }

    public string End { get; set; }

    public string CreatedOn { get; set; }
}