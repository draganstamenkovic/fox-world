using System;
using Message;
using VContainer;

namespace Gameplay
{
    public class ScoreManager : IScoreManager
    {
        [Inject] private readonly IMessageBroker _messageBroker;
        private IDisposable _collectedItemSubscription;
        private int _cherryCount;
        private int _gemCount;

        public void IncreaseCherryCount()
        {
            _cherryCount++;
        }

        public int GetCherryCount()
        {
            return _cherryCount;
        }

        public void IncreaseGemCount()
        {
            _gemCount++;
        }

        public int GetGemCount()
        {
            return _gemCount;
        }
    }
}