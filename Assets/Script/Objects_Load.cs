using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;

public class Objects_Load
{
    DPS_FileReader fileReader = new DPS_FileReader();


    public void mainLoad(string path, GameWorldObject o, string id, bool debug = false)
    {
        dataArray_File loadFile = fileReader.loadDataArray(Resources.Load<TextAsset>(path));

        if(loadFile.entries.ContainsKey("ScriptLoad"))
            for(int i = 0; i < loadFile.entries["ScriptLoad"].count; i++)
            {
                scriptFile temp = fileReader.loadScript(Resources.Load<TextAsset>("Char/"+ id + "/" + loadFile.entries["ScriptLoad"].arrays[i].strings[0]));
                for(int i2 = 0; i2 < temp.entries.Count; i2++)
                {
                    if (temp.entries[temp.entryNames[i2]].subroutine)
                        o.subroutines.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
                    else
                        o.states.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
                }
            }

        if(loadFile.entries.ContainsKey("ColLoad"))
            for(int i = 0; i < loadFile.entries["ColLoad"].count; i++)
            {
                collisionFile temp = fileReader.loadCollision(Resources.Load<TextAsset>("Char/" + id + "/" + loadFile.entries["ColLoad"].arrays[i].strings[0]));
                for (int i2 = 0; i2 < temp.entries.Count; i2++)
                    o.collisions.Add(temp.entries[temp.entryNames[i2]].name, temp.entries[temp.entryNames[i2]]);
            }
    }
}
