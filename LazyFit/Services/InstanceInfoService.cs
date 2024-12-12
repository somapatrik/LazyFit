using LazyFit.Models.Administration;

namespace LazyFit.Services
{
    public class InstanceInfoService
    {
        DatabaseService Connection;

        public InstanceInfoService() 
        {
            Connection = new DatabaseService();
        }

        public async Task<bool> InstanceExists()
        {
            return await Connection.Database.Table<InstanceInfo>().CountAsync() > 0;
        }

        public async Task CreateNewInstanceInfo()
        {
            var instance = new InstanceInfo(GetDeviceType(), "Android", IsVirtual());
            await Connection.Database.InsertAsync(instance);
        }

        public async Task UpdateInstanceInfo()
        {
            var instance = await Connection.Database.Table<InstanceInfo>().FirstAsync();
            instance.UpdateInstanceInfo(GetDeviceType(), "Android", IsVirtual());
            await Connection.Database.UpdateAsync(instance);
        }

        public async Task CreateOrReplaceInstance()
        {
            var instance = await Connection.Database.Table<InstanceInfo>().FirstOrDefaultAsync();
            if (instance == null)
                instance = new InstanceInfo(GetDeviceType(), "Android", IsVirtual());

            await Connection.Database.InsertOrReplaceAsync(instance);
        }

        private bool IsVirtual()
        {
            return DeviceInfo.Current.DeviceType == DeviceType.Virtual;
        }

        private string GetDeviceType()
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

        public async Task<InstanceInfo> GetInstance()
        {
            return await Connection.Database.Table<InstanceInfo>().FirstAsync();
        }
    }
}
