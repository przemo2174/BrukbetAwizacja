using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrukbetAwizacja
{
    public enum NotificationType
    {
        GreenNotification = 0,
        RedNotification,
        Both,
        None
    }

    public enum NotificationStatus
    {
        Current = 0,
        Pending
    }
}
