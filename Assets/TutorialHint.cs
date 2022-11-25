using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHint : MonoBehaviour
{
    // Listener
    private void Awake()
    {
        MiningMgr.PickaxeHit += TutorialCompleted;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("FirstLaunch", 1) != 1)
        {
            gameObject.SetActive(false);
        }
    }

    void TutorialCompleted()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetInt("FirstLaunch", 0);
    }
}
