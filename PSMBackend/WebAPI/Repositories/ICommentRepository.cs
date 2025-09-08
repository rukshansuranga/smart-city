using System;
using PSMModel.Models;
using PSMModel.Enums;

namespace PSMWebAPI.Repositories;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task<Comment?> GetByIdAsync(int id);
    Task<IEnumerable<Comment>> GetByEntityAsync(EntityType entityType, string entityId);
    Task<Comment?> UpdateAsync(Comment comment);
    Task<bool> DeleteAsync(int id);
}
