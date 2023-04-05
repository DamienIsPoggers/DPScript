using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    RuntimeAnimatorController defaultController;
    [SerializeField]
    GameObject camera;
    Animator cameraAnimator;

    Vector3 defaultPosition = Vector3.zero;
    Vector3 defaultRotation = Vector3.zero;
    bool isFocused = false;
    Transform camFocus;
    float camZoom = 1;
    Vector3 focusPos;

    [SerializeField]
    Transform animOffset;
    bool isInAnim = false;
    uint[] camAnimBlendTime = new uint[4];

    void Awake()
    {
        Instance = this;
        cameraAnimator = camera.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInAnim)
        {
            camAnimBlendTime[0]++;
            if (camAnimBlendTime[3] <= camAnimBlendTime[1])
            {
                transform.localPosition = Vector3.Lerp(defaultPosition, focusPos, (float)camAnimBlendTime[3] / camAnimBlendTime[1]);
                camAnimBlendTime[3]++;
            }
            if (camAnimBlendTime[0] >= camAnimBlendTime[2])
            {
                if (camAnimBlendTime[0] == camAnimBlendTime[2])
                    camAnimBlendTime[3] = 1;
                if (camAnimBlendTime[3] <= camAnimBlendTime[1])
                {
                    Debug.Log((float)camAnimBlendTime[3] / camAnimBlendTime[1]);
                    transform.localPosition = Vector3.Lerp(focusPos, defaultPosition, (float)camAnimBlendTime[3] / camAnimBlendTime[1]);
                    camAnimBlendTime[3]++;
                }
                else
                {
                    isInAnim = false;
                    cameraAnimator.runtimeAnimatorController = defaultController;
                    cameraAnimator.Play("Camera Rest");
                    transform.localScale = Vector3.one;
                    animOffset.localPosition = Vector3.zero;
                    animOffset.localEulerAngles = Vector3.zero;
                }
            }

        }
    }

    public void playCameraAnimation(RuntimeAnimatorController animator, float playerDir, string state, Transform focus, uint blendTime, uint blendOutTime, 
        Vector3 offsetPos, Vector3 offsetRot)
    {
        updateDefaultPos();
        cameraAnimator.runtimeAnimatorController = animator;
        cameraAnimator.Play(state);
        camFocus = focus;
        transform.localScale = new Vector3(playerDir, 1, 1);
        camAnimBlendTime[0] = 0;
        camAnimBlendTime[1] = blendTime;
        camAnimBlendTime[2] = blendOutTime;
        camAnimBlendTime[3] = 1;
        isInAnim = true;
        animOffset.localPosition = offsetPos;
        animOffset.localEulerAngles = offsetRot;
        focusPos = new Vector3(camFocus.localPosition.x, camFocus.localPosition.y + 2, -3);
    }

    void updateDefaultPos()
    {
        defaultPosition = new Vector3(0, -2, -3);
        defaultRotation = transform.localEulerAngles;
    }
}
