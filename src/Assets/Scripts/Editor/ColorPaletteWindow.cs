using UnityEngine;
using UnityEditor;

// Change the color of the selected GameObjects.

public class ColorPalette : EditorWindow
{
    Color32 green1 = new Color32(68, 255, 19, 1);
    Color32 green2 = new Color32(48, 184, 28, 1);
    Color32 green3 = new Color32(37, 129, 48, 1);
    Color32 green4 = new Color32(18, 63, 39, 1);
    Color32 green5 = new Color32(17, 30, 24, 1);
    Color32 pink1 = new Color32(255, 52, 245, 1);
    Color32 pink2 = new Color32(120, 34, 160, 1);
    Color32 pink3 = new Color32(44, 24, 99, 1);
    Color32 blue1 = new Color32(11, 245, 231, 1);
    Color32 blue2 = new Color32(21, 169, 193, 1);
    Color32 blue3 = new Color32(22, 70, 146, 1);
    Color32 blue4 = new Color32(11, 29, 57, 1);
    Color32 darkBlue = new Color32(4, 6, 11, 1);
    Color32 lightGray = new Color32(205, 205, 205, 1);
    Color32 darkGray = new Color32(127, 127, 127, 1);

    [MenuItem("Window/Color Window")]

    static void Init()
    {
        EditorWindow window = GetWindow(typeof(ColorPalette));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Color Modifiers", EditorStyles.boldLabel);
        GUILayout.Space(15);
        green1 = EditorGUILayout.ColorField("44FF13", green1);
        green2 = EditorGUILayout.ColorField("30B81C", green2);
        green3 = EditorGUILayout.ColorField("258130", green3);
        green4 = EditorGUILayout.ColorField("123F27", green4);
        green5 = EditorGUILayout.ColorField("111E18", green5);
        GUILayout.Space(15);
        pink1 = EditorGUILayout.ColorField("FF34F5", pink1);
        pink2 = EditorGUILayout.ColorField("7822A0", pink2); 
        pink3 = EditorGUILayout.ColorField("2C1863", pink3);
        GUILayout.Space(15);
        blue1 = EditorGUILayout.ColorField("0BF5E7", blue1);
        blue2 = EditorGUILayout.ColorField("15A9C1", blue2);
        blue3 = EditorGUILayout.ColorField("164692", blue3);
        blue4 = EditorGUILayout.ColorField("0B1D39", blue4);
        GUILayout.Space(15);
        darkBlue = EditorGUILayout.ColorField("04060B", darkBlue); 
        lightGray = EditorGUILayout.ColorField("CDCDCD", lightGray);
        darkGray = EditorGUILayout.ColorField("7F7F7F", darkGray);
        GUILayout.Space(15);


        if (GUILayout.Button("Reset"))
            ResetColors();
    }

    private void ResetColors()
    {
        green1 = new Color32(68, 255, 19, 1);
        green2 = new Color32(48, 184, 28, 1);
        green3 = new Color32(37, 129, 48, 1);
        green4 = new Color32(18, 63, 39, 1);
        green5 = new Color32(17, 30, 24, 1);
        pink1 = new Color32(255, 52, 245, 1);
        pink2 = new Color32(120, 34, 160, 1);
        pink3 = new Color32(44, 24, 99, 1);
        blue1 = new Color32(11, 245, 231, 1);
        blue2 = new Color32(21, 169, 193, 1);
        blue3 = new Color32(22, 70, 146, 1);
        blue4 = new Color32(11, 29, 57, 1);
        darkBlue = new Color32(4, 6, 11, 1);
        lightGray = new Color32(205, 205, 205, 1);
        darkGray = new Color32(127, 127, 127, 1);

    }
}