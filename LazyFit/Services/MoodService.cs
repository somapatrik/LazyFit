using CommunityToolkit.Mvvm.Messaging;
using LazyFit.LocalData;
using LazyFit.Messages;
using LazyFit.Models.Moods;

namespace LazyFit.Services
{
    public class MoodService
    {
        DatabaseService Connection;
        LocalMoodPropertyData LocalMoodPropertyRepository;
        public MoodService()
        {
            Connection = new DatabaseService();
            LocalMoodPropertyRepository = new LocalMoodPropertyData();
        }

        public async Task InsertMood(Mood mood)
        {
            await Connection.Database.InsertAsync(mood);
            WeakReferenceMessenger.Default.Send(new MoodNewMessage(mood));
        }

        public async Task UpdateMood(Mood mood)
        {
            await Connection.Database.UpdateAsync(mood);
            WeakReferenceMessenger.Default.Send(new MoodUpdateMessage(mood));
        }

        public async Task DeleteMood(Mood mood)
        {
            await Connection.Database.DeleteAsync(mood);
            WeakReferenceMessenger.Default.Send(new MoodDeleteMessage(mood));
        }

        public async Task<List<Mood>> GetAllMoods()
        {
            return await Connection.Database.Table<Mood>().ToListAsync();
        }

        public async Task<List<Mood>> GetMoods(DateTime fromTime, DateTime toTime, bool LoadProperties = false)
        {
            var moods = await Connection.Database.Table<Mood>().Where(m => m.Time >= fromTime && m.Time <= toTime).ToListAsync();

            if (!LoadProperties)
            {
                return moods;
            }
            else
            {
                var properties = LocalMoodPropertyRepository.MoodProperties;
                foreach (var mood in moods)
                    mood.Property = properties.FirstOrDefault(x => x.MoodID == mood.TypeOfMood);

                return moods;

            }

        }

        public async Task<List<Mood>> GetMoodsFromLastDays(int numberOfDays)
        {
            DateTime now = DateTime.Today;

            DateTime from = new DateTime(now.AddDays(-numberOfDays).Date.Ticks);
            DateTime to = new DateTime(now.Ticks).AddDays(1).AddSeconds(-1);

            return await GetMoods(from, to, true);
        }

        public List<MoodProperty> GetAllMoodProperties()
        {
            return LocalMoodPropertyRepository.MoodProperties;
        }


    }
}
