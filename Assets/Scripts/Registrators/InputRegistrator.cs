using Input;
using VContainer;
using VContainer.Unity;

namespace Registrators
{
    public class InputRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InputManager>();
        }
    }
}