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

        public static async Task<List<Mood>> GetMoods(DateTime fromTime, DateTime toTime)
        {
            return await DB.Database.Table<Mood>().Where(m => m.Time >= fromTime && m.Time <= toTime).ToListAsync();
        }

        public static async Task<List<MoodProperty>> GetAllMoodProperties()
        {
            return await DB.Database.Table<MoodProperty>().ToListAsync();
        }
    }
}
