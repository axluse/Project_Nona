using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;

public class NonaDriver : MonoBehaviour, NonaInterface {

    [SerializeField] private Map mapManager;

    #region 実装されなかった部分
    /// <summary>
    /// Player1の初期フェーズ時のクリックされたオブジェクト
    /// </summary>
    public void OnZero1(GameObject target) {
        Debug.Log("OnZero1:" + target);
    }

    /// <summary>
    /// Player2の初期フェーズ時のクリックされたオブジェクト
    /// </summary>
    public void OnZero2(GameObject target) {
        Debug.Log("OnZero2:" + target);
    }
    #endregion

    /// <summary>
    /// Player1の通常フェーズ時のクリックされたオブジェクト
    /// </summary>
    public void OnControll1(GameObject target) {
        Position targetPos = target.GetComponent<Block>().GetPosition();
        Position playerPos = mapManager.GetPlayerOnBlock(1).GetComponent<Block>().GetPosition();
        //Debug.Log("OnControll1:<Target>[Height]" + targetPos.height + "[Width]" + targetPos.width);
        //Debug.Log("OnControll1:<Player>[Height]" + playerPos.height + "[Width]" + playerPos.width);
        Debug.Log("OnControll1:<Diff>" + Mathf.Abs((targetPos.height - playerPos.height) - (targetPos.width - playerPos.width)));
    }

    /// <summary>
    /// Player2の通常フェーズ時のクリックされたオブジェクト
    /// </summary>
    public void OnControll2(GameObject target) {
        Debug.Log("OnControll2:" + target);
    }
}
