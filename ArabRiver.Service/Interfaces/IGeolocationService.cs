namespace ArabRiver.Service.Interfaces
{
    public interface IGeolocationService
    {
        Task<(string Country,
            string CountryCode,
            bool IsEgypt)>
            GetLocationAsync(string ipAddress);
    }
}
