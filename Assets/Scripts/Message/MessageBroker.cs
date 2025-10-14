using System;
using System.Collections.Generic;
using R3;

namespace Message
{
    public class MessageBroker : IMessageBroker
    {
        private readonly Dictionary<Type, object> _subjects = new();

        public void Publish<T>(T message)
        {
            if (_subjects.TryGetValue(typeof(T), out var subjectObj))
            {
                var subject = (Subject<T>)subjectObj;
                subject.OnNext(message);
            }
        }

        public Observable<T> Receive<T>()
        {
            if (!_subjects.TryGetValue(typeof(T), out var subjectObj))
            {
                var subject = new Subject<T>();
                _subjects[typeof(T)] = subject;
                return subject.AsObservable();
            }

            return ((Subject<T>)subjectObj).AsObservable();
        }
    }
}