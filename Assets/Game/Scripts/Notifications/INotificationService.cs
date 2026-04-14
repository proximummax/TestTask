using System;

namespace Game.Scripts.Notifications
{
    public interface INotificationService
    {
        event Action<string> MessagePublished;
        string CurrentMessage { get; }
        void Show(string message);
    }
}
