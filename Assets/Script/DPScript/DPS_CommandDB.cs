using System;
using System.Collections.Generic;
using System.IO;
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
                    temp.args = new string[] { "1h", "1c" };
                    break;
                case 15:
                    temp.args = new string[] { "1h", "1c" };
                    break;
                case 16:
                    temp.args = new string[] { "1h", "1c" };
                    break;
                case 17:
                    temp.args = new string[] { "1n" };
                    break;
                case 18:
                    temp.args = new string[] { "2i" };
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
                case 110:
                    temp.args = new string[] { "1s" };
                    break;
                case 111:
                case 112:
                case 113:
                case 114:
                    temp.args = new string[] { "1n" };
                    break;
            }

            return temp;
        }
    }
}
