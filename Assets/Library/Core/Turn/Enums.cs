namespace NonaEngine {
    public class Enums{}

    public enum BlockType {
        Field,
        Wall,
        None,
        OnPlayer1,
        OnPlayer2,
        Water,
        Lava
    }

    public enum ControllPhase {
        Stop,
        Move,
        Attack,
        Wall,
        Skill
    }

    public enum Skill {
        Damage_5,
        Heal_5,
        PowerUP_2,
        RandomTeleport_Player,
        RandomTeleport_Enemy,
        WallBreak
    }

    public enum Ability {
        Damage,
        Heal,
        Spawn,
        Teleport,
        Move,
        Wall
    }

    public enum Target {
        None,
        Player1,
        Player2,
        Field,
        Wall,
        Base
    }

    public enum VariableType {
        None,
        Fixed,
        Random,
        RandomRange
    }
}