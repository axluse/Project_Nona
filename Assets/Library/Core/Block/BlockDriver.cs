using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;

public class BlockDriver : MonoBehaviour , BlockClickHandler {

    public void OnClickBlock(GameObject clickBlock) {
        BlockType type = clickBlock.GetComponent<BlockEvent>().eventType;
        Debug.Log(type);
    }
   
}
