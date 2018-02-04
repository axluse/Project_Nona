using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonaEngine {
    public class BlockEvent : MonoBehaviour {

        public BlockType eventType;


    }

    public enum BlockType {
        None,
        Base,
        Damage,
        Wall,
        Item
    }
}