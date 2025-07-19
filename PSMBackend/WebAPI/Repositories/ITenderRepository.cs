using System;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.tender;

namespace PSMWebAPI.Repositories;

public interface ITenderRepository
{
    Task<Tender> AddAsync(Tender tender);
    Task<Tender> UpdateAsync(Tender tender);
    Task<Tender> GetByIdAsync(int id);
    Task<IEnumerable<Tender>> GetTendersByProjectIdAsync(int projectId);
}