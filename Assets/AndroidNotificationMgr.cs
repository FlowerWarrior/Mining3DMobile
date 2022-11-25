using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class AndroidNotificationMgr : MonoBehaviour
{
    bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus == true && isPaused == false)
        {
            ScheduleNotifiction();
            isPaused = true;
        }
    }

    void OnApplicationQuit()
    {
        ScheduleNotifiction();
    }

    void ScheduleNotifiction()
    {
        if (PlayerPrefs.GetInt("Notifications", 1) == 0)
        {
            return;
        }

        AndroidNotificationCenter.CancelAllNotifications();

        var c = new AndroidNotificationChannel()
        {
            Id = "notif1",
            Name = "openreminder",
            Importance = Importance.Low,
            Description = "Remind of existence of game",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);

        var notification = new AndroidNotification();
        notification.Title = "Mining Game 3D";

        int r = Random.Range(0, 2);
        if (r == 0)
        {
            notification.Text = "Ready for some mining?";
        }
        else if (r == 1)
        {
            notification.Text = "Discover new depths and tools.";
        }
        else
        {
            notification.Text = "Search for rare ores.";
        }

        notification.FireTime = System.DateTime.Now.AddDays(3);
        notification.SmallIcon = "icon_1";
        notification.LargeIcon = "icon_0";
        AndroidNotificationCenter.SendNotification(notification, "notif1");
    }

    
}
