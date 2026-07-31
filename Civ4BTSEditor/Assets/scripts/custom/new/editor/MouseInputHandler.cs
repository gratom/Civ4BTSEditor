using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.iOS;
using UnityEngine.SceneManagement;

namespace Tools
{
    [CustomEditor(typeof(SceneView))]
    [InitializeOnLoad]
    public class MouseInputHandler : Editor
    {
        private static GUIContent tooltipContent = new GUIContent();
        public static Main Main;

        private Vector3 startMousePosition;
        private Vector3 endMousePosition;

        public static Vector2 MousePos;
        public static GameObject PickedObject;
        public static TechView CurrentTech;
        public static TechView ConnectToTech;

        private static bool isSelected = false;
        private static string connection = "";

        static MouseInputHandler()
        {
            //StartListening();
        }

        private static bool isListening = false;

        [MenuItem("Custom/Switch Listening &e")]
        private static void SwitchListening()
        {
            if (isListening)
            {
                isListening = !isListening;
                StopListening();
            }
            else
            {
                isListening = !isListening;
                StartListening();
            }
        }

        [MenuItem("Custom/Start Listening")]
        private static void StartListening()
        {
            SceneView.duringSceneGui += DuringScene;
            SceneView.beforeSceneGui += BeforeScene;
            Main = SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(x => x.GetComponent<Main>() != null)?.GetComponent<Main>();
        }

        [MenuItem("Custom/Stop Listening")]
        private static void StopListening()
        {
            SceneView.duringSceneGui -= DuringScene;
            SceneView.beforeSceneGui -= BeforeScene;
        }

        private static void BeforeScene(SceneView obj)
        {
            Event currentEvent = Event.current;
            EventType eventType = currentEvent.type;
            if (eventType == EventType.MouseUp)
            {
                if (currentEvent.button == 0)
                {
                    if (CurrentTech != null && Main != null)
                    {
                        if (isSelected && ConnectToTech != null)
                        {
                            Vector2 v = GetMouseWorldPosition();
                            if (v.x > ConnectToTech.AnchoredPosition.x)
                            {
                                //and
                                CurrentTech.data.AndPreq(ConnectToTech);
                            }
                            else
                            {
                                //or
                                CurrentTech.data.OrPreq(ConnectToTech);
                            }
                        }

                        CurrentTech.CheckPos();
                        CurrentTech.InitFromData();
                        Main.GameObjects2Object();
                        Main.Object2GameObjects();

                    }
                    ConnectToTech = null;
                    CurrentTech = null;
                    isSelected = false;
                }
            }
        }

        private static void DuringScene(SceneView sceneView)
        {
            ClearSelection();

            Event currentEvent = Event.current;
            EventType eventType = currentEvent.type;

            MousePos = GetMouseWorldPosition();

            if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.Used)
            {
                PickedObject = HandleUtility.PickGameObject(currentEvent.mousePosition, true);
                if (PickedObject != null)
                {
                    if (TryGetTechObject(PickedObject, out TechView techObject))
                    {
                        if (isSelected)
                        {
                            if (techObject != CurrentTech)
                            {
                                ConnectToTech = techObject;
                            }
                        }
                        else
                        {
                            CurrentTech = techObject;
                        }
                    }
                }
            }

            if (eventType == EventType.MouseDown && currentEvent.button == 0)
            {
                isSelected = CurrentTech != null;
            }
            HandleMouseHover();
        }

        private static bool TryGetTechObject(GameObject pickedObject, out TechView techObject)
        {
            if (pickedObject.TryGetComponent<TechView>(out techObject))
            {
                return true;

            }
            if (pickedObject.transform.parent != null)
            {
                if (pickedObject.transform.parent.gameObject.TryGetComponent<TechView>(out techObject))
                {
                    return true;
                }
            }
            return false;
        }

        private static void ConnectUpdate(TechView connect)
        {
            Vector2 v = GetMouseWorldPosition();
            if (v.x > connect.AnchoredPosition.x)
            {
                connection = $"+[AND]({connect.data.Type.Substring(5)})";
            }
            else
            {
                connection = $"+[OR]({connect.data.Type.Substring(5)})";
            }
        }

        private static void ClearSelection()
        {
            List<GameObject> selectedObjects = Selection.gameObjects.ToList();

            if (selectedObjects.Count > 0)
            {
                selectedObjects.RemoveAll(x => x.GetComponent<TechView>() == null && x.GetComponent<Main>() == null);
            }
            Selection.objects = selectedObjects.ToArray();
        }

        private static Vector2 GetMouseWorldPosition()
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
            float distance;
            if (groundPlane.Raycast(mouseRay, out distance))
            {
                return mouseRay.GetPoint(distance);
            }
            return Vector2.zero;
        }

        private static void HandleMouseHover()
        {
            if (PickedObject == null)
            {
                return;
            }

            connection = "";
            if (isSelected && ConnectToTech != null)
            {
                ConnectUpdate(ConnectToTech);
            }
            if (CurrentTech != null)
            {
                Event currentEvent = Event.current;
                tooltipContent.text = "  " + CurrentTech.data.Type.Substring(5) + (isSelected ? "[selected]" : "") + connection;
                Vector2 tooltipSize = GUI.skin.label.CalcSize(tooltipContent);

                Handles.BeginGUI();
                GUI.Label(new Rect(currentEvent.mousePosition.x, currentEvent.mousePosition.y, tooltipSize.x, tooltipSize.y), tooltipContent);
                Handles.EndGUI();
            }
        }
    }
}