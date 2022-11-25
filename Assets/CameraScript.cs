using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Vector3 gameplayPos;
    Quaternion gameplayRot;

    // Start is called before the first frame update
    void Start()
    {
        gameplayPos = transform.position;
        gameplayRot = transform.rotation;
    }

    public void SetCameraToShop()
    {
        transform.position = new Vector3(100, 100, 100);
    }

    public void SetCameraToGameplay()
    {
        transform.position = gameplayPos;
        transform.rotation = gameplayRot;
    }
}
