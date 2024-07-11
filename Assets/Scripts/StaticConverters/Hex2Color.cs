using UnityEngine;
public class Hex2Color
{
    public static Color Convert(string hexCode)
    {
        if (ColorUtility.TryParseHtmlString(hexCode, out Color color))
        {
            return color;
        }
        else
            return new Color();
    }
}