
namespace LazyFit.Services
{
    public static class LazyUnitConvertes
    {
        public static decimal LbsToKg(decimal lbs)
        {
            decimal kg = Math.Round(lbs * 0.45359237m, 1);
            return kg;
        }
    }
}
