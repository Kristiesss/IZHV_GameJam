using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character System/Profile")]
public class CharacterData : ScriptableObject
{
    public string charName;
    public int age;
    public string hobby1;
    public string hobby2;
    public string hobby3;
    public string pet;
    public string favMedia;
    public Sprite pfp;

    [Header("Compatibility (%)")]
    public int compatibilityFriend1;
    public int compatibilityFriend2;
    public int compatibilityFriend3;

    public bool isSelected = false; // checks if the character has been picked for a previous friend
}