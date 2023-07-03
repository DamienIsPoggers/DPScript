using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;
using DPScript.Loading;
using System.Linq;
using UnityEditor;

public class Objects_Load
{

    public IEnumerator mainLoad(DPS_ObjectLoad load, int color)
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

        if (load.materials.mats.Count == 0)
            yield break;

        if(o.useArmature)
            for(int i = 0; i < load.materials.mats[color].materials.Count; i++)
            {
                DPS_ObjectMat mat = load.materials.mats[color].materials[i];
                if (!o.renderers.ContainsKey(mat.name))
                    continue;
                o.renderers[mat.name].materials = mat.materials.ToArray();
            }
                
    }

    public void debugLoad(string path, GameWorldObject o, string id, bool debug = false)
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
