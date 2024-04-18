using UnityEngine;
using UnityEngine.Events;

public class OnChangedShaderProperty : UnityEvent<float>
{
    public static OnChangedShaderProperty Instance = new OnChangedShaderProperty();
}

public class OnChangePartShaderProperty : UnityEvent<PartController, string, float>
{
    public static OnChangePartShaderProperty Instance = new OnChangePartShaderProperty();
}

public class OnSlideShaderProperty : UnityEvent<float>
{
    public static OnSlideShaderProperty Instance = new OnSlideShaderProperty();
}

public class OnAffectFeatureWithShaderProperty : UnityEvent<ShaderProperty.AffectedFeature>
{
    public static OnAffectFeatureWithShaderProperty Instance = new OnAffectFeatureWithShaderProperty();
}

public class OnAffectSpeakAbility : UnityEvent<float>
{
    public static OnAffectSpeakAbility Instance = new OnAffectSpeakAbility();
}

public class SetCurrentShaderInterval : UnityEvent<float>
{
    public static SetCurrentShaderInterval Instance = new SetCurrentShaderInterval();
}
