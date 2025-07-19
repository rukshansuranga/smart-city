using System;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public interface IMiscRepository
{
    Task<IEnumerable<Company>> GetCompanies();
    Task<Company> GetCompanyByIdAsync(int id);
}
