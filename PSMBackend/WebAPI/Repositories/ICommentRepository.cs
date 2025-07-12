using System;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task<Comment> GetByIdAsync(int id);
}
