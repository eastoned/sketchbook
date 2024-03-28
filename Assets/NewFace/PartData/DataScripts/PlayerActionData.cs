using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayerActionData : CharacterActionData
{
    public bool characterExported = false;
    //part edited to be recorded
    public PlayerActionData(ActionType actionType) : base(actionType)
    {
    }
    public PlayerActionData(ActionType actionType, PartData partData) : base(actionType, partData)
    {
    }
}

[Serializable]
public class CharacterActionData
{
    public PartData partEdited;
    public string partName;
    public enum ActionType
    {
        TRANSFORMCHANGE,
        PROPERTYCHANGE,
        BREAKCHANGE,
        BUTTONCHANGE,
        NOTHINGCHANGE
    }
    
    public ActionType actionType;
    public float timeToChange;
    public Vector2 positionChange, scaleChange;
    public float angleChange;
    public float propertyChange;
    public CharacterActionData(ActionType actionType){
        this.actionType = actionType;
    }

    public CharacterActionData(ActionType actionType, PartData partData){
        this.actionType = actionType;
        this.partEdited = partData;
        this.partName = partData.partName;
    }
}