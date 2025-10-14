using R3;

namespace Message
{
    public interface IMessageBroker
    {
        void Publish<T>(T message);
        Observable<T> Receive<T>();
    }

}