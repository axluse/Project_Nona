using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;

public class Main : MonoBehaviour, NonaHandler {

    public GameObject player1 { get; set; }
    public GameObject player1Block { get; set; }
    public GameObject player2 { get; set; }
    public GameObject player2Block { get; set; }

    [SerializeField]
    private Phase phase = Phase.Move;
    [SerializeField]
    private BlockDriver blockDriver;

    // [オブジェクトトリガー] ブロッククリック起動
    public void OnClickBlock(GameObject clickObject) {

        // ターンが0以上の時
        if (GetTurn() > 0) {
            // 先攻
            if(TurnHandler.firstBehaviour) {
                // 移動先指定時のみ入力受付
                if(phase == Phase.Move) {
                    // 指定ポイントが移動可能ポイントである場合
                    if (blockDriver.GetTileAttribute(clickObject) == BlockDriver.TileAttribute.CanMove) {
                        Move(player1,clickObject);
                    // 指定ポイント上にプレイヤーが存在する場合
                    } else if (blockDriver.GetTileAttribute(clickObject) == BlockDriver.TileAttribute.Playered) {
                        phase = Phase.Attack;
                        Attack();
                    }
                }
            // 後攻
            } else {
                // 移動先指定時のみ入力受付
                if (phase == Phase.Move) {

                }
            }
        }
    }

    // ターン移行時表示(何ターン！みたいな）
    public void TurnView() {

    }

    // あなたのターンみたいな表示の処理
    public void PlayerView() {

    }

    // 移動/アイテム/壁生成選択
    public void InfoView() {

    }

    public void MoveView() {

    }

    /// <summary>
    /// フェーズ切り替え
    /// </summary>
    public void ChangePhase(Phase togglePhase) {
        phase = togglePhase;
    }

    public void Move(GameObject targetPlayer, GameObject toBlock) {
        if(blockDriver.GetCanMovePoint(targetPlayer, toBlock)) {
            StartCoroutine(IEMove(targetPlayer, toBlock));
            ControllStop();
        }
    }

    private IEnumerator IEMove(GameObject targetPlayer, GameObject toBlock) {
        yield return null;
        targetPlayer.transform.parent = toBlock.transform;
        targetPlayer.transform.localPosition = new Vector3(0,0.5f,0);
        yield return new WaitForSeconds(1.0f);
    }

    public void Attack() {

    }

    public void UseItem() {

    }

    public void SummonWall() {

    }
    
    #region ターン制御
    public int GetTurn() {
        return TurnHandler.turn;
    }
    
    public int TurnEnd() {
        TurnHandler.TurnEnd();
        return TurnHandler.turn;
    }
    #endregion

    public void ControllStop() {
        phase = Phase.StopControll;
    }

    public enum Phase {
        Move,
        Attack,
        Item,
        SummonWall,
        StopControll
    }

}
