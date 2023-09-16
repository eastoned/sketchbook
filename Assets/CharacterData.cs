using UnityEngine;

[CreateAssetMenu(fileName = "CustomCharacter", menuName = "ScriptableObjects/CustomCharacterProfile", order = 2)]
public class CharacterData : ScriptableObject
{

    #region TransformData
    
        [Range(0f, 1f)] public float headWidth, headLength;
        
        [Range(0f, 1f)] public float neckWidth;

        [Range(-45f, 25)] public float eyeAngle;
        [Range(0f, 1f)] public float eyeSpacing;
        [Range(-1f, .1f)] public float eyeHeight;
        [Range(0.5f, 1f)] public float eyeLength, eyeWidth;

        [Range(-45f, 25f)] public float eyebrowAngle;
        [Range(0f, 1f)] public float eyebrowSpacing;
        [Range(-1f, 0.5f)] public float eyebrowHeight;
        [Range(0.2f, 1.5f)] public float eyebrowLength;
        [Range(0.1f, 0.5f)] public float eyebrowWidth;

        [Range(-0.25f, 0.5f)] public float noseHeight;
        [Range(0.5f, 2f)] public float noseLength, noseWidth;

        [Range(0.25f, 2.5f)] public float mouthWidth;
        [Range(0f, 1f)] public float mouthLength;
        [Range(-1f, 1.25f)] public float mouthHeight;
        
        [Range(.5f, 2f)] public float earWidth, earLength;
        [Range(-0.5f,1f)] public float earHeight;
        [Range(-45f, 45f)] public float earAngle;
        [Range(0f, 1f)] public float earSpacing;

    #endregion

    #region ShaderData

        [Range(0.1f, 5f)] public float chinWidth, chinLength, foreheadWidth, foreheadLength;
        public Color headTop, headBottom;

        [Range(1f, 5f)] public float neckTopWidth, neckCurveScale;
        [Range(0f, 3f)] public float neckCurveRoundness;
        public Color neckTop, neckBottom;

        [Range(0f, 1f)] public float eyeRadius, pupilRadius;
        [Range(0.1f, 1f)] public float pupilWidth, pupilLength;
        [Range(0f, 2.75f)] public float eyelidTopLength, eyelidBottomLength;
        [Range(0f, 1f)] public float eyelidTopSkew, eyelidBottomSkew;
        [Range(0f, 1f)] public float eyelidTopOpen, eyelidBottomOpen;
        [Range(0.25f, 1f)] public float pupilRoundness;
        public Color eyelidCenter, eyelidEdge;

        public float eyebrowCount, eyebrowThickness, eyebrowRoundness, eyebrowCurve;
        public Color eyebrowInner, eyebrowOutter;

        //nose
        public float noseBaseWidth, noseTotalWidth, noseTopWidth, noseCurve;
        public float noseTotalLength;
        public float nostrilRadius, nostrilHeight, nostrilSpacing, nostrilScale;
        public Color noseTop, noseBottom;

        //mouth
        public float mouthRadius, mouthLipTop, mouthLipBottom;
        public float mouthLipMaskRoundness;
        public float teethTop, teethBottom;
        public float teethCount, teethRoundness;
        public float tongueRadius, tongueScale, tongueHeight;
        public Color mouthTop, mouthBottom;
        public Color tongueTop, tongueBottom;

        //ear
        public float earWidthSkew, earLengthSkew;
        public float earShape;
        public float earOpenWidth, earOpenLength;
        public float earRoundness;
        public float earConcha, earTragus;
        public Color earTop, earBottom;
        
    #endregion

    
}
