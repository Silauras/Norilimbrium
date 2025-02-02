using System;
using System.Collections.Generic;
using Code.Scripts.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Characters.DialogueSystem
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking;
        [SerializeField] Sprite portrait;
        [SerializeField] string title;
        [SerializeField] string text;
        [SerializeField] AudioClip audioClip;
        [SerializeField] List<string> children = new();
        [SerializeField] Rect rect = new(0, 0, 340, 180);
        [SerializeField] UnityEvent onEnterAction;
        [SerializeField] UnityEvent onExitAction;
        [SerializeField] Condition condition;

        public Rect GetRect()
        {
            return rect;
        }

        public Sprite GetPortrait()
        {
            return portrait;
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetText()
        {
            return text;
        }

        public List<string> GetChildren()
        {
            return children;
        }

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public bool CheckCondition(IEnumerable<IConditionEvaluator> evaluators)
        {
            return condition.Check(evaluators);
        }

#if UNITY_EDITOR

        public void SetTitle(string newTitle)
        {
            if (newTitle == title) return;
            Undo.RecordObject(this, "Update Dialogue Text");
            title = newTitle;
            EditorUtility.SetDirty(this);
        }

        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText == text) return;
            Undo.RecordObject(this, "Update Dialogue Text");
            text = newText;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Toggle Dialogue Speaker");
            isPlayerSpeaking = newIsPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }

#endif
    }
}