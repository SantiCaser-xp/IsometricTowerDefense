using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using Unity.Notifications.Android;

public class ControladorNotificaciones : MonoBehaviour
{
    public static ControladorNotificaciones Instance;

    AndroidNotificationChannel notifChannel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        RequestNotificationPermission();
    }

    private void RequestNotificationPermission()
    {
        string notificationPermission = "android.permission.POST_NOTIFICATIONS";

        if (!Permission.HasUserAuthorizedPermission(notificationPermission))
        {
            PermissionCallbacks callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += PermissionForNotifChannel;
            Permission.RequestUserPermission(notificationPermission, callbacks);
        }
        else
        {
            PermissionForNotifChannel("Permitido");
        }
    }
    private void PermissionForNotifChannel(string permission)
    {
        if (PlayerPrefs.HasKey("Display_Comeback"))
            CancelNotification(PlayerPrefs.GetInt("Display_ComeBack"));

        notifChannel = new AndroidNotificationChannel()
        {
            Id = "reminder_notif_ch",
            Name = "Reminder Notification",
            Importance = Importance.High,
            Description = "Canal para las notificaciones del juego",
            EnableLights = true,
            EnableVibration = true,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notifChannel);

        PlayerPrefs.SetInt("Display_Comeback", DisplayNotification("VUELVE!!!!", "Te extranamos mucho", IconSelecter.icon_0, IconSelecter.icon_1, DateTime.Now.AddHours(36)));
    }

    public int DisplayNotification(string title, string text, IconSelecter iconSmall, IconSelecter iconLarge, DateTime fireTime )
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.SmallIcon = iconSmall.ToString();
        notification.LargeIcon = iconLarge.ToString();
        notification.FireTime = fireTime;
        return AndroidNotificationCenter.SendNotification(notification, notifChannel.Id);
    }

    public void CancelNotification(int id)
    {
        AndroidNotificationCenter.CancelNotification(id);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // El jugador minimizó o salió de la app
            ProgramarNotificacionInactividad();
        }
        else
        {
            // El jugador regresó, cancela la notificación
            CancelarNotificacionInactividad();
        }
    }

    private void OnApplicationQuit()
    {
        // El jugador cerró la app
        ProgramarNotificacionInactividad();
        ProgramarNotificacionCierre();
    }

    private void ProgramarNotificacionCierre()
    {
        // Por ejemplo, 5 minutos después de cerrar la app
        DateTime fireTime = DateTime.Now.AddSeconds(2);
        int notifId = DisplayNotification(
            "¡No te vayas!",
            "Vuelve pronto, tenemos sorpresas para ti.",
            IconSelecter.icon_0,
            IconSelecter.icon_1,
            fireTime
        );
        PlayerPrefs.SetInt("CloseAppNotifId", notifId);
    }

    private void ProgramarNotificacionInactividad()
    {
        DateTime fireTime = DateTime.Now.AddSeconds(4);
        int notifId = DisplayNotification(
            "¡Vuelve al juego!",
            "Te extrañamos, regresa y sigue jugando.",
            IconSelecter.icon_0,
            IconSelecter.icon_1,
            fireTime
        );
        PlayerPrefs.SetInt("InactivityNotifId", notifId);
    }

    private void CancelarNotificacionInactividad()
    {
        if (PlayerPrefs.HasKey("InactivityNotifId"))
        {
            CancelNotification(PlayerPrefs.GetInt("InactivityNotifId"));
            PlayerPrefs.DeleteKey("InactivityNotifId");
        }
    }

}

public enum IconSelecter
{
    icon_0,
    icon_1,
}
