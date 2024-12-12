using LazyFit.Models.Pressure;

namespace LazyFit.Services
{
    public  class PressureService
    {
        DatabaseService Connection;

        public PressureService()
        {
            Connection = new DatabaseService();
        }

        public async Task InsertPressure(BloodPressure pressure)
        {
            await Connection.Database.InsertAsync(pressure);
        }

        public async Task<List<BloodPressure>> GetPressures(DateTime fromDate, DateTime toDate)
        {
            return await Connection.Database.Table<BloodPressure>().Where(p => p.Time >= fromDate && p.Time <= toDate).OrderByDescending(bp => bp.Time).ToListAsync();
        }

        public async Task<BloodPressure> GetPressure(Guid id)
        {
            return await Connection.Database.Table<BloodPressure>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<BloodPressure>> GetLastPressures(int count)
        {
            return await Connection.Database.Table<BloodPressure>().OrderByDescending(p => p.Time).Take(count).ToListAsync();
        }
    }
}
