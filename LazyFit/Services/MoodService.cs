using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models.Moods;

namespace LazyFit.Services
{
    public static class MoodService
    {
        public static async Task InsertMood(Mood mood)
        {
            await DB.Database.InsertAsync(mood);
            WeakReferenceMessenger.Default.Send(new MoodNewMessage(mood));
        }

        public static async Task UpdateMood(Mood mood)
        {
            await DB.Database.UpdateAsync(mood);
            WeakReferenceMessenger.Default.Send(new MoodUpdateMessage(mood));
        }

        public static async Task DeleteMood(Mood mood)
        {
            await DB.Database.DeleteAsync(mood);
            WeakReferenceMessenger.Default.Send(new MoodDeleteMessage(mood));
        }

        public static async Task<List<Mood>> GetAllMoods()
        {
            return await DB.Database.Table<Mood>().ToListAsync();
        }

        public static async Task<List<Mood>> GetMoods(DateTime fromTime, DateTime toTime, bool LoadProperties = false)
        {
            var moods = await DB.Database.Table<Mood>().Where(m => m.Time >= fromTime && m.Time <= toTime).ToListAsync();

            if (!LoadProperties)
            {
                return moods;
            }
            else
            {
                var properties = await GetAllMoodProperties();
                foreach (var mood in moods)
                    mood.Property = properties.FirstOrDefault(x => x.MoodID == mood.TypeOfMood);

                return moods;

            }

        }

        public static async Task<List<Mood>> GetMoodsFromLastDays(int numberOfDays)
        {
            DateTime now = DateTime.Today;

            DateTime from = new DateTime(now.AddDays(-numberOfDays).Date.Ticks);
            DateTime to = new DateTime(now.Ticks).AddDays(1).AddSeconds(-1);

            return await GetMoods(from, to, true);
        }

        public static async Task<List<MoodProperty>> GetAllMoodProperties()
        {
            return await DB.Database.Table<MoodProperty>().ToListAsync();
        }
    }
}
