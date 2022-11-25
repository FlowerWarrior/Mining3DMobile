using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : MonoBehaviour
{
    public static System.Action Clicked;
    public void ClickedSell()
    {
        Clicked?.Invoke();
    }
}
