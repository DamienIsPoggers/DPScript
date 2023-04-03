using System.Collections;
using System.Collections.Generic;

public class InputSwitchCase
{
    public byte[] switchCase(short type)
    {
        switch(type)
        {
            default:
                return new byte[] { 5 };
            case 0:
                return new byte[] {};
            case 10:
                return new byte[] { 2, 3, 6 };
            case 11:
                return new byte[] { 2, 1, 4 };
            case 15:
                return new byte[] { 6, 3, 2, 1, 4, 6 };
            case 16:
                return new byte[] { 4, 1, 2, 3, 6, 4 };
        }
    }
}
