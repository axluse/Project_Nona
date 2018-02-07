using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine.Input;

namespace NonaEngine {
    public class BlockEvent : MonoBehaviour {

        public BlockType eventType;
        public bool player1;
        public bool player2;

        public Vector3Int localPos { get; set; }

        // Use this for initialization
        void Start() {
            localPos = Convert.VectorInt(transform.localPosition);
        }

    }

    public enum BlockType {
        None,
        Player1Base,
        Player2Base,
        Damage,
        Wall,
        Item
    }
}