public class InputSwitchCase
{
    public static byte[] switchCase(short type)
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
            case 12:
                return new byte[] { 6, 2, 3 };
            case 13:
                return new byte[] { 4, 2, 1 };
            case 15:
                return new byte[] { 6, 3, 2, 1, 4, 6 };
            case 16:
                return new byte[] { 4, 1, 2, 3, 6, 4 };
            case 17:
                return new byte[] { 2, 3, 6, 2, 3, 6 };
            case 18:
                return new byte[] { 2, 1, 4, 2, 1, 4 };
            case 19:
                return new byte[] { 6, 3, 2, 1, 4 };
            case 20:
                return new byte[] { 4, 1, 2, 3, 6 };
            case 44:
                return new byte[] { 4, 4 };
            case 66:
                return new byte[] { 6, 6 };
        }
    }
}
