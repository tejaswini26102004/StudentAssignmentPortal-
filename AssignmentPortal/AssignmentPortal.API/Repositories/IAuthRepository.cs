using AssignmentPortal.API.Models;

namespace AssignmentPortal.API.Repositories
{
    // This defines the contract for authentication database operations
    public interface IAuthRepository
    {
        void RegisterUser(User user);
        User GetUserByEmail(string email);
    }
}