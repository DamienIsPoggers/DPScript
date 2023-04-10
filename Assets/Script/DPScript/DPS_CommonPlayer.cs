using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript
{
    public class DPS_CommonPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Battle_Manager.Instance.commonPlayer = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}