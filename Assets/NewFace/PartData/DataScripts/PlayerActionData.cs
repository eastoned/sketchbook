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
    public PlayerActionData(PartData partData, ActionType actionType) : base(partData, actionType){
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
        NOTHINGCHANGE
    }
    
    public ActionType actionType;
    public float timeToChange;
    public bool brokePart;
    public Vector2 positionChange, scaleChange;
    public float angleChange;
    public float propertyChange;
    public CharacterActionData(PartData partData, ActionType actionType){
        this.partEdited = partData;
        this.actionType = actionType;
        this.partName = partData.partName;
    }
}