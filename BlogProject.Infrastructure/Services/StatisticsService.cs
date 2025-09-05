using BlogProject.Application.DTOs;
using BlogProject.Application.Interfaces;
using BlogProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly BlogDbContext _context;

        public StatisticsService(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<StatisticsDto> GetStatisticsAsync()
        {
            var statistics = new StatisticsDto();

            // Get basic counts
            statistics.TotalPosts = await _context.Posts.CountAsync(p => !p.IsDeleted);
            statistics.TotalUsers = await _context.Users.CountAsync(u => !u.IsDeleted);
            statistics.TotalComments = await _context.Comments.CountAsync(c => !c.IsDeleted);
            statistics.TotalViews = await _context.Posts.Where(p => !p.IsDeleted).SumAsync(p => p.ViewCount);

            // Calculate averages
            statistics.AverageViewsPerPost = statistics.TotalPosts > 0 
                ? statistics.TotalViews / statistics.TotalPosts 
                : 0;

            statistics.AverageCommentsPerPost = statistics.TotalPosts > 0 
                ? statistics.TotalComments / statistics.TotalPosts 
                : 0;

            // Get posts by category
            var postsByCategory = await _context.Posts
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.Category.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            statistics.PostsByCategory = postsByCategory.ToDictionary(x => x.Category, x => x.Count);

            // Get views by category
            var viewsByCategory = await _context.Posts
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.Category.Name)
                .Select(g => new { Category = g.Key, Views = g.Sum(p => p.ViewCount) })
                .ToListAsync();

            statistics.ViewsByCategory = viewsByCategory.ToDictionary(x => x.Category, x => x.Views);

            // Get posts by tag
            var postsByTag = await _context.PostTags
                .Where(pt => !pt.Post.IsDeleted)
                .GroupBy(pt => pt.Tag.Name)
                .Select(g => new { Tag = g.Key, Count = g.Count() })
                .ToListAsync();

            statistics.PostsByTag = postsByTag.ToDictionary(x => x.Tag, x => x.Count);

            // Get views by tag
            var viewsByTag = await _context.PostTags
                .Where(pt => !pt.Post.IsDeleted)
                .GroupBy(pt => pt.Tag.Name)
                .Select(g => new { Tag = g.Key, Views = g.Sum(pt => pt.Post.ViewCount) })
                .ToListAsync();

            statistics.ViewsByTag = viewsByTag.ToDictionary(x => x.Tag, x => x.Views);

            // Get posts over time (last 30 days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var postsOverTime = await _context.Posts
                .Where(p => !p.IsDeleted && p.CreatedTime >= thirtyDaysAgo)
                .GroupBy(p => p.CreatedTime.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToListAsync();

            statistics.PostsOverTime = postsOverTime.Select(x => (x.Date, x.Count)).ToList();

            // Get top authors by post count
            statistics.TopAuthors = await _context.Posts
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.Author.UserName)
                .Select(g => new { Author = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync()
                .ContinueWith(t => t.Result.Select(x => (Author: x.Author, PostCount: x.Count)).ToList());

            // Get most viewed posts
            statistics.MostViewedPosts = await _context.Posts
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.ViewCount)
                .Take(10)
                .Select(p => new { Title = p.Title, ViewCount = p.ViewCount })
                .ToListAsync()
                .ContinueWith(t => t.Result.Select(x => (Post: x.Title, ViewCount: x.ViewCount)).ToList());

            return statistics;
        }
    }
}