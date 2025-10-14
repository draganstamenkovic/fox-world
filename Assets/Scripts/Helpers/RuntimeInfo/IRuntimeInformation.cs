namespace Helpers.RuntimeInfo
{
    public interface IRuntimeInformation
    {
        RuntimeOSPlatform OSPlatform { get; set; }
        void Initialize();
    }
}