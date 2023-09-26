using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomCharacter", menuName = "ScriptableObjects/CustomCharacterProfile", order = 2)]
public class CharacterData : ScriptableObject
{

    #region TransformData
    
        [Range(0f, 1f)] public float headWidth, headLength;
        
        [Range(0f, 1f)] public float neckWidth;

        [Range(-45f, 25)] public float eyeAngle;
        [Range(0f, 1f)] public float eyeSpacing;
        [Range(-0.75f, 1f)] public float eyeHeight;
        [Range(0.5f, 1f)] public float eyeLength, eyeWidth;

        [Range(-45f, 25f)] public float eyebrowAngle;
        [Range(0f, 1f)] public float eyebrowSpacing;
        [Range(-0.25f, 0.5f)] public float eyebrowHeight;
        [Range(0.2f, 1.5f)] public float eyebrowLength;
        [Range(0.1f, 0.5f)] public float eyebrowWidth;

        [Range(0f, 1f)] public float noseHeight;
        [Range(0.5f, 2f)] public float noseLength, noseWidth;

        [Range(0.25f, 2.5f)] public float mouthWidth;
        [Range(0.2f, 1f)] public float mouthLength;
        [Range(0f, 1f)] public float mouthHeight;
        
        [Range(.5f, 2f)] public float earWidth, earLength;
        [Range(-0.5f, 1f)] public float earHeight;
        [Range(-45f, 45f)] public float earAngle;
        [Range(0f, 1f)] public float earSpacing;

        [Range(0f, 1f)] public float bangWidth;
        [Range(0.25f, 1f)] public float bangLength;
        [Range(0.25f, 1f)] public float hairWidth;
        [Range(0f, 1f)] public float hairHeight;
        [Range(0.5f, 4f)] public float hairLength;

        

    #endregion

    #region ShaderData

        [Range(0.1f, 5f)] public float chinWidth, chinLength, foreheadWidth, foreheadLength;
        [Range(2f, 4f)] public float chinScale, foreheadScale;
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

        [Range(0, 16)] public float eyebrowCount;
        [Range(1f, 8f)] public float eyebrowThickness;
        [Range(0.3f, 4f)] public float eyebrowRoundness;
        [Range(-1f, 1f)] public float eyebrowCurve;
        public Color eyebrowInner, eyebrowOuter;

        //nose
        [Range(0.1f, 2.5f)]public float noseBaseWidth, noseTopWidth;
        [Range(1f, 5f)] public float noseTotalWidth, noseCurve;
        [Range(0.1f, 3f)] public float noseTotalLength;
        [Range(0f, 0.5f)] public float nostrilRadius;
        [Range(-0.5f, 0f)] public float nostrilHeight;
        [Range(0f, 1)] public float nostrilSpacing;
        [Range(0.25f, 2f)] public float nostrilScale;
        public Color noseTop, noseBottom;

        //mouth
        [Range(0.1f, 1f)] public float mouthRadius;
        [Range(-1f, 1f)] public float mouthLipTop, mouthLipBottom;
        [Range(0f, 1f)] public float mouthLipMaskRoundness;
        [Range(0f, 1f)] public float teethTop, teethBottom;
        [Range(0, 30)] public float teethCount;
        [Range(0.5f, 4f)] public float teethRoundness;
        [Range(0f, .25f)] public float tongueRadius;
        [Range(0.25f, .75f)] public float tongueScale;
        [Range(0f, 1f)] public float tongueHeight;
        public Color mouthTop, mouthBottom;
        public Color tongueTop, tongueBottom;

        //ear
        [Range(0f, 1f)] public float earWidthSkew;
        [Range(-1f, 1f)] public float earLengthSkew;
        [Range(1f, 6f)] public float earShape;
        [Range(1f, 1.5f)]public float earOpenWidth, earOpenLength;
        [Range(0.6f, 1.25f)] public float earRoundness;
        [Range(0.5f, 1.25f)] public float earConcha;
        [Range(0f, 1f)] public float earTragus;
        public Color earTop, earBottom;

        [Range(0.25f, 4f)] public float bangRoundnessFront, bangRoundnessBack;
        [Range(0, 20)] public float strandCountFront, strandCountBack;
        [Range(0, 1)] public float strandOffsetFront, strandOffsetBack;
        [Range(0.5f, 2f)] public float hairBangScaleFront, hairBangScaleBack;

        [Range(1f, 5f)] public float hairRoundnessFront, hairRoundnessBack;
        public Color hairBaseFront, hairBaseBack, hairAccentFront, hairAccentBack;
        
    #endregion

    
}
