using Data;
using Data.Load;
using Data.Save;
using UnityEngine;
using VContainer;

namespace Registrators
{
    public class DataRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            builder.Register<ISaveManager, SaveManager>(Lifetime.Singleton);
            builder.Register<ILoadManager, LoadManager>(Lifetime.Singleton);
            
            /*
            var gameData = Resources.Load<GameData>("Data/GameData");
            builder.RegisterInstance(gameData).As(gameData.GetType()).AsImplementedInterfaces();
            */
        }
    }
}