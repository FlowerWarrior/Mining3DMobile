using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Notifications.Android;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _image;
    [SerializeField] string SavedVariableName;
    [SerializeField] Sprite _spriteON;
    [SerializeField] Sprite _spriteOFF;

    void Start()
    {
        UpdateLooks();
    }

    public void Clicked()
    {
        if (PlayerPrefs.GetInt(SavedVariableName, 1) == 0)
        {
            PlayerPrefs.SetInt(SavedVariableName, 1);
        }
        else
        {
            PlayerPrefs.SetInt(SavedVariableName, 0);
            if (SavedVariableName == "Notifications")
            {
                AndroidNotificationCenter.CancelAllNotifications();
            }
        }

        UpdateLooks();
    }

    void UpdateLooks()
    {
        if (PlayerPrefs.GetInt(SavedVariableName, 1) == 1)
        {
            // Enabled
            _text.color = new Color32(120, 255, 120, 255);
            _image.color = new Color32(120, 255, 120, 255);
            _image.sprite = _spriteON;
        }
        else
        {
            // Disabled
            _text.color = new Color32(255, 120, 120, 255);
            _image.color = new Color32(255, 120, 120, 255);
            _image.sprite = _spriteOFF;
        }
    }
}
