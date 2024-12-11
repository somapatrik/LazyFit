using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models.Moods;

namespace LazyFit.Services
{
    public class MoodService
    {
        DB Connection;

        public MoodService()
        {
            Connection = new DB();
            
            if (Connection.Database == null)
                Connection.InitDB();
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
                var properties = await GetAllMoodProperties();
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

        public async Task<List<MoodProperty>> GetAllMoodProperties()
        {
            return await Connection.Database.Table<MoodProperty>().ToListAsync();
        }

        private void CreateDefaultMoodData()
        {
            List<MoodProperty> moods = new List<MoodProperty>()
            {
                new MoodProperty()
                {
                    MoodID = MoodName.VeryGood,
                    DisplayName = "Very good",
                    Description = "Everything is great!",
                    ImageName = "very_happy.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Good,
                    DisplayName = "Good",
                    Description = "This is fine!",
                    ImageName = "happy.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Normal,
                    DisplayName = "Normal",
                    Description = "Not great, not terrible...",
                    ImageName = "neutral.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Bad,
                    DisplayName = "Bad",
                    Description = "Could be better",
                    ImageName = "angry.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.VeryBad,
                    DisplayName = "Very bad",
                    Description = "F*ck this!",
                    ImageName = "cursing.png"
                },
            };

            moods.ForEach(async m => await Connection.Database.InsertOrReplaceAsync(m));
        }
    }
}
