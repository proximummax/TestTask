using System;
using VContainer.Unity;

namespace Game.Scripts.Notifications
{
    public class NotificationPresenter : IStartable, IDisposable
    {
        private readonly INotificationService _notificationService;
        private readonly NotificationView _notificationView;

        public NotificationPresenter(
            INotificationService notificationService,
            NotificationView notificationView)
        {
            _notificationService = notificationService;
            _notificationView = notificationView;
        }

        public void Start()
        {
            _notificationService.MessagePublished += OnMessagePublished;

            if (!string.IsNullOrWhiteSpace(_notificationService.CurrentMessage))
            {
                _notificationView.ShowNotification(_notificationService.CurrentMessage);
            }
        }

        public void Dispose()
        {
            _notificationService.MessagePublished -= OnMessagePublished;
        }

        private void OnMessagePublished(string message)
        {
            _notificationView.ShowNotification(message);
        }
    }
}
