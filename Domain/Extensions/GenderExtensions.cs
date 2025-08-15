using Domain.Enums;

namespace Domain.Extensions;
public static class GenderExtensions
{
    public static string GetStringValue(this Gender gender)
    {
        return gender switch
        {
            Gender.Male => "Male",
            Gender.Female => "Female",
            Gender.NotSpecified => "Not Specified",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };
    }
}

/*
Usage:
    Gender gender = Gender.Male;
    string genderString = gender.GetStringValue();
    Console.WriteLine(genderString); // Output: "Male"
*/