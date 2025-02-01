using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Code.Scripts.Characters.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 20);

        Dictionary<string, DialogueNode> nodeLookup = new();

        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (var node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            return
                from childId in parentNode.GetChildren()
                where nodeLookup.ContainsKey(childId)
                select nodeLookup[childId];
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            return GetAllChildren(currentNode).Where(node => node.IsPlayerSpeaking());
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            return GetAllChildren(currentNode).Where(node => !node.IsPlayerSpeaking());
        }

#if UNITY_EDITOR

        public void CreateNode(DialogueNode parentNode)
        {
            DialogueNode newNode = MakeNode(parentNode);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            AddNode(newNode);
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private DialogueNode MakeNode(DialogueNode parentNode)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();

            if (!parentNode) return newNode;

            parentNode.AddChild(newNode.name);
            newNode.SetPlayerSpeaking(!parentNode.IsPlayerSpeaking());
            newNode.SetPosition(parentNode.GetRect().position + newNodeOffset);

            return newNode;
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (var node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }

#endif


        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                var rootNode = MakeNode(null);
                AddNode(rootNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (var node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}