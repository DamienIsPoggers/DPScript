using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;
using System;
using UnityEngine.UIElements;

public class Object_Collision : MonoBehaviour
{
    public collisionBox box;
    public GameWorldObject player;
    public GameWorldObject parent;
    
    public bool isStatic = false;

    public void init(collisionBox box, bool sphere, GameWorldObject player, GameWorldObject parent)
    {
        this.box = box;
        this.player = player;
        this.parent = parent;

        if (sphere)
        {

        }
        else
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            Vector3 size = new Vector3(box.x[1] / 2, box.y[1] / 2, box.z[1] / 2);
            Vector3 center = new Vector3((float)(box.x[0] - size.x) / 225, (float)(box.y[0] - size.y) / 225, (float)(box.z[0] - size.z) / 225);
            size.x /= 100; //size.x = Mathf.Abs(size.x);
            size.y /= 100; //size.y = Mathf.Abs(size.y);
            size.z /= 100; //size.z = Mathf.Abs(size.z);
            collider.size = size;
            collider.center = center;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("i wanna kill myself");
        Object_Collision tempBox = other.GetComponent<Object_Collision>();
        if (tempBox == null || !isActive())
            return;
        
        if (tempBox.player == player)
            if (!parent.friendlyFire || tempBox.parent == parent)
                return;

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
        parent.boxesCollide(this, tempBox);
    }

    public bool isActive()
    {
        switch (box.type)
        {
            case 0:
                if (parent.noCollision)
                    return false;
                break;
            case 1:
                if (parent.invincible)
                    return false;
                break;
            case 2:
                if (parent.hitboxesDisabled)
                    return false;
                break;
        }
        return true;
    }

    public void kill()
    {
        parent.loadedCollisions.Remove(this);
        Destroy(gameObject);
    }
}
