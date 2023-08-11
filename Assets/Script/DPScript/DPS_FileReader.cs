using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace DPScript
{
    public  static class DPS_FileReader
    {
        public static scriptFile loadScript(TextAsset path)
        {
            scriptFile script = new scriptFile();
            BinaryReader file = new BinaryReader(new MemoryStream(path.bytes));
            string indent = Encoding.ASCII.GetString(file.ReadBytes(6));
            Debug.Assert(indent == "DPS |\0"); //Debug.Log(indent);

            uint entryCount = file.ReadUInt32();
            script.version = file.ReadUInt16();
            script.signiture = Encoding.ASCII.GetString(file.ReadBytes(file.ReadUInt16()));
            //Console.WriteLine(script.signiture);
            file.ReadBytes(5);

            for (int i = 0; i < entryCount; i++)
            {
                scriptEntry entry = new scriptEntry();
                byte nameSize = file.ReadByte();
                //Console.WriteLine(nameSize);
                entry.name = Encoding.ASCII.GetString(file.ReadBytes(nameSize));
                //Debug.Log(entry.name);
                //Console.WriteLine(entry.name);
                uint entrySize = file.ReadUInt32();
                entry.subroutine = file.ReadBoolean();

                uint position = 0;
                while (position < entrySize)
                {
                    position += 4;
                    scriptCommand com = new scriptCommand();
                    com.id = file.ReadInt32();
                    //Debug.Log(com.id);
                    DPS_CommandDB.Command temp = DPS_CommandDB.getCommand(com.id);
                    if (temp.id == -1)
                        Debug.Log("Command db error at " + file.BaseStream.Position + ", entry is " + entry.name);
                    //Console.WriteLine(temp.name);
                    string[] args = temp.args;
                    for (int i2 = 0; i2 < args.Length; i2++)
                    {
                        uint typeCount;
                        UInt32.TryParse(args[i2][0].ToString(), out typeCount);
                        for (int i3 = 0; i3 < typeCount; i3++)
                            position = scriptSwitchCase(com, i2, temp, position, file);
                    }
                    entry.commands.Add(com);
                }
                script.entries.Add(entry.name, entry);
                script.entryNames.Add(entry.name);
            }
            file.Close();
            return script;
        }

        private static uint scriptSwitchCase(scriptCommand com, int i2, DPS_CommandDB.Command temp, uint position, 
            BinaryReader file)
        {
            //Debug.Log(com.id);
            switch (temp.args[i2][1])
            {
                case 's':
                    string tempString = Encoding.ASCII.GetString(file.ReadBytes(file.ReadByte()));
                    com.stringArgs.Add(tempString);
                    position += (uint)tempString.Length + 1;
                    break;
                case 'i':
                    com.intArgs.Add(file.ReadInt32());
                    position += 4;
                    break;
                case 'u':
                    com.uintArgs.Add(file.ReadUInt32());
                    position += 4;
                    break;
                case 'f':
                    com.floatArgs.Add(file.ReadSingle());
                    position += 4;
                    break;
                case 'h':
                    com.byteArgs.Add(file.ReadByte());
                    position++;
                    break;
                case 'b':
                    com.boolArgs.Add(file.ReadBoolean());
                    position++;
                    break;
                case 'c':
                    if (com.byteArgs[com.byteArgs.Count - 1] == 2)
                    {
                        position += 4;
                        scriptCommand com2 = new scriptCommand();
                        com2.id = file.ReadInt32();
                        //Debug.Log(com2.id);
                        DPS_CommandDB.Command temp2 = DPS_CommandDB.getCommand(com2.id);
                        if(temp2.id == -1)
                            Debug.Log("Command db error at " + file.BaseStream.Position);
                        //Debug.Log(temp2.name);
                        string[] args = temp2.args;
                        for (int j2 = 0; j2 < args.Length; j2++)
                        {
                            uint typeCount;
                            UInt32.TryParse(args[j2][0].ToString(), out typeCount);
                            for (int j3 = 0; j3 < typeCount; j3++)
                                position = scriptSwitchCase(com2, j2, temp2, position, file);
                        }
                        com.commands.Add(com2);
                    }
                    else
                    {
                        com.intArgs.Add(file.ReadInt32());
                        position += 4;
                    }
                    break;
                case 'm':
                    com.math = (DPS_MathTypes)file.ReadByte();
                    position++;
                    break;
            }


            return position;
        }

        public static collisionFile loadCollision(string path)
        {
            return loadCollision(new BinaryReader(File.Open(path, FileMode.Open)));
        }

        public static collisionFile loadCollision(TextAsset path)
        {
            return loadCollision(new BinaryReader(new MemoryStream(path.bytes)));
        }

        public static collisionFile loadCollision(BinaryReader file)
        {
            collisionFile collision = new collisionFile();
            string indent = Encoding.ASCII.GetString(file.ReadBytes(5));
            Debug.Assert(indent == "DPS |" && file.ReadByte() == 0x01);

            uint entryCount = file.ReadUInt32();
            collision.version = file.ReadUInt16();
            collision.signiture = Encoding.ASCII.GetString(file.ReadBytes(file.ReadUInt16()));
            file.ReadBytes(5);

            for(int i = 0; i < entryCount; i++)
            {
                collisionEntry entry = new collisionEntry();
                byte nameSize = file.ReadByte();
                entry.name = Encoding.ASCII.GetString(file.ReadBytes(nameSize));
                entry.boxCount = file.ReadByte();
                //Debug.Log(entry.name + " " + entry.boxCount);
                entry.chunkCount = file.ReadByte();
                entry.sphere = file.ReadBoolean();
                entry.hasZ = file.ReadBoolean();

                /*
                Debug.Log(entry.name);
                Debug.Log(entry.boxCount);
                Debug.Log(entry.chunkCount);
                Debug.Log(entry.sphere);
                Debug.Log(entry.hasZ);
                */

                List<string> tempSprites = new List<string>();
                byte spriteCount = file.ReadByte();
                //Debug.Log(spriteCount);
                for (int i2 = 0; i2 < spriteCount; i2++)
                {
                    byte stringSize = file.ReadByte();
                    string tempString = Encoding.ASCII.GetString(file.ReadBytes(stringSize));
                    //Debug.Log(i2);
                    tempSprites.Add(tempString);
                }
                entry.sprites = tempSprites;

                for(int i2 = 0; i2 < entry.chunkCount; i2++)
                {
                    collisionChunk chunk = new collisionChunk();
                    chunk.sprite = file.ReadByte();
                    chunk.id = file.ReadByte();
                    chunk.uv = new int[] { file.ReadInt32(), file.ReadInt32(), file.ReadInt32(), file.ReadInt32() };
                    //Debug.Log(chunk.uv[0]); Debug.Log(chunk.uv[1]); Debug.Log(chunk.uv[2]); Debug.Log(chunk.uv[3]);
                    chunk.origin = new int[] { file.ReadInt32(), file.ReadInt32() };
                    chunk.scale = new Vector2(file.ReadSingle(), file.ReadSingle());
                    chunk.rotation = new Vector3(file.ReadSingle(), file.ReadSingle(), file.ReadSingle());
                    entry.chunks.Add(chunk);
                }

                for(int i2 = 0; i2 < entry.boxCount; i2++)
                {
                    collisionBox box = new collisionBox();
                    box.id = file.ReadByte();
                    box.type = file.ReadByte();
                    box.x = new int[] { file.ReadInt32(), file.ReadInt32() };
                    box.y = new int[] { file.ReadInt32(), file.ReadInt32() };
                    if (entry.hasZ)
                        box.z = new int[] { file.ReadInt32(), file.ReadInt32() };
                    entry.boxes.Add(box);
                }
                //Debug.Log(entry.name); 
                if (collision.entries.ContainsKey(entry.name))
                {
                    Debug.Log(entry.name + ", entry found at " + file.BaseStream.Position);
                    //Debug.Log(collision.entryNames[collision.entryNames.Count - 1]);
                    continue; 
                }
                collision.entries.Add(entry.name, entry);
                collision.entryNames.Add(entry.name);
            }
            file.Close();
            return collision;
        }

        public static dataArray_File loadDataArray(TextAsset path)
        {
            dataArray_File data = new dataArray_File();
            //Debug.Log("loading data array");
            BinaryReader file = new BinaryReader(new MemoryStream(path.bytes));
            //Debug.Log("file loaded into memory");
            string indent = Encoding.ASCII.GetString(file.ReadBytes(5));
            Debug.Assert(indent == "DPS |" && file.ReadByte() == 0x0A);

            uint entryCount = file.ReadUInt32();
            data.version = file.ReadUInt16();
            data.signiture = Encoding.ASCII.GetString(file.ReadBytes(file.ReadUInt16()));
            file.ReadBytes(5);

            for(int i = 0; i < entryCount; i++)
            {
                dataArray_Entry entry = new dataArray_Entry();
                entry.name = Encoding.ASCII.GetString(file.ReadBytes(file.ReadByte()));
                //Debug.Log(entry.name);
                entry.count = file.ReadInt16();
                //Debug.Log(entry.count);
                entry.fileStruct = file.ReadBytes(file.ReadByte());
                for (int i2 = 0; i2 < entry.count; i2++)
                {
                    dataArray_Table temp = new dataArray_Table();
                    for (int i3 = 0; i3 < entry.fileStruct.Length; i3++)
                    {
                        //Debug.Log(entry.fileStruct[i3]);
                        switch(entry.fileStruct[i3])
                        {
                            case 0:
                                temp.ints.Add(file.ReadInt32());
                                break;
                            case 1:
                                temp.uints.Add(file.ReadUInt32());
                                break;
                            case 2:
                                temp.floatArgs.Add(file.ReadSingle());
                                break;
                            case 3:
                                temp.strings.Add(Encoding.ASCII.GetString(file.ReadBytes(file.ReadByte())));
                                break;
                            case 4:
                                temp.bytes.Add(file.ReadByte());
                                break;
                            case 5:
                                temp.bools.Add(file.ReadBoolean());
                                break;
                        }
                    }
                    entry.arrays.Add(temp);
                }
                data.entries.Add(entry.name, entry);
            }
            file.Close();
            return data;
        }
    }
}
