using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using PSMDataAccess;
using PSMModel.Models;
using PSMModel.Enums;

namespace PSMWebAPI.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        comment.CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments
            .Include(c => c.Client)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CommentId == id);
    }

    public async Task<IEnumerable<Comment>> GetByEntityAsync(EntityType entityType, string entityId)
    {
        return await _context.Comments
            .Include(c => c.Client)
            .Include(c => c.User)
            .Where(c => c.EntityType == entityType && c.EntityId == entityId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Comment?> UpdateAsync(Comment comment)
    {
        var existingComment = await _context.Comments.FindAsync(comment.CommentId);
        if (existingComment == null)
        {
            return null;
        }

        existingComment.Text = comment.Text;
        existingComment.IsPrivate = comment.IsPrivate;
        existingComment.Type = comment.Type;
        existingComment.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        existingComment.UpdatedAt = SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime;

        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(existingComment.CommentId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return false;
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }
}
