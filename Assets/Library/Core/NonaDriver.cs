using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NonaEngine;
using DG.Tweening;

public class NonaDriver : MonoBehaviour, NonaInterface {

    #region 変数
    [SerializeField] private Map mapManager;
    [SerializeField] private GameObject phaseSelecter;
    [SerializeField] private Button summonWallBtn;
    [SerializeField] private TweenFadeAlphaAndScale left;
    [SerializeField] private TweenFadeAlphaAndScale right;
    [SerializeField] private GameObject wallObj;

    [SerializeField] private Text costText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text atkText;

    private int p1Cost = 0;
    private int p2Cost = 0;

    private int p1Hp = 10;
    private int p2Hp = 10;

    private int p1Atk = 5;
    private int p2Atk = 5;

    public ControllPhase phase = ControllPhase.Stop;

    private TurnEnd turnEndMGR = new TurnEnd();

    #endregion

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
        ControllModule(target);
    }

    /// <summary>
    /// Player2の通常フェーズ時のクリックされたオブジェクト
    /// </summary>
    public void OnControll2(GameObject target) {
        ControllModule(target);
    }

    /// <summary>
    /// 共通コントローラー
    /// </summary>
    public void ControllModule(GameObject target) {

        // プレイヤー情報取得
        int playerNumber = 1;
        if(TurnHandler.turnType != TurnHandler.TurnType.player1) {
            playerNumber = 2;
        }

        // 移動受付時
        if (phase == ControllPhase.Move) {
            // ポジション取得
            Position targetPos = target.GetComponent<Block>().GetPosition();
            Position playerPos = mapManager.GetPlayerOnBlock(playerNumber).GetComponent<Block>().GetPosition();

            // 差分検査
            if (DiffPosition(targetPos, playerPos) == 1) {

                // フィールドタイル以外の移動を禁止
                if (target.GetComponent<Block>().GetBlockType() == BlockType.Field) {
                    ControllStop(); // プレイヤーコントロールをストップ
                    AddCost(playerNumber);  // 対象プレイヤーのコストを１追加

                    #region 対象ブロックの色を変更する
                    List<Block> cashes = mapManager.GetDecision(playerNumber);
                    foreach(Block b in cashes) {
                        if(b.GetBlockType() == BlockType.Field) {
                            BlockEffect.Focus(b.gameObject);
                        } else if (b.GetBlockType() == BlockType.OnPlayer1 ||
                                      b.GetBlockType() == BlockType.OnPlayer2 ){
                            BlockEffect.ChangeColor(b.gameObject, Color.magenta);
                        }
                    }
                    #endregion

                    if(playerNumber == 1) {
                        Move(GamePropertys.p1, target);
                    } else {
                        Move(GamePropertys.p2, target);
                    }
                    
                    turnEndMGR.OnStart();

                // 指定ポジションに敵プレイヤーが存在する場合は攻撃モードに移行
                } else if (target.GetComponent<Block>().GetBlockType() == BlockType.OnPlayer2) {
                    ControllStop();
                    Attack();
                }

            }
        // 壁生成
        } else if (phase == ControllPhase.Wall) {
            if (UseCost(playerNumber, 3)) {
                SummonWall(target);
            }
        }
    }

    /// <summary>
    /// コスト追加
    /// </summary>
    public void AddCost(int targetPlayer) {
        if(targetPlayer == 1) {
            p1Cost++;
        } else {
            p2Cost++;
        }
    }
    
    /// <summary>
    /// コスト使用
    /// </summary>
    public bool UseCost(int targetPlayer, int qty) {
        if(targetPlayer == 1) {
            if(p1Cost >= qty) {
                p1Cost -= qty;
                return true;
            } else {
                return false;
            }
        } else {
            if (p2Cost >= qty) {
                p2Cost -= qty;
                return true;
            } else {
                return false;
            }
        }
    }

    /// <summary>
    /// 2ブロック間の移動歩数を測定
    /// </summary>
    /// <param name="a">地点A</param>
    /// <param name="b">地点B</param>
    /// <returns>ブロック差分</returns>
    public int DiffPosition(Position a, Position b) {
        return Mathf.Abs((a.height - b.height) - (a.width - b.width));
    }

    /// <summary>
    /// コントローラーからの入力を受付
    /// </summary>
    public void OnController(int mode) {
        phaseSelecter.SetActive(false);
        switch (mode) {
            case 1:
                phase = ControllPhase.Move;
                break;
            case 2:
                phase = ControllPhase.Wall;
                break;
        }
    }

    /// <summary>
    /// ターン開始処理
    /// </summary>
    public void OnTurnStart() {
        if (TurnHandler.turnType == TurnHandler.TurnType.player1) {
            left.StartTween();
            costText.text = p1Cost.ToString();
            // コストが足りない場合は壁生成を使えないようにする
            if(p1Cost < 3) {
                summonWallBtn.interactable = false;
            } else {
                summonWallBtn.interactable = true;
            }

        } else if (TurnHandler.turnType == TurnHandler.TurnType.player2) {
            right.StartTween();
            costText.text = p2Cost.ToString();
            // コストが足りない場合は壁生成を使えないようにする
            if (p2Cost < 3) {
                summonWallBtn.interactable = false;
            } else {
                summonWallBtn.interactable = true;
            }
        }
        StartCoroutine(IETurnStart());
    }

    private IEnumerator IETurnStart() {
        yield return new WaitForSeconds(2.5f);
        phaseSelecter.SetActive(true);
    }

    /// <summary>
    /// ブロック操作をストップ
    /// </summary>
    public void ControllStop() {
        phase = ControllPhase.Stop;
    }

    #region 移動
    public void Move(GameObject player, GameObject target) {
        // Player2 Move
        if (TurnHandler.turnType == TurnHandler.TurnType.player2) {
            mapManager.GetPlayerOnBlock(2).GetComponent<Block>().SetBlockType(BlockType.Field);
            target.GetComponent<Block>().SetBlockType(BlockType.OnPlayer2);
        
        // Player1 Move
        } else {
            mapManager.GetPlayerOnBlock(1).GetComponent<Block>().SetBlockType(BlockType.Field);
            target.GetComponent<Block>().SetBlockType(BlockType.OnPlayer1);
        }

        StartCoroutine(IEMove(player, target));
    }

    private IEnumerator IEMove(GameObject player, GameObject target) {
        // プレイヤーのアニメーション実行
        player.GetComponent<Animator>().SetInteger("animation", 15);

        // プレイヤーの移動
        player.transform.DOMove(new Vector3(
            target.transform.position.x,
            target.transform.position.y + 0.5f,
            target.transform.position.z
        ), 1.0f);

        yield return new WaitForSeconds(1.1f);
        player.GetComponent<Animator>().SetInteger("animation", 1);

        yield return new WaitForSeconds(0.5f);
        OnTurnStart();
    }
    #endregion

    #region 壁生成

    public void SummonWall(GameObject target) {
        Block block = mapManager.GetBlock(target);
        // 既に移動不可ゾーンかプレイヤーが存在する場合は生成不可
        if(block.GetBlockType() != BlockType.Wall && block.GetBlockType() != BlockType.OnPlayer1 && block.GetBlockType() != BlockType.OnPlayer2 && block.GetBlockType() != BlockType.Water) {
            ControllStop();
            StartCoroutine(IESummonWall(target));
            
        } 
    }

    private IEnumerator IESummonWall(GameObject target) {
        // 指定ポイントのブロックタイプを壁に変更
        target.GetComponent<Block>().SetBlockType(BlockType.Wall);

        // 壁を生成(地中)
        GameObject cash = Instantiate(wallObj, Vector3.zero, target.transform.rotation);
        cash.transform.parent = target.transform;
        cash.transform.localPosition = Vector3.zero;

        // 地中から地上に盛り上げる
        cash.transform.DOLocalMoveY(1.0f, 1.0f);

        yield return new WaitForSeconds(1.0f);
        turnEndMGR.OnStart();
        yield return new WaitForSeconds(0.3f);

        // ターンエンド
        OnTurnStart();
    }

    #endregion

    #region 攻撃
    public void Attack() {
        if(TurnHandler.turnType == TurnHandler.TurnType.player1) {
            StartCoroutine(IEAttack(GamePropertys.p1));
        } else {
            StartCoroutine(IEAttack(GamePropertys.p2));
        }
    }

    private IEnumerator IEAttack(GameObject player) {
        yield return null;
        player.GetComponent<Animator>().SetInteger("animation", 13);
        yield return new WaitForSeconds(1.1f);
        player.GetComponent<Animator>().SetInteger("animation", 1);
        yield return new WaitForSeconds(0.4f);
        turnEndMGR.OnStart();
        OnTurnStart();
    }
    #endregion
}
