using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SceneContextMenuHandler
{
    static SceneContextMenuHandler()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
        {
            GameObject selected = Selection.activeGameObject;

            if (selected != null)
            {
                TechView tv = selected.GetComponent<TechView>();
                if (tv != null)
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Copy tech"), false, () =>
                    {
                        ExecuteCustomAction(selected);
                    });

                    menu.ShowAsContext();
                    currentEvent.Use();
                }
            }
        }
    }

    private static void ExecuteCustomAction(GameObject selected)
    {
        if (selected != null)
        {
            TechView tv = selected.GetComponent<TechView>();
            if (tv != null)
            {
                Main.Instance.CreateNewTech(tv.data);
            }
        }
    }
}