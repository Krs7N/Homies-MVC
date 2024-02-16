namespace Homies.Data.Constants;

public static class DataConstants
{
    public const string DateTimeFormat = "yyyy-MM-dd H:mm";

    public static class Event
    {

        public const int NameMinLength = 5;
        public const int NameMaxLength = 20;

        public const int DescriptionMinLength = 15;
        public const int DescriptionMaxLength = 150;
    }

    public static class Type
    {
        public const int NameMinLength = 5;
        public const int NameMaxLength = 15;
    }

    public static class ErrorMessages
    {
        public const string StringLengthError = "The {0} must be between {2} and {1} characters long.";
        public const string DateFormatError = "The {0} date must be in a valid date and time format.";
        public const string InvalidPeriod = "The {0} date must be after the {1} date.";
    }
}