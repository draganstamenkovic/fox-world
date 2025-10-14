using Cameras;
using Gameplay;
using Gameplay.Level;
using Gameplay.Player;
using VContainer;

namespace Registrators
{
    public class GameRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            builder.Register<IGameManager, GameManager>(Lifetime.Singleton);
            builder.Register<ICameraManager, CameraManager>(Lifetime.Singleton);
            builder.Register<IPlayerController, PlayerController>(Lifetime.Singleton);
            builder.Register<ILevelManager, LevelManager>(Lifetime.Singleton);
        }
    }
}