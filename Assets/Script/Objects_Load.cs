using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;
using DPScript.Loading;
using System.Linq;
using UnityEditor;

public class Objects_Load
{
    public static IEnumerator mainLoad(DPS_ObjectLoad load, int color, bool setNum = false, byte num = 0, int setPos = 0)
    {
        GameObject character = Object.Instantiate(load.prefab);
        GameWorldObject o = character.GetComponent<GameWorldObject>();

        for(int i = 0; i < load.scriptLoad.Count; i++)
        {
            scriptFile temp = DPS_FileReader.loadScript(load.scriptLoad[i]);
            for (int i2 = 0; i2 < temp.entries.Count; i2++)
            {
                if (temp.entries[temp.entryNames[i2]].subroutine)
                    o.subroutines.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
                else
                    o.states.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
            }
        }

        for(int i = 0; i < load.cmnScriptLoad.Count; i++)
        {
            scriptFile temp = DPS_FileReader.loadScript(load.cmnScriptLoad[i]);
            for (int i2 = 0; i2 < temp.entries.Count; i2++)
            {
                if (temp.entries[temp.entryNames[i2]].subroutine)
                    o.commonSubroutines.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
            }
        }

        for(int i = 0; i < load.colLoad.Count; i++)
        {
            collisionFile temp = DPS_FileReader.loadCollision(load.colLoad[i]);
            for (int i2 = 0; i2 < temp.entries.Count; i2++)
                o.collisions.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
        }

        if(load.stats != null)
        {
            DPS_PlayerStats stats = load.stats;
            o.idStr = stats.characterId;
            o.maxHealth = stats.health;
            o.curHealth = o.maxHealth;
            o.dirType = stats.directionType;
            o.faceCamera = stats.faceCamera;
            o.useArmature = stats.usesMeshes;
            o.weightMultiplier = stats.weightMultiplier;
            o.objectStartState = stats.startState;
            o.walkingSpeed[0] = stats.forwardWalkingSpeed;
            o.walkingSpeed[1] = stats.backwardsWalkingSpeed;
            o.dashSpeed[0] = stats.initalDashingSpeed;
            o.dashSpeed[1] = stats.dashingAccelerationRate;
            o.dashSpeed[2] = stats.dashingMaxSpeed;
            o.jumpSpeed[0] = stats.forwardJumpSpeed;
            o.jumpSpeed[1] = stats.backwardsJumpSpeed;
            o.jumpSpeed[2] = stats.jumpingHeight;
            o.defaultGravity = stats.defualtGravity;
            o.airActionsCount[0] = stats.airJumpCount;
            o.airActionsCount[1] = stats.forwardAirDashCount;
            o.airActionsCount[2] = stats.backwardsAirDashCount;
            o.defualtAirActionsCount = o.airActionsCount;
            o.armatureList = stats.meshNames;
        }    

        if (load.materials == null || load.materials.mats.Count == 0)
            yield break;

        if (o.useArmature)
        {
            if (o.renderers.Count == 0)
                o.recallAwake();

            for (int i = 0; i < load.materials.mats[color].materials.Count; i++)
            {
                DPS_ObjectMat mat = load.materials.mats[color].materials[i];
                if (!o.renderers.ContainsKey(mat.name))
                    continue;
                o.renderers[mat.name].materials = mat.materials.ToArray();
            }
        }

        if (setNum)
        {
            o.playerNum = num;
            o.locX = setPos;
        }
    }

    public static void debugLoad(string path, GameWorldObject o, string id, bool debug = false)
    {
        dataArray_File loadFile = DPS_FileReader.loadDataArray(Resources.Load<TextAsset>(path));

        if(loadFile.entries.ContainsKey("ScriptLoad"))
            for(int i = 0; i < loadFile.entries["ScriptLoad"].count; i++)
            {
                scriptFile temp = DPS_FileReader.loadScript(Resources.Load<TextAsset>("Char/"+ id + "/" + loadFile.entries["ScriptLoad"].arrays[i].strings[0]));
                for(int i2 = 0; i2 < temp.entries.Count; i2++)
                {
                    if (temp.entries[temp.entryNames[i2]].subroutine)
                        o.subroutines.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
                    else
                        o.states.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
                }
            }

        if (loadFile.entries.ContainsKey("CommonScriptLoad"))
            for(int i = 0; i < loadFile.entries["CommonScriptLoad"].count; i++)
            {
                //Debug.Log("Cmn Script Load");
                scriptFile temp = DPS_FileReader.loadScript(Resources.Load<TextAsset>("Char/Cmn/" + loadFile.entries["CommonScriptLoad"].arrays[i].strings[0]));
                for (int i2 = 0; i2 < temp.entries.Count; i2++)
                {
                    if (temp.entries[temp.entryNames[i2]].subroutine)
                        o.commonSubroutines.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
                }
            }

        if(loadFile.entries.ContainsKey("ColLoad"))
            for(int i = 0; i < loadFile.entries["ColLoad"].count; i++)
            {
                collisionFile temp = DPS_FileReader.loadCollision(Resources.Load<TextAsset>("Char/" + id + "/" + loadFile.entries["ColLoad"].arrays[i].strings[0]));
                for (int i2 = 0; i2 < temp.entries.Count; i2++)
                    o.collisions.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
            }
    }
}
