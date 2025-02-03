using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Speaker", menuName = "Create new Dialogue Speaker")]
public class DialogueSpeaker : ScriptableObject
{
    public string Name;
    public Color NameColor;
    public Sprite Portrait;
}
