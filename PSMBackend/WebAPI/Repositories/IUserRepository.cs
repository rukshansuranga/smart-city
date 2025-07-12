using System;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetByIdAsync(int id);
}
