using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;

public class CharacterSelect : MonoBehaviour
{
    struct Character
    {
        uint idNum;
        string idStr;
        string name;
        uint trainingCssOrder;

        public Character(uint num, string strS, string strL, uint order)
        {
            idNum = num;
            idStr = strS;
            name = strL;
            trainingCssOrder = order;
        }
    }

    Dictionary<uint, Character> characterEntries;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("css start");
        dataArray_File tempCharacterTable;
        DPS_FileReader tempFileReader = new DPS_FileReader();
        //TextAsset temp = Resources.Load<TextAsset>("Script/characterTable");
        //Debug.Log(temp.text);
        tempCharacterTable = tempFileReader.loadDataArray(Resources.Load<TextAsset>("Script/characterTable"));
        //Debug.Log(tempCharacterTable.signiture);

        dataArray_Entry tempEntry = tempCharacterTable.entries["CharacterEntries"];
        for (int i = 0; i < tempEntry.count; i++)
            characterEntries.Add(tempEntry.arrays[i].uints[0], new Character(tempEntry.arrays[i].uints[0], tempEntry.arrays[i].strings[0],
                tempEntry.arrays[i].strings[1], tempEntry.arrays[i].uints[1]));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
