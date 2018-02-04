using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine.Input;

public class BlockBehaviour : MonoBehaviour {

    [SerializeField] private Vector3Int localPos;

    // Use this for initialization
    void Start() {
        localPos = Convert.VectorInt( transform.localPosition);
    }
}
