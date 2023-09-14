using UnityEngine;

[CreateAssetMenu(fileName = "CustomCharacter", menuName = "ScriptableObjects/CustomCharacterProfile", order = 2)]
public class CharacterData : ScriptableObject
{
    [Range(-45f, 25)] public float eyeAngle;
    [Range(0f, 1f)] public float eyeSpacing;
    [Range(-1f, .1f)] public float eyePos;
    [Range(0.5f, 1f)] public float eyeWidth, eyeHeight;

    [Range(-45f, 25f)] public float eyebrowAngle;
    [Range(0f, 1f)] public float eyebrowSpacing;
    [Range(-1f, 0.5f)] public float eyebrowHeight;
    [Range(0.2f, 1.5f)] public float eyebrowLength;
    [Range(0.1f, 0.5f)] public float eyebrowWidth;

    [Range(-0.25f, 0.5f)] public float noseHeight;
    [Range(0.5f, 2f)] public float noseLength, noseWidth;

    [Range(0.25f, 2.5f)] public float mouthWidth;
    [Range(0f, 1f)] public float mouthHeight;
    [Range(-1f, 1.25f)] public float mouthPos;
    
    [Range(0f, 1f)] public float neckWidth;

    [Range(0f, 1f)] public float headWidth, headHeight;

    [Range(.5f, 2f)] public float earWidth, earHeight;
    [Range(-0.5f,1f)] public float earPos;

    [Range(-45f, 45f)] public float earAngle;

    [Range(0f, 1f)] public float earSpacing;
    public float eyeRadius;
}
