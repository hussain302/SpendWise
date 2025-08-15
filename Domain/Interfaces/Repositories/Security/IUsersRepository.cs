using Domain.Entities.Security;
using Domain.Interfaces.Repositories;

namespace Domain.Interfaces.Repositories.Security;
public interface IUsersRepository : IRepository<User, Guid>
{
    //Task<bool> IsUserExistsByUserNameAsync(string username,
    //   CancellationToken cancellationToken = default);

    //Task<UserViewModel> GetLoginUser(string username, string password,
    //    CancellationToken cancellationToken = default);

    //Task<bool> IsUserExistsByEmailAsync(string email,
    //    CancellationToken cancellationToken = default);


    //Task UpdateUserAsync(UpdateUserCommand updateUserCommand,
    //CancellationToken cancellationToken = default);
}
