using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;

[CreateAssetMenu(menuName = "SkillSetting")]
public class SkillApi : ScriptableObject {
    public string skillName;
    public List<AbillitySetting> abillitySetting = new List<AbillitySetting>();
    public GameObject Effect;
    public Target target;
    public GameObject targetTile;
}

[System.Serializable] public class AbillitySetting {
    public Ability abbility;
    public Target target;
    public VariableType varType;
    public int var1;
    public int var2;
}