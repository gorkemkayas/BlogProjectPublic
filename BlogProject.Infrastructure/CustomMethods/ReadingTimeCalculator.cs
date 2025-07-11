namespace BlogProject.Infrastructure.CustomMethods
{
        public static class ReadingTimeCalculator
        {
            // Ortalama dakikadaki kelime sayısı
            private const int AverageWordsPerMinute = 200;

            public static TimeSpan CalculateReadingTime(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return TimeSpan.Zero;

                // Kelimeleri boşluklara göre ayır
                var wordCount = text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

                // Dakika cinsinden süreyi hesapla
                double readingTimeInMinutes = (double)wordCount / AverageWordsPerMinute;

                // Dakika cinsinden süreyi TimeSpan'e dönüştür
                return TimeSpan.FromMinutes(readingTimeInMinutes);
            }
        }

    }
