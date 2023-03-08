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
            case 10:
                return new byte[] { 2, 3, 6 };
            case 11:
                return new byte[] { 2, 1, 4 };
        }
    }
}
