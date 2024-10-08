using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Code.Scripts.Characters.DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue;
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] GUIStyle playerNodeStyle;
        [NonSerialized] DialogueNode draggingNode;
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode;
        [NonSerialized] DialogueNode deletingNode;
        [NonSerialized] DialogueNode linkingParentNode;
        Vector2 scrollPosition;
        [NonSerialized] bool draggingCanvas;
        [NonSerialized] Vector2 draggingCanvasOffset;

        const float canvasSize = 4000;
        const float backGroundSize = 50;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            nodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node0") as Texture2D,
                    textColor = Color.white
                },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };

            playerNodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node1") as Texture2D,
                    textColor = Color.white
                },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue)
            {
                ProcessEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect textureCoords = new Rect(0, 0, canvasSize / backGroundSize, canvas.height / backGroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoords);

                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }

                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }

                if (deletingNode)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
            else
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
        }


        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && !draggingNode)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingOffset - Event.current.mousePosition;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
            
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (var node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    foundNode = node;
                }
            }

            return foundNode;
        }

        private void DrawNode(DialogueNode node)
        {
            var style = node.IsPlayerSpeaking() ? playerNodeStyle : nodeStyle;

            GUILayout.BeginArea(node.GetRect(), style);
            EditorGUI.BeginChangeCheck();
            
            node.SetText(EditorGUILayout.TextField(node.GetText()));
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("-"))
            {
                deletingNode = node;
            }

            DrawLinkButtons(node);
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }
            
            GUILayout.EndHorizontal();
            
            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (!linkingParentNode)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    Undo.RecordObject(selectedDialogue, "Add Dialogue Link");
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;

                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosiltion = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosiltion;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(
                    startPosiltion, endPosition,
                    startPosiltion + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white, null, 4f);
            }
        }
    }
}