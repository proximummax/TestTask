using System;

namespace Game.Scripts.Notifications
{
    public class NotificationService : INotificationService
    {
        public event Action<string> MessagePublished;

        public string CurrentMessage { get; private set; } = string.Empty;

        public void Show(string message)
        {
            CurrentMessage = message;
            MessagePublished?.Invoke(message);
        }
    }
}
