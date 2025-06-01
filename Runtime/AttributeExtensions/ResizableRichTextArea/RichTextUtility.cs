using UnityEngine;

public static class RichTextUtility
{
    public static string WithColor(this string value, string color)
    {
        return $"<color={color}>{value}</color>";
    }
    
    public static string WithColor(this string value, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        return value.WithColor($"#{hexColor}");
    }

    public static string InBold(this string value)
    {
        return $"<b>{value}</b>";
    }

    public static string InItalic(this string value)
    {
        return $"<i>{value}</i>";
    }

    public static string InSize(this string value, int size)
    {
        return $"<size={size}>{value}</size>";
    }
}