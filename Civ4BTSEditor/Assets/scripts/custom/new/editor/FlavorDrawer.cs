using System;
using UnityEditor;
using UnityEngine;
public enum FlavorTypeEnum
{
    NONE,
    FLAVOR_RELIGION,
    FLAVOR_GOLD,
    FLAVOR_CULTURE,
    FLAVOR_GROWTH,
    FLAVOR_SCIENCE,
    FLAVOR_PRODUCTION,
    FLAVOR_MILITARY
}

[CustomPropertyDrawer(typeof(Flavor))]
public class FlavorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProp = property.FindPropertyRelative("FlavorType");
        SerializedProperty flavorProp = property.FindPropertyRelative("flavor");

        position.height = EditorGUIUtility.singleLineHeight;

        Rect foldoutRect = new Rect(position.x, position.y, position.width, position.height);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        if (property.isExpanded)
        {
            position.y += EditorGUIUtility.singleLineHeight + 2;

            FlavorTypeEnum currentEnum = FlavorTypeEnum.NONE;
            if (!string.IsNullOrEmpty(typeProp.stringValue))
            {
                try
                {
                    currentEnum = (FlavorTypeEnum)Enum.Parse(typeof(FlavorTypeEnum), typeProp.stringValue);
                }
                catch
                {
                    currentEnum = FlavorTypeEnum.NONE;
                }
            }

            EditorGUI.BeginChangeCheck();
            FlavorTypeEnum selectedEnum = (FlavorTypeEnum)EditorGUI.EnumPopup(
                new Rect(position.x, position.y, position.width, position.height),
                "Flavor Type",
                currentEnum
            );
            if (EditorGUI.EndChangeCheck())
            {
                typeProp.stringValue = selectedEnum.ToString();
            }

            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.BeginChangeCheck();
            int newFlavor = EditorGUI.IntSlider(
                new Rect(position.x, position.y, position.width, position.height),
                "Flavor Value",
                flavorProp.intValue,
                0,
                10
            );
            if (EditorGUI.EndChangeCheck())
            {
                flavorProp.intValue = newFlavor;
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        else
        {
            return (EditorGUIUtility.singleLineHeight + 2) * 3;
        }
    }
}