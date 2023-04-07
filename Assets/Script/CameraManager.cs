using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    RuntimeAnimatorController defaultController;
    [SerializeField]
    GameObject cam;
    Animator cameraAnimator;

    Vector3 defaultPosition = Vector3.zero;
    Vector3 defaultRotation = Vector3.zero;
    bool isFocused = false;
    Transform camFocus;
    float camZoom = 1f;
    byte camZoomTime = 0, camZoomReturnTimer = 0;
    Vector3 focusPos;

    [SerializeField]
    Transform animOffset;
    bool isInAnim = false;
    uint[] camAnimBlendTime = new uint[4];

    Transform p1, p2;

    void Awake()
    {
        Instance = this;
        cameraAnimator = cam.GetComponent<Animator>();
    }

    void Start()
    {
        p1 = Battle_Manager.Instance.players[0].transform;
        p2 = Battle_Manager.Instance.players[1].transform;
    }

    void Update()
    {
        transform.parent = Battle_Manager.Instance.stages[p1.GetComponent<GameWorldObject>().onStage].transform;

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
        else
        {
            float posX = (p1.localPosition.x + p2.localPosition.x) / 2;
            float posY = 1.5f;
            float posZ = -3f;
            float distance = Mathf.Abs(p1.localPosition.x - p2.localPosition.x); 
            if (distance < camZoom + 3f && camZoomTime > 0)
            {
                camZoomTime--;
                if (camZoomTime <= 20)
                {
                    if (distance < 4f)
                        distance = 4f;
                    camZoomReturnTimer++;
                    posZ += -Mathf.Lerp(camZoom + 3f, distance, (float)camZoomReturnTimer / 20) / 2 + 2f;
                }
                else
                    posZ += -(camZoom + 3) / 2 + 2f;
            }
            else if (distance > 4f)
            {
                posZ += -distance / 2 + 2f;
                camZoom = distance - 3f;
                camZoomTime = 90;
                camZoomReturnTimer = 0;
            }
            transform.localPosition = new Vector3(posX, posY, posZ);
            updateDefaultPos();
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
        focusPos = new Vector3(camFocus.localPosition.x, camFocus.localPosition.y + 1.5f, -3);
    }

    void updateDefaultPos()
    {
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localEulerAngles;
    }
}
