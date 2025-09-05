namespace BlogProject.Application.DTOs
{
    public class StatisticsDto
    {
        public int TotalPosts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalComments { get; set; }
        public int TotalViews { get; set; }
        public Dictionary<string, int> PostsByCategory { get; set; } = new();
        public Dictionary<string, int> ViewsByCategory { get; set; } = new();
        public Dictionary<string, int> PostsByTag { get; set; } = new();
        public Dictionary<string, int> ViewsByTag { get; set; } = new();
        public int AverageViewsPerPost { get; set; }
        public int AverageCommentsPerPost { get; set; }
        public List<(DateTime Date, int Count)> PostsOverTime { get; set; } = new();
        public List<(DateTime Date, int Count)> ViewsOverTime { get; set; } = new();
        public List<(string Author, int PostCount)> TopAuthors { get; set; } = new();
        public List<(string Post, int ViewCount)> MostViewedPosts { get; set; } = new();
    }
}