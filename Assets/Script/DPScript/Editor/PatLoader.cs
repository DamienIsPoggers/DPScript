using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Linq.Expressions;

namespace DPScript.Editor
{
    public class PatLoader : EditorWindow
    {
        List<int> spriteIds = new List<int>(), effectIds = new List<int>(), imageIDs = new List<int>();
        Dictionary<int, patSprite> sprites = new Dictionary<int, patSprite>();
        Dictionary<int, patEffect> effects = new Dictionary<int, patEffect>();
        patShapeHeader shapes = new patShapeHeader();
        Dictionary<int, patImageHeader> imageHeaders = new Dictionary<int, patImageHeader>();

        string patLoadPath = "", pathOfMat = "";
        int createSpriteId = 0;

        [MenuItem("Window/DPScript/Pat Loader")]
        public static void create()
        {
            GetWindow<PatLoader>("Pat Loader");
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            patLoadPath = EditorGUILayout.TextField(patLoadPath, GUILayout.Width(150));
            GUILayout.Label("Path on pc to load Pat");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Load", GUILayout.Width(100)) && patLoadPath != string.Empty)
                loadPat(patLoadPath.Replace("\\", "/"));
            if (GUILayout.Button("UnLoad", GUILayout.Width(100)))
                unloadPat();
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Data Loaded: sprites = " + sprites.Count + ", effects = " + effects.Count + ", shapes = " + shapes.shapes.Count + ", images = " + imageHeaders.Count);

            GUILayout.Label("");
            EditorGUILayout.BeginHorizontal();
            createSpriteId = EditorGUILayout.IntField(createSpriteId, GUILayout.Width(75));
            GUILayout.Label("Sprite to create");
            pathOfMat = EditorGUILayout.TextField(pathOfMat, GUILayout.Width(150));
            GUILayout.Label("Path of material to use (in unity project)");
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Create Sprite", GUILayout.Width(100)))
                createSprite(createSpriteId);
        }

        void createSprite(int id)
        {
            if (!sprites.ContainsKey(id))
            {
                Debug.Log("Sprite doesnt exist");
                return;
            }

            Material mat = AssetDatabase.LoadAssetAtPath<Material>(pathOfMat);

            int[] tX = { 0, 1, 1, 1, 0, 0 };
            int[] tY = { 0, 0, 1, 1, 1, 0 };
            int[] tXI = { 0, 1, 1, 1, 0, 0 };
            int[] tYI = { 1, 1, 0, 0, 0, 1 };

            patSprite spr = sprites[id];
            GameObject parentObj = new GameObject("Sprite " + id);
            for(int i = 0; i < spr.layers.Count; i++)
            {
                spriteObj obj = spr.layers[spr.layerIds[i]];
                patEffect eff = effects[obj.ppId];
                GameObject child = new GameObject("Object" + spr.layerIds[i]);
                child.transform.parent = parentObj.transform;
                child.transform.localPosition = new Vector3((float)obj.posX / 1000, (float)obj.posY / 1000);
                child.transform.localEulerAngles = new Vector3(obj.rotX * 360, obj.rotY * 360, obj.rotZ * 360);
                child.transform.localScale = new Vector3(obj.scaleX, obj.scaleY);
                Mesh mesh = new Mesh();
                List<Vector3> verts = new List<Vector3>(), normals = new List<Vector3>();
                List<Vector2> uv = new List<Vector2>();
                int[] tris = new int[] { 0 };
                float width = 256f, height = 256f;
                if (imageHeaders.ContainsKey(eff.textureIndex))
                {
                    width = imageHeaders[eff.textureIndex].width / 256f;
                    height = imageHeaders[eff.textureIndex].width / 256f;
                }
                else
                {
                    Debug.Log("Texture " + eff.textureIndex + " doesnt exist, on eff " + obj.ppId + ", using defualt");
                    width = imageHeaders[imageIDs[0]].width / 256f;
                    height = imageHeaders[imageIDs[0]].height / 256f;
                }
                switch (shapes.shapes[eff.shapeIndex].type)
                {
                    //plane
                    default:
                    case 1:
                    case 2:
                        for (int j = 0; j < 6; j++)
                        {
                            verts.Add(new Vector3(-(float)(eff.centerX / 250) + (float)eff.sizeX / 250 * tX[j],
                                -(float)(eff.centerY / 250) + (float)eff.sizeY / 250 * tY[j], 0));

                            uv.Add(new Vector2((eff.uvX + eff.uvW * tX[j]) / width / 49f, (eff.uvY + eff.uvH * tY[j]) / height / 49f));
                            normals.Add(Vector3.one);
                        }
                        tris = new int[] { 0, 1, 2, 4, 5, 3 };
                        break;
                    //ring
                    case 3:
                    case 4:

                        break;
                }
                mesh.SetVertices(verts);
                mesh.SetUVs(0, uv);
                mesh.SetTriangles(tris, 0);
                mesh.SetNormals(normals);
                mesh.RecalculateNormals();
                child.AddComponent<MeshRenderer>();
                child.GetComponent<MeshRenderer>().rendererPriority = obj.priority;
                child.GetComponent<MeshRenderer>().material = mat;
                child.AddComponent<MeshFilter>();
                child.GetComponent<MeshFilter>().mesh = mesh;
            }
        }

        #region pat loading shit
        void unloadPat()
        {
            spriteIds.Clear();
            effectIds.Clear();
            imageIDs.Clear();
            sprites.Clear();
            effects.Clear();
            shapes.names.Clear();
            shapes.shapes.Clear();
            imageHeaders.Clear();
            Debug.Log("Succsesfully unloaded");
        }

        public void loadPat(string path)
        {
            unloadPat();
            BinaryReader file = new BinaryReader(File.Open(path, FileMode.Open));
            if (Encoding.ASCII.GetString(file.ReadBytes(12)) != "PAniDataFile")
            {
                Debug.Log("not valid pat");
                return;
            }
            file.ReadBytes(20);

            while (Encoding.ASCII.GetString(file.ReadBytes(4)) != "_END" || file.BaseStream.Position >= file.BaseStream.Position)
            {
                string arg = Encoding.ASCII.GetString(file.ReadBytes(4));
                //Console.WriteLine(arg);
                //Console.ReadLine();
                switch (arg)
                {
                    default:
                        Debug.Log("Unknown Argument at loadPat: " + BitConverter.ToInt32(Encoding.ASCII.GetBytes(arg), 0).ToString() + " at position: " + file.BaseStream.Position);
                        file.Close();
                        return;
                    case "_STR":
                    case "_END":
                        break;
                    case "P_ST":
                        loadSprite(file);
                        break;
                    case "PPST":
                        loadEff(file);
                        break;
                    case "VEST":
                        loadShape(file);
                        break;
                    case "PGST":
                        loadPage(file);
                        break;
                }
            }
            file.Close();
            Debug.Log("Succusfully Loaded");
        }

        void loadSprite(BinaryReader file)
        {
            patSprite sprite = new patSprite();
            int id = file.ReadInt32();

            string arg = Encoding.ASCII.GetString(file.ReadBytes(4));
            while (arg != "P_ED")
            {
                switch (arg)
                {
                    default:
                        Console.WriteLine("Unknown Argument at loadSprite: " + arg + " at position: " + file.BaseStream.Position);
                        break;
                    case "PANA":
                        byte length = file.ReadByte();
                        sprite.name = Encoding.ASCII.GetString(file.ReadBytes(length));
                        break;
                    case "PRST":
                        loadObj(sprite, file);
                        break;
                }
                arg = Encoding.ASCII.GetString(file.ReadBytes(4));
            }
            //Console.WriteLine(arg);
            //Console.ReadLine();
            sprites.Add(id, sprite);
            spriteIds.Add(id);
            file.BaseStream.Position -= 4;
        }

        void loadObj(patSprite sprite, BinaryReader file)
        {
            spriteObj layer = new spriteObj();
            int id = file.ReadInt32();

            string arg = Encoding.ASCII.GetString(file.ReadBytes(4));
            while (arg != "PRED")
            {
                //Console.WriteLine(arg);
                //Console.ReadLine();
                switch (arg)
                {
                    default:
                        Console.WriteLine("Unknown Argument at loadPR: " + arg + " at position: " + file.BaseStream.Position);
                        break;
                    case "PRXY":
                        layer.posX = file.ReadInt32();
                        layer.posY = file.ReadInt32();
                        break;
                    case "PRAL":
                        layer.additive = file.ReadBoolean();
                        break;
                    case "PRRV":
                        layer.flip = file.ReadByte();
                        break;
                    case "PRFL":
                        layer.filter = file.ReadBoolean();
                        break;
                    case "PRZM":
                        layer.scaleX = file.ReadSingle();
                        layer.scaleY = file.ReadSingle();
                        break;
                    case "PRSP":
                        layer.colorOverlay = new byte[] { file.ReadByte(), file.ReadByte(), file.ReadByte(), file.ReadByte() };
                        break;
                    case "PRPR":
                        layer.priority = file.ReadInt32();
                        break;
                    case "PRID":
                        layer.ppId = file.ReadInt32();
                        break;
                    case "PRCL":
                        layer.color = new byte[] { file.ReadByte(), file.ReadByte(), file.ReadByte(), file.ReadByte() };
                        break;
                    case "PRA3":
                        file.ReadBytes(4);
                        layer.rotX = file.ReadSingle();
                        layer.rotY = file.ReadSingle();
                        layer.rotZ = file.ReadSingle();
                        break;
                }
                arg = Encoding.ASCII.GetString(file.ReadBytes(4));
            }

            sprite.layers.Add(id, layer);
            sprite.layerIds.Add(id);
        }

        void loadEff(BinaryReader file)
        {
            patEffect pp = new patEffect();
            int id = file.ReadInt32();

            string arg = Encoding.ASCII.GetString(file.ReadBytes(4));
            while (arg != "PPED")
            {
                switch (arg)
                {
                    default:
                        Console.WriteLine("Unknown Argument at loadPP: " + arg + " at position: " + file.BaseStream.Position);
                        break;
                    case "PPNA":
                        byte length = file.ReadByte();
                        pp.name = Encoding.ASCII.GetString(file.ReadBytes(length));
                        break;
                    case "PPCC":
                        pp.centerX = file.ReadInt32();
                        pp.centerY = file.ReadInt32();
                        break;
                    case "PPUV":
                        pp.uvX = file.ReadInt32();
                        pp.uvY = file.ReadInt32();
                        pp.uvW = file.ReadInt32();
                        pp.uvH = file.ReadInt32();
                        break;
                    case "PPSS":
                        pp.sizeX = file.ReadInt32();
                        pp.sizeY = file.ReadInt32();
                        break;
                    case "PPTE":
                        pp.ppte1 = file.ReadUInt16();
                        pp.ppte2 = file.ReadUInt16();
                        break;
                    case "PPPA":
                        pp.paletteNum = file.ReadInt32();
                        break;
                    case "PPTP":
                        pp.textureIndex = file.ReadInt32();
                        break;
                    case "PPPP":
                        pp.shapeIndex = file.ReadInt32();
                        break;
                }
                arg = Encoding.ASCII.GetString(file.ReadBytes(4));
            }

            effects.Add(id, pp);
            effectIds.Add(id);

            file.BaseStream.Position -= 4;
        }

        void loadShape(BinaryReader file)
        {
            shapes.count = file.ReadInt32();
            shapes.entrySize = file.ReadInt32();
            for (int i = 0; i < shapes.count; i++)
            {
                //Console.WriteLine(file.BaseStream.Position);
                //Console.ReadLine();
                patShapes sh = new patShapes();
                sh.type = file.ReadInt32();
                switch (sh.type)
                {
                    //Plane
                    case 1:
                    case 2:
                        file.ReadBytes(4 * (shapes.entrySize - 1));
                        break;
                    //Ring
                    case 3:
                    case 4:
                        file.ReadBytes(8);
                        sh.radius = file.ReadInt32();
                        sh.width = file.ReadInt32();
                        sh.vertexCount = file.ReadInt32();
                        sh.length = file.ReadInt32();
                        sh.dz = file.ReadInt32();
                        sh.dRadius = file.ReadInt32();
                        file.ReadBytes(4 * (shapes.entrySize - 9));
                        break;
                    //Sphere
                    case 5:
                        file.ReadBytes(8);
                        sh.radius = file.ReadInt32();
                        sh.vertexCount = file.ReadInt32();
                        sh.vertexCount2 = file.ReadInt32();
                        sh.length = file.ReadInt32();
                        sh.length2 = file.ReadInt32();
                        file.ReadBytes(4 * (shapes.entrySize - 8));
                        break;
                    //Cone
                    case 6:
                        file.ReadBytes(8);
                        sh.radius = file.ReadInt32();
                        sh.dz = file.ReadInt32();
                        sh.vertexCount = file.ReadInt32();
                        sh.vertexCount2 = file.ReadInt32();
                        sh.length = file.ReadInt32();
                        file.ReadBytes(4 * (shapes.entrySize - 8));
                        break;
                    default:
                        Console.WriteLine("Unknown Shape type: " + sh.type + " at position: " + file.BaseStream.Position);
                        break;
                }

                shapes.shapes.Add(sh);
            }

            file.ReadBytes(4);
            for (int i = 0; i < shapes.count; i++)
                shapes.names.Add(Encoding.ASCII.GetString(file.ReadBytes(32)));
        }

        void loadPage(BinaryReader file)
        {
            int id = file.ReadInt32();
            file.ReadBytes(4);
            patImageHeader pg = new patImageHeader();
            pg.name = Encoding.ASCII.GetString(file.ReadBytes(32)).Replace("\0", string.Empty);
            Debug.Log(pg.name);
            file.ReadBytes(4);
            pg.widthShort = file.ReadUInt16();
            pg.heightShort = file.ReadUInt16();
            file.ReadBytes(8);
            pg.width = file.ReadUInt32();
            pg.height = file.ReadUInt32();
            pg.encoding = (patImageHeader.imageType)file.ReadUInt32();
            pg.DDPF_FOURCC = file.ReadBytes(8);
            file.ReadBytes(8);
            pg.fileSize = file.ReadUInt32();
            pg.aspectRatio = file.ReadUInt32();

            file.BaseStream.Position += pg.fileSize;

            if (pg.width != pg.widthShort || pg.height != pg.heightShort || pg.width * pg.height != pg.aspectRatio - 128)
                Debug.Log("Issues with pg header " + id + ", might cause issues with making uvs");

            imageHeaders.Add(id, pg);
            imageIDs.Add(id);
        }
        #endregion
    }

    #region structs
    public class patSprite //P_ST
    {
        public string name = "";
        public Dictionary<int, spriteObj> layers = new Dictionary<int, spriteObj>();
        public List<int> layerIds = new List<int>();
    }

    public class spriteObj //PRST
    {
        public int posX = 0;
        public int posY = 0;
        public bool additive = false;
        public byte flip = 0;
        public bool filter = false;
        public float scaleX = 1;
        public float scaleY = 1;
        public byte[] color = { 255, 255, 255, 255 };
        public byte[] colorOverlay = { 0, 0, 0, 0 };
        public int priority = 0;
        public int ppId = 0;
        public float rotX = 0;
        public float rotY = 0;
        public float rotZ = 0;
    }

    public class patEffect //PPST
    {
        public string name = "";
        public int centerX = 0;
        public int centerY = 0;
        public int uvX = 0;
        public int uvY = 0;
        public int uvW = 0;
        public int uvH = 0;
        public int sizeX = 0;
        public int sizeY = 0;
        public ushort ppte1 = 0;
        public ushort ppte2 = 0;
        public int paletteNum = 0;
        public int textureIndex = 0;
        public int shapeIndex = 0;
    }
    
    public class patShapeHeader
    {
        public int count = 1;
        public int entrySize = 16;
        public List<patShapes> shapes = new List<patShapes>();
        public List<string> names = new List<string>();
    }

    public class patShapes //VEST
    {
        public int type;
        public int vertexCount;
        public int vertexCount2;
        public int length;
        public int length2;
        public int radius;
        public int dRadius;
        public int width;
        public int dz;
    }

    public class patImageHeader //PGST
    {
        public string name = "";
        public ushort widthShort = 0, heightShort = 0;
        public uint fileSize = 0;
        public uint width = 0, height = 0;
        public imageType encoding = imageType.DXT5;
        public byte[] DDPF_FOURCC;
        public uint aspectRatio = 0;

        public enum imageType
        {
            DXT1 = 827611204,
            DXT5 = 894720068,
            uncompressed = 21,
        }
    }

    #endregion
}