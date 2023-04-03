using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;
using System;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

public class Object_Collision : MonoBehaviour
{
    public collisionBox box;
    public bool hasZ = false;
    public bool sphere = false;
    public GameWorldObject player;
    public GameWorldObject parent;

    public int locX, locY, locZ;
    public int posX, posY, posZ;
    public int distanceX, distanceY, distanceZ;

    [SerializeField]
    bool hasInit = false;
    public bool isStatic = false;

    public void init(collisionBox box, bool hasZ, bool sphere, GameWorldObject player, GameWorldObject parent)
    {
        this.box = box;
        this.hasZ = hasZ;
        this.sphere = sphere;
        this.player = player;
        this.parent = parent;


        locX = box.x[0]; // 1000
        locY = box.y[0]; // 250
        distanceX = box.x[1]; // 100
        distanceY = box.y[1]; // 100
        if (hasZ)
        {
            locZ = box.z[0] + parent.locZ / 10000;
            distanceZ = box.z[0];
        }
        else
        {
            locZ = 0;
            distanceZ = 2; // 0.2f
        }

        posX = (locX * parent.dir) + (parent.locX / 100);
        posY = locY + (parent.locY / 100);
        posZ = locZ + (parent.locZ / 100);

        hasInit = true;
    }

    void Update()
    {
        if (!hasInit)
            return;

        switch(box.type)
        {
            case 0:
                if (parent.noCollision)
                    return;
                break;
            case 1:
                if (parent.invincible)
                    return;
                break;
            case 2:
                if (parent.hitboxesDisabled)
                    return;
                break;
        }

        posX = (locX * parent.dir) + (parent.locX / 100);
        posY = locY + (parent.locY / 100);
        posZ = locZ + (parent.locZ / 100);

        /*
        if(battleManager.showHitboxes)
        {
            if (boxOverlay == null)
                createOverlayBox();

            switch (box.type)
            {
                case 0:
                    boxColor.color = new Color(0, 1, 1, .3f); break;
                case 1:
                    boxColor.color = new Color(0, 0, 1, .3f); break;
                case 2:
                    boxColor.color = new Color(1, 0, 0, .3f); break;
            }

            boxOverlay.GetComponent<MeshRenderer>().material = boxColor;
            boxOverlay.transform.parent = transform;
            boxOverlay.transform.localPosition = new Vector3((float)posX / 1000, (float)posY / 250);
            boxOverlay.transform.localScale = new Vector3((float)distanceX / 100, (float)distanceY / 100, (float)distanceZ / 100);
        }
        */

        for (int i = 0; i < Battle_Manager.Instance.collisions.Count; i++)
        {
            
            if (Battle_Manager.Instance.collisions[i].parent == parent)
                continue;
            if (Battle_Manager.Instance.collisions[i].player == player)
                if(!parent.friendlyFire)
                    continue;
            
            Object_Collision tempBox = Battle_Manager.Instance.collisions[i];

            switch (tempBox.box.type)
            {
                case 0:
                    if (tempBox.parent.noCollision)
                        return;
                    break;
                case 1:
                    if (tempBox.parent.invincible)
                        return;
                    break;
                case 2:
                    if (tempBox.parent.hitboxesDisabled)
                        return;
                    break;
            }

            if (posY + distanceY / 2 >= tempBox.posY - tempBox.distanceY / 2
                && posY - distanceY / 2 <= tempBox.posY + tempBox.distanceY / 2
                && posX + distanceX / 2 >= tempBox.posX - tempBox.distanceX / 2
                && posX - distanceX / 2 <= tempBox.posX + tempBox.distanceX / 2)
                parent.boxesCollide(this, tempBox);
        }
        
    }

    /*
    void OnDrawGizmos()
    {
        switch(box.type)
        {
            case 0:
                Gizmos.color = Color.yellow; break;
            case 1:
                Gizmos.color = Color.blue; break;
            case 2:
                Gizmos.color = Color.red; break;
        }

        Gizmos.DrawCube(new Vector3((float)posX / 100, (float)posY / 250), new Vector3((float)distanceX / 10, (float)distanceY / 100, (float)distanceZ / 100));
    }
    */

    public void kill()
    {
        DestroyImmediate(this);
        Battle_Manager.Instance.collisions.Remove(this);
        parent.loadedCollisions.Remove(this);
    }
}
