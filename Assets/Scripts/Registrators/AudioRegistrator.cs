using Audio.Managers;
using VContainer;

namespace Registrators
{
    public class AudioRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            builder.Register<IAudioManager, AudioManager>(Lifetime.Scoped);
        }
    }
}