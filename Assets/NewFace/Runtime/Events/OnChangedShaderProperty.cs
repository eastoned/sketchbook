using UnityEngine;
using UnityEngine.Events;

public class OnChangedShaderProperty : UnityEvent<float>
{
    public static OnChangedShaderProperty Instance = new OnChangedShaderProperty();
}

public class OnSlideShaderProperty : UnityEvent<float>
{
    public static OnSlideShaderProperty Instance = new OnSlideShaderProperty();
}

public class SetCurrentShaderInterval : UnityEvent<float>
{
    public static SetCurrentShaderInterval Instance = new SetCurrentShaderInterval();
}
