namespace BlogProject.Web.Utilities
{
    public static class TimeDifference
    {
        public static string TimeAgo(DateTime dateTime)
        {
            var difference = DateTime.Now - dateTime;

            if(difference.TotalSeconds < 60)
            {
                return $"{(int)difference.TotalSeconds} seconds ago";
            }
            else if (difference.TotalMinutes < 60)
            {
                return $"{(int)difference.TotalMinutes} minutes ago";
            }
            else if (difference.TotalHours < 24)
            {
                return $"{(int)difference.TotalHours} hours ago";
            }
            else if (difference.TotalDays < 30)
            {
                return $"{(int)difference.TotalDays} days ago";
            }
            else if (difference.TotalDays < 365)
            {
                return $"{(int)(difference.TotalDays / 30)} months ago";
            }
            else
            {
                return $"{(int)(difference.TotalDays / 365)} years ago";
            }
        }
    }
}
