using System;
using PSMDataAccess;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Comment> AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment> GetByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
    }
}
