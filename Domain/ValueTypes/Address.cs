namespace Domain.ValueTypes;
public record Address(int? HouseNo, 
    string? StreetName, 
    string? City,
    string? State, 
    string? Country);