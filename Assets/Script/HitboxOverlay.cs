using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;

public class HitboxOverlay : MonoBehaviour
{
    public bool active = false;

    Material colBox, hitBox, hurtBox, miscBox, snapBox;

    [SerializeField]
    Battle_Manager battleManager;
    List<Object_Collision> collisions;
    List<GameObject> boxesRendered = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        colBox = Resources.Load<Material>("UI/HitboxOverlay/Colbox_Mat");
        hitBox = Resources.Load<Material>("UI/HitboxOverlay/Hitbox_Mat");
        hurtBox = Resources.Load<Material>("UI/HitboxOverlay/Hurtbox_Mat");
        miscBox = Resources.Load<Material>("UI/HitboxOverlay/Miscbox_Mat");
        snapBox = Resources.Load<Material>("UI/HitboxOverlay/Snapbox_Mat");
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!active)
            return;

        if(collisions != battleManager.collisions)
        {
            if(collisions != null)
            for(int i = 0; i < collisions.Count; i++)
                Destroy(boxesRendered[0]);

            boxesRendered.Clear();

            collisions = battleManager.collisions;

            for(int i = 0; i < collisions.Count; i++)
            {
                Object_Collision tempBox = collisions[i];
                GameObject box = new GameObject("box");

                

                box.transform.parent = transform;

                box.transform.position = new Vector3(tempBox.locX, tempBox.locY, tempBox.locZ);
                box.transform.localScale = new Vector3(tempBox.distanceX, tempBox.distanceY, tempBox.distanceZ);

                boxesRendered.Add(box);
                
            }
        }
    }
}
