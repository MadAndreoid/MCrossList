namespace MCrossList.Client.Services
{
    public interface IVintedService
    {
        int Items { get; set; }
        Task Update();
    }
}