using System;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public interface IClientRepository
{
    Task<Client> AddAsync(Client client);
    Task<Client> UpdateAsync(Client client);
    Task<Client> GetByIdAsync(string id);
}
