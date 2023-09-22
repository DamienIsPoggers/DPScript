using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Csel_Controller : MonoBehaviour
{
    [SerializeField]
    PlayerInput input;
    public int controllerNum;

    void Start()
    {
        controllerNum = CharacterSelect.Instance.controllersAdded;
        CharacterSelect.Instance.controllersAdded++;
    }

    void Update()
    {
        Vector2Int move = new Vector2Int(Mathf.RoundToInt(input.actions["Move"].ReadValue<Vector2>().x),
            Mathf.RoundToInt(input.actions["Move"].ReadValue<Vector2>().y));
        CharacterSelect.Instance.inputUpdate(move, input.actions["Accept"].IsPressed(), input.actions["Close"].IsPressed(), controllerNum);
    }
}
