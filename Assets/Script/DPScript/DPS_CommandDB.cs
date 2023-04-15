using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace DPScript
{
    public class DPS_CommandDB
    {
        public struct Command
        {
            public string name;
            public int id;
            public string[] args;
        }

        DPS_FileReader fileReader = new DPS_FileReader();
        dataArray_Entry db;
        List<Command> commands = new List<Command>();

        public Command getCommand(int id)
        {
            Command temp = new Command();

            switch(id)
            {
                default:
                    Debug.Log("Command " + id + " doesnt exist in the db");
                    break;
                case 1:
                case 2:
                    temp.args = new string[] { "1s", "1u" };
                    break;
                case 3:
                    temp.args = new string[] { "1n" };
                    break;
                case 4:
                case 5:
                    temp.args = new string[] { "1u" };
                    break;
                case 6:
                    temp.args = new string[] { "1s" };
                    break;
                case 7:
                    temp.args = new string[] { "1s", "1h", "2i" };
                    break;
                case 8:
                    temp.args = new string[] { "1s" };
                    break;
                case 9:
                    temp.args = new string[] { "1s", "4i", "1s" };
                    break;
                case 10:
                    temp.args = new string[] { "1s" };
                    break;
                case 11:
                    temp.args = new string[] { "1s", "4i", "1s" };
                    break;
                case 12:
                    temp.args = new string[] { "1h", "1c" };
                    break;
                case 13:
                    temp.args = new string[] { "1n" };
                    break;
                case 14:
                case 15:
                case 16:
                    temp.args = new string[] { "1h", "1c" };
                    break;
                case 17:
                    temp.args = new string[] { "1n" };
                    break;
                case 18:
                    temp.args = new string[] { "2i" };
                    break;
                case 19:
                case 20:
                    temp.args = new string[] { "1h", "2i" };
                    break;
                case 23:
                    temp.args = new string[] { "1h", "1c", "1m", "1h", "1c" };
                    break;
                case 24:
                    temp.args = new string[] { "1h" };
                    break;
                case 30:
                    temp.args = new string[] { "1h" };
                    break;
                case 31:
                    temp.args = new string[] { "1n" };
                    break;
                case 32:
                case 33:
                    temp.args = new string[] { "1h" };
                    break;
                case 34:
                    temp.args = new string[] { "1h", "1s", "3f", "1u" };
                    break;
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                    temp.args = new string[] { "1h", "1c" };
                    break;
                case 100:
                    temp.args = new string[] { "1s" };
                    break;
                case 101:
                    temp.args = new string[] { "7h" };
                    break;
                case 102:
                    temp.args = new string[] { "1h" };
                    break;
                case 103:
                    temp.args = new string[] { "8h" };
                    break;
                case 104:
                    temp.args = new string[] { "1h", "1s" };
                    break;
                case 105:
                    temp.args = new string[] { "1u" };
                    break;
                case 106:
                    temp.args = new string[] { "1n" };
                    break;
                case 107:
                case 108:
                    temp.args = new string[] { "1s" };
                    break;
                case 109:
                    temp.args = new string[] { "1h", "1s" };
                    break;
                case 110:
                    temp.args = new string[] { "1s" };
                    break;
                case 111:
                case 112:
                case 113:
                case 114:
                    temp.args = new string[] { "1n" };
                    break;
                case 119:
                    temp.args = new string[] { "1s" };
                    break;
                case 120:
                    temp.args = new string[] { "1h" };
                    break;
                case 121:
                case 122:
                    temp.args = new string[] { "1s" };
                    break;
                case 123:
                case 124:
                    temp.args = new string[] { "1b" };
                    break;
                case 125:
                    temp.args = new string[] { "1n" };
                    break;
                case 126:
                    temp.args = new string[] { "1s", "1f" };
                    break;
                case 127:
                    temp.args = new string[] { "1s", "1h", "2u", "6f" };
                    break;
                case 128:
                    temp.args = new string[] { "1u", "1s", "3i" };
                    break;
                case 150:
                case 151:
                case 152:
                case 153:
                    temp.args = new string[] { "1h", "1c" };
                    break;
                case 154:
                    temp.args = new string[] { "1b" };
                    break;
                case 155:
                case 156:
                    temp.args = new string[] { "1h", "1c" };
                    break;
                case 157:
                    temp.args = new string[] { "2h" };
                    break;
                case 158:
                case 159:
                    temp.args = new string[] { "1i" };
                    break;
                case 160:
                    temp.args = new string[] { "1f" };
                    break;
                case 161:
                    temp.args = new string[] { "1h", "1u" };
                    break;
                case 162:
                    temp.args = new string[] { "1u" };
                    break;
                case 163:
                    temp.args = new string[] { "1h", "1s", "3f", "1u" };
                    break;
                case 164:
                    temp.args = new string[] { "1h" };
                    break;
                case 165:
                    temp.args = new string[] { "1f" };
                    break;
                case 166:
                    temp.args = new string[] { "6h" };
                    break;
            }

            return temp;
        }
    }
}
