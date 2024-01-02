using UnityEngine;

namespace Runtime.Utilities
{
    public static class Extentions
    {
        public static Vector3 SetFloat(this Vector3 vector3, float value)
        {
            return new Vector3(value, value, value);
        }
        
        public static Vector3 OwnFloat(float value)
        {
            return new Vector3(value, value, value);
        }

        public static string ColoredText(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }
        
        public static string ColoredObj(this object obj, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{obj.ToString()}</color>";
        }
    }
}