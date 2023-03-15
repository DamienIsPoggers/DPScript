using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript
{
    #region script
    public class scriptCommand
    {
        public int id;
        public List<string> stringArgs = new List<string>();
        public List<int> intArgs = new List<int>();
        public List<uint> uintArgs = new List<uint>();
        public List<byte> byteArgs = new List<byte>();
        public List<float> floatArgs = new List<float>();
        public List<bool> boolArgs = new List<bool>();
        public List<scriptCommand> commands = new List<scriptCommand>();
    }

    public class scriptEntry
    {
        public string name;
        public bool subroutine;
        public List<scriptCommand> commands = new List<scriptCommand>();
    }

    public class scriptFile
    {
        public ushort version;
        public string signiture;
        public Dictionary<string, scriptEntry> entries = new Dictionary<string, scriptEntry>();
        public List<string> entryNames = new List<string>();
    }

    public class uponEntry
    {
        public byte type;
        public List<scriptCommand> commands = new List<scriptCommand>();
    }
    #endregion

    #region collision
    public class collisionBox
    {
        public int[] x = new int[2];
        public int[] y = new int[2];
        public int[] z = new int[2];
        public byte id;
        public byte type;
    }

    public class collisionChunk
    {
        public byte sprite;
        public byte id;
        public int[] uv = new int[4];
        public int[] origin = new int[2];
        public Vector2 scale;
        public Vector3 rotation;
    }

    public class collisionEntry
    {
        public string name;
        public byte boxCount;
        public byte chunkCount;
        public bool sphere = false;
        public bool hasZ = false;
        public List<string> sprites = new List<string>();
        public List<collisionChunk> chunks = new List<collisionChunk>();
        public List<collisionBox> boxes = new List<collisionBox>();
    }

    public class collisionFile
    {
        public ushort version;
        public string signiture;
        public Dictionary<string, collisionEntry> entries = new Dictionary<string, collisionEntry>();
        public List<string> entryNames = new List<string>();
    }
    #endregion


    #region data_array

    public class dataArray_Table
    {
        public List<string> strings = new List<string>();
        public List<int> ints = new List<int>();
        public List<uint> uints = new List<uint>();
        public List<byte> bytes = new List<byte>();
        public List<float> floatArgs = new List<float>();
        public List<bool> bools = new List<bool>();
    }

    public class dataArray_Entry
    {
        public string name;
        public short count;
        public byte[] fileStruct;
        public List<dataArray_Table> arrays = new List<dataArray_Table>();
    }

    public class dataArray_File
    {
        public ushort version;
        public string signiture;
        public Dictionary<string, dataArray_Entry> entries = new Dictionary<string, dataArray_Entry>();
    }


    #endregion

    #region misc

    public class StateEntry
    {
        public string name;
        public string button = "";
        public byte input = 5;
        public uint meterCost = 0;
        public byte type = 0;
        public byte useableIn = 0;
        public byte leniantInput = 0;
        public byte attackType = 0;
        public byte holdBuffer = 0;
        public byte common = 0;

        public bool useSubroutine = false;
        public string subroutine;
        public byte subroutineType;

        public StateEntry(string stateName)
        {
            name = stateName;
        }
    }

    #endregion
}
