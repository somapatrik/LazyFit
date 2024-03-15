using LazyFit.Models.Pressure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Services
{
    public static class PressureService
    {
        public static async Task InsertPressure(BloodPressure pressure)
        {
            await DB.Database.InsertAsync(pressure);
        }

        public static async Task<List<BloodPressure>> GetPressures(DateTime fromDate, DateTime toDate)
        {
            return await DB.Database.Table<BloodPressure>().Where(p => p.Time >= fromDate && p.Time <= toDate).OrderByDescending(bp => bp.Time).ToListAsync();
        }

        public static async Task<BloodPressure> GetPressure(Guid id)
        {
            return await DB.Database.Table<BloodPressure>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public static async Task<List<BloodPressure>> GetLastPressures(int count)
        {
            return await DB.Database.Table<BloodPressure>().OrderByDescending(p => p.Time).Take(count).ToListAsync();
        }
    }
}
