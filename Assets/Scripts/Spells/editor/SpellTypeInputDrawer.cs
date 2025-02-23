using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Spells.editor
{
    [CustomPropertyDrawer(typeof(SpellTypeInput), true)]
    public class SpellTypeInputDrawer : PropertyDrawer
    {
        private static Dictionary<string, Type> _spellTypeMap;
        private string[] _spellTypeNames;
        private int _selectedTypeIndex = 0;
        
        public SpellTypeInputDrawer()
        {
            _spellTypeMap ??= AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(SpellTypeInput)) && !t.IsAbstract)
                .ToDictionary(t => t.Name, t => t);
            _spellTypeNames = _spellTypeMap.Keys.ToArray();
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            _spellTypeNames ??= _spellTypeMap.Keys.ToArray();
            if (property.managedReferenceValue == null)
            {
                _selectedTypeIndex = 0;
            }
            else
            {
                string typeName = property.managedReferenceValue.GetType().Name;
                _selectedTypeIndex = Array.IndexOf(_spellTypeNames, typeName);
            }

            Rect popupRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            int newIndex = EditorGUI.Popup(popupRect, "Input Type", _selectedTypeIndex, _spellTypeNames);

            if (newIndex != _selectedTypeIndex)
            {
                property.managedReferenceValue = Activator.CreateInstance(_spellTypeMap[_spellTypeNames[newIndex]]);
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.PropertyField(
                new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, position.height),
                property, GUIContent.none, true);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true) + EditorGUIUtility.singleLineHeight + 2;
        }
    }
}