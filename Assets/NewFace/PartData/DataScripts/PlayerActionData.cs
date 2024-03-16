using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayerActionData : CharacterActionData
{
    //part edited to be recorded
    public PlayerActionData(PartData partData, ActionType actionType) : base(partData, actionType){
    }
}

[Serializable]
public class CharacterActionData
{
    public PartData partEdited;
    public enum ActionType
    {
        TRANSFORMCHANGE,
        PROPERTYCHANGE
    }
    
    public ActionType actionType;
    public float timeToChange;
    public bool brokePart;
    public CharacterActionData(PartData partData, ActionType actionType){
        this.partEdited = partData;
        this.actionType = actionType;
    }
}