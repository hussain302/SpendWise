using Application.Features.Expenditure.Bills.Commands.AddEdit;
using Domain.Entities.Expenditure;

namespace Application.Features.Expenditure.Bills.Commands;
public static class BillMapper
{

    public static Bill ToEntity(this AddEditBillCommand source)
        => new()
        {
            Title = source.Title,
            BillAmount = source.BillAmount,
            Description = source.Description,
            OrganizationId = source.OrganizationId,
            PaidById = source.PaidById,
        };
    

    //public static BillDto ToDto(this BillDto source)
    //    => new()
    //    {
    //        Title = source.Title,
    //        BillAmount = source.BillAmount,
    //        Description = source.Description,
    //        OrganizationId = source.OrganizationId,
    //        PaidById = source.PaidById,
    //    };

}
