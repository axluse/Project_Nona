using UnityEngine;
using NonaEngine;
public class Character : MonoBehaviour {
    public int hp = 10;
    public int atk = 4;
    public int skillCost = 2;
    public Sprite charaImg;
    public string skillName = "5ダメージ与える";
    public Skill skill = Skill.Damage_5;
}
