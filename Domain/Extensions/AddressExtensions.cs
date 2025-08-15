using Domain.ValueTypes;

namespace Domain.Extensions;
public static class AddressExtensions
{
    public static Address ToDefaultAddress(this Address? address)
    {
        return address ?? new Address(null, null, null, null, null);
    }
}

/*
// Usage
Address? address = null;
Address defaultAddress = address.ToDefaultAddress();
*/