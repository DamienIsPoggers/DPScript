using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UIElements;

namespace DPScript.Editor
{
    public class EditorCollisionBox : MonoBehaviour
    {
        collisionBox box;
        bool sphere = false;

        public void update(collisionBox box, bool sphere)
        {
            this.box = box;
            this.sphere = sphere;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<GameWorldObject>() == null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            switch(box.type)
            {
                default:
                case 0:
                    Gizmos.color = Color.yellow;
                    break;
                case 1:
                    Gizmos.color = Color.blue;
                    break;
                case 2:
                    Gizmos.color = Color.red;
                    break;
                case 3:
                    Gizmos.color = Color.green;
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    Gizmos.color = Color.grey;
                    break;
                case 11:
                    Gizmos.color = Color.magenta;
                    break;
            }

            if(sphere)
            {

            }
            else
            {
                Vector3 size = new Vector3(box.x[1] / 2, box.y[1] / 2, box.z[1] / 2);
                Vector3 center = new Vector3((float)(box.x[0] - size.x) / 225, (float)(box.y[0] - size.y) / 225, (float)(box.z[0] - size.z) / 225);
                size.x /= 100;
                size.y /= 100;
                size.z /= 100;
                Gizmos.DrawWireCube(center + transform.position, size);
            }
        }
#endif
    }
}