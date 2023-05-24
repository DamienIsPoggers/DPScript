using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    bool lookAtCamera = true;
    [SerializeField]
    bool isSprite = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtCamera)
        {
            transform.LookAt(CameraManager.Instance.cam.transform);
            if (isSprite)
                transform.localEulerAngles += new Vector3(0, 180, 0);
        }
    }
}
