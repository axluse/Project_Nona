using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Main : MonoBehaviour, NonaHandler {
    [HideInInspector] public GameObject player1 { get; set; }
    public GameObject player1Block;
    [HideInInspector] public GameObject player2 { get; set; }
    public GameObject player2Block;

    [SerializeField] private Text turnText;
    [SerializeField] private Text firstTurnText;
    [SerializeField] private GameObject phaseSelecter;
    [SerializeField] private Text phase_koma;
    [SerializeField] private Text phase_hp;
    [SerializeField] private Text phase_atk;
    [SerializeField] private TweenFadeAlphaAndScale leftTurn;
    [SerializeField] private TweenFadeAlphaAndScale rightTurn;

    [SerializeField]
    private Phase phase = Phase.Move;
    [SerializeField]
    private BlockDriver blockDriver;

    private void Start() {
        phaseSelecter.SetActive(false);
    }

    // フェーズコントローラー
    public void PhaseController() {
        phase = Phase.StopControll;
        StartCoroutine(IETurnRunner());

    }

    private IEnumerator IETurnRunner() {
        yield return null;
        TurnView();
        yield return new WaitForSeconds(0.25f);
        PlayerView();
        yield return new WaitForSeconds(2f);
        InfoView();

    }

    //// [オブジェクトトリガー] ブロッククリック起動
    //public void OnClickBlock(GameObject clickObject) {

    //    // ターンが0以上の時
    //    if (GetTurn() > 0) {
    //        // 先攻
    //        if (TurnHandler.firstBehaviour) {
    //            // 移動先指定時のみ入力受付
    //            if (phase == Phase.Move) {
    //                int moveCD = blockDriver.GetMoveOrAttack(clickObject);
    //                if (moveCD == 1) {
    //                    Move(player1, player1Block, clickObject);
    //                    player1Block = clickObject;
                        
    //                } else if (moveCD == 2) {
    //                    Attack();
    //                }
    //            }
    //            // 後攻
    //        } else {
    //            // 移動先指定時のみ入力受付
    //            if (phase == Phase.Move) {
    //                int moveCD = blockDriver.GetMoveOrAttack(clickObject);
    //                if (moveCD == 1) {
    //                    Move(player2, player2Block, clickObject);
    //                    player2Block = clickObject;
    //                } else if (moveCD == 2) {
    //                    Attack();
    //                }
    //            }
    //        }
    //    }
    //}

    // ターン移行時表示(何ターン！みたいな）
    public void TurnView() {
        turnText.text = TurnHandler.turn.ToString();
    }

    // あなたのターンみたいな表示の処理
    public void PlayerView() {
        if (TurnHandler.firstBehaviour) {
            leftTurn.StartTween();
        } else {
            rightTurn.StartTween();
        }
    }

    // 選択画面
    public void InfoView() {
        phaseSelecter.SetActive(true);
    }

    public void OnMoving() {
        phaseSelecter.SetActive(false);
        GameObject block = player1Block;
        if (TurnHandler.firstBehaviour == false) {
            block = player2Block;
        }
        blockDriver.CanMovePos(block);
        ChangePhase(Phase.Move, 0.3f);
    }

    /// <summary>
    /// フェーズ切り替え
    /// </summary>
    public void ChangePhase(Phase togglePhase, float duration= 0.0f) {
        StartCoroutine(IECP(togglePhase, duration));
    }

    private IEnumerator IECP(Phase togglePhase, float duration = 0.0f) {
        yield return new WaitForSeconds(duration);
        phase = togglePhase;
    }

    public void Move(GameObject targetPlayer, GameObject targetPlayerBlock, GameObject toBlock) {
        Block block = new Block();
        block.FocusReset();
        if(blockDriver.GetCanMovePoint(targetPlayerBlock, toBlock)) {
            ControllStop();
            StartCoroutine(IEMove(targetPlayer, toBlock));
        }
    }

    private IEnumerator IEMove(GameObject targetPlayer, GameObject toBlock) {
        Debug.Log("1");
        yield return null;
        targetPlayer.transform.parent = toBlock.transform;
        targetPlayer.GetComponent<Animator>().SetInteger("animation", 15);
        targetPlayer.transform.DOLocalMove(new Vector3(0, 0.5f, 0), 1f);
        yield return new WaitForSeconds(1.0f);
        targetPlayer.GetComponent<Animator>().SetInteger("animation", 1);
        yield return new WaitForSeconds(0.5f);
        TurnEnd();
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
        PhaseController();
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
