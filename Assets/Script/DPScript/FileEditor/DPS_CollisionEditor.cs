using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace DPScript.Editor
{
    public class DPS_CollisionEditor : MonoBehaviour
    {
        private class editorCollisionEntry
        {
            public string name;
            public bool anim;
            public byte boxCount;
            public byte chunkCount;
            public bool sphere = false;
            public bool hasZ = false;
            public List<string> sprites = new List<string>();
            public List<collisionChunk> chunks = new List<collisionChunk>();
            public List<collisionBox> boxes = new List<collisionBox>();
        }

        private class editorCollisionFile
        {
            public ushort version;
            public string signiture;
            public List<editorCollisionEntry> entries = new List<editorCollisionEntry>();
        }

        [SerializeField]
        GameObject player;
        Dropdown entrySelecter, spriteList;
        InputField nameEntrySet, spriteCount, spriteListName;
        Button newFileButton, newEntryButton;
        Text noEntryText, spriteCountText;

        editorCollisionFile file;
        int currentEntry = 0, currentSprite = 0;

        Mesh Box, Sphere;
        Material colBox, colSphere, hitBox, hitSphere, hurtBox, hurtSphere, miscBox, miscSphere, snapBox, snapShpere;
        private bool fileLoaded = false;

        #region start
        // Start is called before the first frame update
        void Start()
        {
            Box = Resources.Load<Mesh>("UI/HitboxOverlay/Box_Mesh"); Sphere = Resources.Load<Mesh>("UI/HitboxOverlay/Sphere_Mesh");
            colBox = Resources.Load<Material>("UI/HitboxOverlay/Colbox_Mat"); colSphere = Resources.Load<Material>("UI/HitboxOverlay/Colsphere_Mat");
            hitBox = Resources.Load<Material>("UI/HitboxOverlay/Hitbox_Mat"); hitSphere = Resources.Load<Material>("UI/HitboxOverlay/Hitsphere_Mat");
            hurtBox = Resources.Load<Material>("UI/HitboxOverlay/Hurtbox_Mat"); hurtSphere = Resources.Load<Material>("UI/HitboxOverlay/Hurtsphere_Mat");
            miscBox = Resources.Load<Material>("UI/HitboxOverlay/Miscbox_Mat"); miscSphere = Resources.Load<Material>("UI/HitboxOverlay/Miscsphere_Mat");
            snapBox = Resources.Load<Material>("UI/HitboxOverlay/Snapbox_Mat"); snapShpere = Resources.Load<Material>("UI/HitboxOverlay/Snapsphere_Mat");

            newFileButton = transform.Find("New File Button").GetComponent<Button>();
            newFileButton.onClick.AddListener(delegate
            {
                newFile();
            });

            GameObject temp1 = transform.Find("Entry Stuff").gameObject;

            entrySelecter = temp1.transform.Find("Entry Selector").GetComponent<Dropdown>();
            entrySelecter.onValueChanged.AddListener(delegate 
            {
                currentEntry = entrySelecter.value;
                currentSprite = 0;
            });

            nameEntrySet = temp1.transform.Find("Name Input").GetComponent<InputField>();
            nameEntrySet.onValueChanged.AddListener(delegate
            {
                if(file.entries.Count > 0)
                {
                    file.entries[currentEntry].name = nameEntrySet.text;
                    reloadEntrySelecter();
                }
            });

            noEntryText = temp1.transform.Find("No Entries Text").GetComponent<Text>();

            newEntryButton = temp1.transform.Find("New Entry Button").GetComponent<Button>();
            newEntryButton.onClick.AddListener(delegate
            {
                newEntry();
            });

            spriteCountText = temp1.transform.Find("Sprite Count Text").GetComponent<Text>();

            spriteList = temp1.transform.Find("Sprite List").GetComponent<Dropdown>();
            spriteList.onValueChanged.AddListener(delegate
            {
                currentSprite = spriteList.value;
            });

            spriteCount = temp1.transform.Find("Sprite Count").GetComponent<InputField>();
            spriteCount.onValueChanged.AddListener(delegate
            {

            });


            GameObject temp2 = transform.Find("Chunk Stuff").gameObject;

            


            entrySelecter.ClearOptions();
            noEntryText.enabled = false;
        }
        #endregion

        // Update is called once per frame
        void Update()
        {
            if (file.entries.Count == 0)
            {
                noEntryText.enabled = true;
                return;
            }
            else
                noEntryText.enabled = false;

            if (!fileLoaded)
                return;

            if (entrySelecter.options.Count != file.entries.Count)
                reloadEntrySelecter();

        }

        void newFile()
        {
            file = new editorCollisionFile();
            fileLoaded = true;
            newFileButton.GetComponent<RectTransform>().sizeDelta = new Vector2(190, 64);
            newFileButton.GetComponent<RectTransform>().localPosition = new Vector3(544, 328);
            newFileButton.GetComponentInChildren<Text>().fontSize = 40;
            //Debug.Log("added new file");
        }

        void newEntry()
        {
            file.entries.Add(new editorCollisionEntry());
        }

        void reloadEntrySelecter()
        {
            entrySelecter.ClearOptions();
            List<string> names = new List<string>();
            for (int i = 0; i < file.entries.Count; i++)
                names.Add(file.entries[i].name);
            entrySelecter.AddOptions(names);
        }
    }
}