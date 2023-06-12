using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace BeUP.Services;

public static class NotificationService
{
    public static async Task MakeNotification(int hours, int minutes)
    {
        DateTime requestedTime = DateTime.Today;
        requestedTime = requestedTime.AddHours(hours);
        requestedTime = requestedTime.AddMinutes(minutes);

        if (requestedTime.Hour < DateTime.Now.Hour || requestedTime.Minute <= DateTime.Now.Minute) 
        {
            requestedTime = requestedTime.AddDays(1);
        }

        var request = new NotificationRequest
        {
            NotificationId = 1,
            Title = "Нагадуємо вам про страву.",
            Description = "Переглянте рецепт обраної страви та перевірте наявність всіх інгредієнтів для її приготування.",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = requestedTime
            }
        };

        await LocalNotificationCenter.Current.Show(request);
    }
}
