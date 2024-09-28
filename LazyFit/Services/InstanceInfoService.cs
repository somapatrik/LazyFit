using Kotlin.Contracts;
using LazyFit.Models.Administration;
using System.ComponentModel.DataAnnotations.Schema;

namespace LazyFit.Services
{
    public static class InstanceInfoService
    {

        public static async Task<bool> InstanceExists()
        {
            return await DB.Database.Table<InstanceInfo>().CountAsync() > 0;
        }

        public static async Task CreateNewInstanceInfo()
        {
            var instance = new InstanceInfo(GetDeviceType(), "Android", IsVirtual());
            await DB.Database.InsertAsync(instance);
        }

        public static async Task UpdateInstanceInfo()
        {
            var instance = await DB.Database.Table<InstanceInfo>().FirstAsync();
            instance.UpdateInstanceInfo(GetDeviceType(), "Android", IsVirtual());
            await DB.Database.UpdateAsync(instance);
        }

        private static bool IsVirtual()
        {
            return DeviceInfo.Current.DeviceType == DeviceType.Virtual;
        }

        private static string GetDeviceType()
        {
            if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
               return "Desktop";
            else if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
                return "Phone";
            else if (DeviceInfo.Current.Idiom == DeviceIdiom.Tablet)
                return "Tablet";
            else
                return "";
        }

        public static async Task<InstanceInfo> GetInstance()
        {
            return await DB.Database.Table<InstanceInfo>().FirstAsync();
        }
    }
}
