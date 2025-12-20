using Registrators;
using Message;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        ConfigsRegistrator.Register(builder);
        GuiRegistrator.Register(builder);
        GameRegistrator.Register(builder);
        DataRegistrator.Register(builder);
        InputRegistrator.Register(builder);
        AudioRegistrator.Register(builder);
        AnalyticsRegistrator.Register(builder);
        ScreenControllersRegistrator.Register(builder);
        PopupControllersRegistrator.Register(builder);
        
        builder.Register<IMessageBroker, MessageBroker>(Lifetime.Singleton);
        builder.RegisterEntryPoint<Bootstrap>();
    }
}
