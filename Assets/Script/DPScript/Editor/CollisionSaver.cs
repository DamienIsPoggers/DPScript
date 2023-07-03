using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DPScript.Editor
{
    public static class CollisionSaver
    { 
        public static void writeToBinary(string outPath, ushort version, collisionFile colfile)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(Encoding.ASCII.GetBytes("DPS |").ToList()); buffer.Add(0x01); //header and collision type file
            buffer.AddRange(BitConverter.GetBytes(colfile.entries.Count)); //entry count
            buffer.AddRange(BitConverter.GetBytes(version)); //file version
            string signiture = "DIPsUnityEditorGUI"; byte[] signSize = { 18, 0x00 };
            buffer.AddRange(signSize); buffer.AddRange(Encoding.ASCII.GetBytes(signiture).ToList()); //signiture
            byte[] padding = { 0x00, 0x00, 0x00, 0x00, 0x00 }; buffer.AddRange(padding); //padding at the end of header


            for (int i = 0; i < colfile.entries.Count; i++)
            {
                collisionEntry entry = colfile.entries[colfile.entryNames[i]];
                buffer.Add(BitConverter.GetBytes(entry.name.Length)[0]);
                buffer.AddRange(Encoding.ASCII.GetBytes(entry.name));
                buffer.Add(entry.boxCount);
                buffer.Add(entry.chunkCount);
                if (entry.sphere)
                    buffer.Add(0x01);
                else
                    buffer.Add(0x00);

                if (entry.hasZ)
                    buffer.Add(0x01);
                else
                    buffer.Add(0x00);

                if (entry.sprites.Count == 0 || entry.sprites[0] == string.Empty)
                    buffer.Add(0x00);
                else
                {
                    buffer.Add(BitConverter.GetBytes(entry.sprites.Count)[0]);
                    for (int i2 = 0; i2 < entry.sprites.Count; i2++)
                    {
                        buffer.Add(BitConverter.GetBytes(entry.sprites[i2].Length)[0]);
                        buffer.AddRange(Encoding.ASCII.GetBytes(entry.sprites[i2]));
                    }
                }

                for (int i2 = 0; i2 < entry.chunkCount; i2++)
                {
                    collisionChunk chunk = entry.chunks[i2];
                    buffer.Add(chunk.sprite);
                    buffer.Add(chunk.id);
                    for (int i3 = 0; i3 < 4; i3++)
                        buffer.AddRange(BitConverter.GetBytes(chunk.uv[i3]));
                    buffer.AddRange(BitConverter.GetBytes(chunk.origin[0]));
                    buffer.AddRange(BitConverter.GetBytes(chunk.origin[1]));
                    buffer.AddRange(BitConverter.GetBytes(chunk.scale[0]));
                    buffer.AddRange(BitConverter.GetBytes(chunk.scale[1]));
                    buffer.AddRange(BitConverter.GetBytes(chunk.rotation[0]));
                    buffer.AddRange(BitConverter.GetBytes(chunk.rotation[1]));
                    buffer.AddRange(BitConverter.GetBytes(chunk.rotation[2]));
                }

                for(int i2 = 0; i2 < entry.boxCount; i2++)
                {
                    collisionBox box = entry.boxes[i2];
                    buffer.Add(box.id);
                    buffer.Add(box.type);
                    buffer.AddRange(BitConverter.GetBytes(box.x[0]));
                    buffer.AddRange(BitConverter.GetBytes(box.x[1]));
                    buffer.AddRange(BitConverter.GetBytes(box.y[0]));
                    buffer.AddRange(BitConverter.GetBytes(box.y[1]));
                    if (box.z[0] > 0 || box.z[1] > 0)
                    {
                        buffer.AddRange(BitConverter.GetBytes(box.z[0]));
                        buffer.AddRange(BitConverter.GetBytes(box.z[1]));
                    }
                }
            }

            FileStream output = File.Create(outPath);
            output.Write(buffer.ToArray(), 0, buffer.Count);
            output.Close();
        }
    }
}
