using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace DPScript.Editor
{
    public class BoxEditor : EditorWindow
    {
        collisionFile col;
        string saveToString = "", loadedCol = "";
        List<EditorCollisionBox> boxes = new List<EditorCollisionBox>();
        bool sprite = false;
        uint boxLookingAt = 0;
        entry copiedEntry;
        box copiedBox;

        [MenuItem("Window/DPScript/Collision Editor")]
        public static void ShowMyWindow()
        {
            GetWindow<BoxEditor>("Collision Editor");
        }

        void OnGUI()
        {
            foreach (EditorCollisionBox b in boxes)
                if (b == null)
                    boxes.Remove(b);
            #region top
            EditorGUILayout.BeginHorizontal();
            saveToString = EditorGUILayout.TextField(saveToString, GUILayout.Width(150f));
            GUILayout.Label("Path on pc to load/save to");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load", GUILayout.Width(70f)))
                load();
            if (GUILayout.Button("Save", GUILayout.Width(70f)))
                save();
            if (GUILayout.Button("Clear", GUILayout.Width(70f)))
            { col = null; Debug.Log("Unloaded collision"); }
            if (GUILayout.Button("New", GUILayout.Width(70f)))
                col = new collisionFile();
            EditorGUILayout.EndHorizontal();

            if (col == null)
            {
                GUILayout.Label("Nothing is loaded!");
                return;
            }

            EditorGUILayout.BeginHorizontal();
            string tempCol = EditorGUILayout.TextField(loadedCol, GUILayout.Width(120f));
            if (tempCol != loadedCol)
                updateBoxes();
            loadedCol = tempCol;
            GUILayout.Label("Collision to edit");
            if(!col.entries.ContainsKey(loadedCol))
            {
                GUILayout.Label("Collision doesnt exist");
                if (GUILayout.Button("Create", GUILayout.Width(75f)))
                    newCol();
                return;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Update Anim", GUILayout.Width(100f)))
                updateSprite();
            sprite = EditorGUILayout.Toggle(sprite, GUILayout.Width(15f));
            GUILayout.Label("Is a sprite");
            EditorGUILayout.EndHorizontal();
            #endregion

            #region bottom
            EditorGUILayout.BeginHorizontal();
            if (col.entries[loadedCol].sprites.Count == 0)
                col.entries[loadedCol].sprites.Add("");
            col.entries[loadedCol].sprites[0] = EditorGUILayout.TextField(col.entries[loadedCol].sprites[0], GUILayout.Width(125f));
            GUILayout.Label("Sprite/Anim to use");
            EditorGUILayout.EndHorizontal();

            if (boxes.Count == 0 && col.entries[loadedCol].boxCount > 0)
                updateBoxes();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Box", GUILayout.Width(90f)))
                addBox();
            if (GUILayout.Button("Remove Box", GUILayout.Width(90f)))
                removeBox();
            GUILayout.Label("Boxes in collision: " + col.entries[loadedCol].boxCount, GUILayout.Width(125f));
            col.entries[loadedCol].sphere = EditorGUILayout.Toggle(col.entries[loadedCol].sphere, GUILayout.Width(15f));
            GUILayout.Label("Boxes are spheres");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Box looking at", GUILayout.Width(86f));
            if (GUILayout.Button("-", GUILayout.Width(25f)))
                boxLookingAt--;
            int tempInt = EditorGUILayout.IntField((int)boxLookingAt, GUILayout.Width(50f));
            if (tempInt < 0)
                tempInt = 0;
            boxLookingAt = (uint)tempInt;
            if (GUILayout.Button("+", GUILayout.Width(25f)))
                boxLookingAt++;
            if (GUILayout.Button("Update Box Rendering", GUILayout.Width(150f)))
                updateBoxes();
            if (boxLookingAt >= col.entries[loadedCol].boxCount)
            {
                GUILayout.Label("          Box doesnt exist");
                EditorGUILayout.EndHorizontal();
                return;
            }
            EditorGUILayout.EndHorizontal();

            collisionBox box = col.entries[loadedCol].boxes[(int)boxLookingAt];
            GUILayout.Label("        X Position                      Y Position                      Z Position");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(25f)))
                box.x[0]--;
            box.x[0] = EditorGUILayout.IntField(box.x[0], GUILayout.Width(60f));
            if (GUILayout.Button("+", GUILayout.Width(25f)))
                box.x[0]++;
            if (GUILayout.Button("-", GUILayout.Width(25f)))
                box.y[0]--;
            box.y[0] = EditorGUILayout.IntField(box.y[0], GUILayout.Width(60f));
            if (GUILayout.Button("+", GUILayout.Width(25f)))
                box.y[0]++;
            if (GUILayout.Button("-", GUILayout.Width(25f)))
                box.z[0]--;
            box.z[0] = EditorGUILayout.IntField(box.z[0], GUILayout.Width(60f));
            if (GUILayout.Button("+", GUILayout.Width(25f)))
                box.z[0]++;
            EditorGUILayout.EndHorizontal();
            GUILayout.Label("        X Width                           Y Width                          Z Width");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(25f)))
                box.x[1]--;
            box.x[1] = EditorGUILayout.IntField(box.x[1], GUILayout.Width(60f));
            if (GUILayout.Button("+", GUILayout.Width(25f)))
                box.x[1]++;
            if (GUILayout.Button("-", GUILayout.Width(25f)))
                box.y[1]--;
            box.y[1] = EditorGUILayout.IntField(box.y[1], GUILayout.Width(60f));
            if (GUILayout.Button("+", GUILayout.Width(25f)))
                box.y[1]++;
            if (GUILayout.Button("-", GUILayout.Width(25f)))
                box.z[1]--;
            box.z[1] = EditorGUILayout.IntField(box.z[1], GUILayout.Width(60f));
            if (GUILayout.Button("+", GUILayout.Width(25f)))
                box.z[1]++;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            box.type = (byte)EditorGUILayout.IntField(box.type, GUILayout.Width(75f));
            GUILayout.Label("Box type");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            box.id = (byte)EditorGUILayout.IntField(box.id, GUILayout.Width(75f));
            GUILayout.Label("Box id");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy Collision", GUILayout.Width(95f)))
                copyCol();
            if (GUILayout.Button("Paste Collision", GUILayout.Width(95f)))
                pasteCol();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy Box", GUILayout.Width(95f)))
                copyBox();
            if (GUILayout.Button("Paste Box", GUILayout.Width(95f)))
                pasteBox();
            EditorGUILayout.EndHorizontal();

            #endregion
        }

        void save()
        {
            if (col == null)
            {
                Debug.Log("Nothing loaded to save");
                return;
            }

            CollisionSaver.writeToBinary(saveToString, 0, col);
        }

        void load()
        {
            if(!File.Exists(saveToString))
            {
                Debug.Log("File does not exist");
                return;
            }

            col = DPS_FileReader.loadCollision(saveToString);
            Debug.Log("Loaded collision");
        }

        void newCol()
        {
            col.entries.Add(loadedCol, new collisionEntry());
            col.entryNames.Add(loadedCol);
            col.entries[loadedCol].name = loadedCol;
        }

        void updateSprite()
        {
            if (Selection.activeGameObject.GetComponent<GameWorldObject>() == null)
                return;

            string state = col.entries[loadedCol].sprites[0].Remove(col.entries[loadedCol].sprites[0].LastIndexOf("_"));
            int frame = Int32.Parse(col.entries[loadedCol].sprites[0].Remove(0, col.entries[loadedCol].sprites[0].LastIndexOf("_") + 1));

            int stateId = Animator.StringToHash(state);

            if(sprite)
            {
                Animator spriteAnimator = Selection.activeTransform.Find("Sprites").GetComponent<Animator>();
                if (!spriteAnimator.HasState(0, stateId))
                    return;
                spriteAnimator.Play(stateId, 0, (float)frame * (1f / 12f) / spriteAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
                spriteAnimator.speed = 0f;
                spriteAnimator.Update(0.01f);
            }
            else
            {
                Transform mesh = Selection.activeTransform.Find("Mesh");
                mesh.LookAt(GameObject.Find("Main Camera").transform);
                mesh.localEulerAngles = new Vector3(0, mesh.localEulerAngles.y, 0);
                foreach(Transform child in mesh)
                {
                    Animator anim = child.GetComponent<Animator>();
                    if (!anim.HasState(0, stateId))
                        return;
                    anim.Play(stateId, 0, (float)frame * (1f / 12f) / anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
                    anim.speed = 0f;
                    anim.Update(0.01f);
                }
            }
        }

        void updateBoxes()
        {
            if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<GameWorldObject>() == null || !col.entries.ContainsKey(loadedCol))
            {
                foreach (EditorCollisionBox box in boxes)
                    if(box != null)
                        DestroyImmediate(box.gameObject);
                boxes.Clear();
                return;
            }

            if(boxes.Count != col.entries[loadedCol].boxCount)
            {
                if (boxes.Count > col.entries[loadedCol].boxCount)
                    for(int i = col.entries[loadedCol].boxCount; i > boxes.Count; i--)
                    {
                        DestroyImmediate(boxes[i].gameObject);
                        boxes.RemoveAt(i);
                    }
                else if (boxes.Count < col.entries[loadedCol].boxCount)
                    for(int i = boxes.Count; i < col.entries[loadedCol].boxCount; i++)
                    {
                        Transform temp = Selection.activeTransform.Find("Collision");
                        GameObject box = new GameObject("Temp");
                        box.transform.parent = temp;
                        box.transform.localPosition = Vector3.zero;
                        box.transform.localEulerAngles = Vector3.zero;
                        box.AddComponent<EditorCollisionBox>();
                        box.GetComponent<EditorCollisionBox>().update(col.entries[loadedCol].boxes[i], col.entries[loadedCol].sphere);
                        boxes.Add(box.GetComponent<EditorCollisionBox>());
                    }
            }
            else
                for(int i = 0; i < col.entries[loadedCol].boxCount; i++)
                    boxes[i].update(col.entries[loadedCol].boxes[i], col.entries[loadedCol].sphere);
        }

        void addBox()
        {
            col.entries[loadedCol].boxCount++;
            col.entries[loadedCol].boxes.Add(new collisionBox());
            updateBoxes();
        }

        void removeBox()
        {
            col.entries[loadedCol].boxCount--;
            col.entries[loadedCol].boxes.RemoveAt((int)boxLookingAt);
            updateBoxes();
        }

        void copyCol()
        {
            copiedEntry = entry.classToStruct(col.entries[loadedCol]);
        }

        void pasteCol()
        {
            if (!col.entries.ContainsKey(loadedCol))
                return;
            col.entries[loadedCol] = entry.structToClass(copiedEntry, loadedCol);
        }
        
        void copyBox()
        {
            if (!col.entries.ContainsKey(loadedCol) || col.entries[loadedCol].boxCount <= boxLookingAt)
                return;
            copiedBox = box.classToStruct(col.entries[loadedCol].boxes[(int)boxLookingAt]);
        }

        void pasteBox()
        {
            if (!col.entries.ContainsKey(loadedCol) || col.entries[loadedCol].boxCount <= boxLookingAt)
                return;
            col.entries[loadedCol].boxes[(int)boxLookingAt] = box.structToClass(copiedBox);
        }

        struct entry
        {
            public string name;
            public byte boxCount;
            public byte chunkCount;
            public bool sphere;
            public bool hasZ;
            public List<string> sprites;
            public List<collisionChunk> chunks;
            public List<collisionBox> boxes;

            public static entry classToStruct(collisionEntry e)
            {
                return new entry
                {
                    name = e.name,
                    boxCount = e.boxCount,
                    chunkCount = e.chunkCount,
                    sphere = e.sphere,
                    hasZ = e.hasZ,
                    sprites = e.sprites,
                    chunks = e.chunks,
                    boxes = e.boxes
                };
            }

            public static collisionEntry structToClass(entry e, string newName)
            {
                return new collisionEntry
                {
                    name = newName,
                    boxCount = e.boxCount,
                    chunkCount = e.chunkCount,
                    sphere = e.sphere,
                    hasZ = e.hasZ,
                    sprites = e.sprites,
                    boxes = e.boxes
                };
            }
        }

        struct box
        {
            public int[] x;
            public int[] y;
            public int[] z;
            public byte id;
            public byte type;

            public static box classToStruct(collisionBox b)
            {
                return new box
                {
                    x = b.x,
                    y = b.y,
                    z = b.z,
                    id = b.id,
                    type = b.type,
                };
            }

            public static collisionBox structToClass(box b)
            {
                return new collisionBox
                {
                    x = b.x,
                    y = b.y,
                    z = b.z,
                    id = b.id,
                    type = b.type,
                };
            }
        }
    }
}