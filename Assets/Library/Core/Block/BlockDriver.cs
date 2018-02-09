using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NonaEngine;

public class BlockDriver : MonoBehaviour, NonaHandler {

    #region 変数
    [HideInInspector]public List<BlockPropertys> map = new List<BlockPropertys>();
    [System.Serializable]
    public class BlockPropertys {
        public List<GameObject> x;
    }
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject parent;

    [SerializeField] private Color canMovePointColor = Color.red;

    [SerializeField] private Main main;

    #region プレイヤーポジション
    [HideInInspector]
    public Vector2 player1Pos;
    [HideInInspector]
    public Vector2 player2Pos;
    #endregion

    private bool firstDraw = true;
    #endregion



    #region　[初期配置]コントロール
    public void OnClickBlock(GameObject clickBlock) {

        BlockType type = clickBlock.GetComponent<BlockEvent>().eventType;

        // 初期配置
        if (TurnHandler.turn == 0) {
            TurnZero(clickBlock, type);
        }
    }
    #endregion

    #region 初期配置
    /// <summary>
    /// クリック位置にプレイヤーをスポーンさせる
    /// </summary>
    public void TurnZero (GameObject target, BlockType targetBlockType) {
        Vector2Int pos = GetTilePosition(target);
        if (firstDraw) {
            if(targetBlockType == BlockType.Player1Base) {
                main.player1Block = target;
                GameObject instance = PlayerSpawn(target);
                main.player1 = instance;
                instance.transform.localRotation = Quaternion.Euler(0, 0, 0);
                instance.transform.parent = transform;
                player1Pos = pos;
                firstDraw = false;
            }
        } else {
            if (targetBlockType == BlockType.Player2Base) {
                main.player2Block = target;
                GameObject instance = PlayerSpawn(target);
                instance.transform.localRotation = Quaternion.Euler(0, 180, 0);
                instance.transform.parent = transform;
                firstDraw = true;
                player2Pos = pos;
                TurnHandler.turn++;
                TurnHandler.firstBehaviour = true;
                main.player2 = instance;

                main.PhaseController();

            }
        }
    }
    #endregion

    #region マップ情報取得
    /// <summary>
    /// 北
    /// </summary>
    public GameObject GetNorth(Vector2Int pos) {
        if(pos.y < GetMapHeight()) {
            return map[pos.x].x[pos.y+1];
        }
        return null;
    }

    /// <summary>
    /// 西
    /// </summary>
    public GameObject GetWest(Vector2Int pos) {
        if (pos.x < GetMapWidth()) {
            return map[pos.x].x[pos.y + 1];
        }
        return null;
    }

    /// <summary>
    /// 南
    /// </summary>
    public GameObject GetSouth(Vector2Int pos) {
        if (pos.y > 0) {
            return map[pos.x].x[pos.y -1];
        }
        return null;
    }

    /// <summary>
    /// 東
    /// </summary>
    public GameObject GetEast(Vector2Int pos) {
        if (pos.y > 0) {
            return map[pos.x-1].x[pos.y];
        }
        return null;
    }

    /// <summary>
    /// マップ高さ取得
    /// </summary>
    /// <returns></returns>
    public int GetMapHeight() {
        return map.Count - 1;
    }

    /// <summary>
    /// マップ幅取得
    /// </summary>
    /// <returns></returns>
    public int GetMapWidth() {
        return map[0].x.Count - 1;
    }

    /// <summary>
    /// 対象オブジェクトの移動が可能かもしくは対象オブジェクト上にプレイヤーがいるか。
    /// </summary>
    public TileAttribute GetTileAttribute(GameObject target) {
        try {
            BlockEvent be = target.GetComponent<BlockEvent>();
            if (be.player1 || be.player2) {
                return TileAttribute.Playered;
            }

            if (be.eventType == BlockType.Wall) {
                return TileAttribute.Cant;
            }
            return TileAttribute.CanMove;
        } catch (System.Exception e) {
            return TileAttribute.CanMove;
        }    
    }

    public int GetMoveOrAttack(GameObject target) {
        Block block = new Block();
        if (target != null) {
            if (GetTileAttribute(target) == TileAttribute.CanMove) {
                return 1;

                // 攻撃可能ポイント
            } else if (GetTileAttribute(target) == TileAttribute.Playered) {
                return 2;
            }
        }
        return 0;
    }

    /// <summary>
    /// タイル状態ENUM
    /// </summary>
    public enum TileAttribute {
        CanMove,
        Cant,
        Playered
    }
    #endregion

    #region プレイヤー位置参照及び位置同期
    /// <summary>
    /// プレイヤーがその場所にいることをブロック側に同期します。
    /// </summary>
    public void SetPlayerNow (GameObject target ,int playerNumber, bool flag = true) {
        BlockEvent tBE = target.GetComponent<BlockEvent>();
        if(playerNumber == 1) {
            tBE.player1 = flag;
        } else {
            tBE.player2 = flag;
        }
    }

    /// <summary>
    /// プレイヤー通知情報を削除
    /// </summary>
    public void ResetAllPlayerNow () {
        foreach (BlockPropertys bp in map) {
            foreach (GameObject o in bp.x) {
                BlockEvent tBE = o.GetComponent<BlockEvent>();
                tBE.player1 = false;
                tBE.player2 = false;
            }
        }
    }
    #endregion

    #region スポーン
    /// <summary>
    /// プレイヤースポーン
    /// </summary>
    public GameObject PlayerSpawn(GameObject target) {
        Vector3 spawnPos = target.transform.position;
        spawnPos.y += 1.0f;
        spawnPos.y = (int)spawnPos.y;

        GameObject instance = Instantiate(player, spawnPos, Quaternion.identity);
        instance.transform.parent = target.transform;
        return instance;
    }
    #endregion

    /// <summary>
    /// 移動可能ブロックを表示
    /// </summary>
    // ■表示コントロール
    public void CanMovePos(GameObject PlayerNowGameObj) {
        try {
            // 現在位置
            Vector2Int nowPlayerPos = GetTilePosition(PlayerNowGameObj);
            Block b = new Block();
            b.FocusReset();
            // 東
            MoveOrAttackView(GetEast(nowPlayerPos));
            // 西
            MoveOrAttackView(GetWest(nowPlayerPos));
            // 南
            MoveOrAttackView(GetSouth(nowPlayerPos));
            // 北
            MoveOrAttackView(GetNorth(nowPlayerPos));

        } catch (System.Exception e) {
            Debug.LogWarning(e);
        }
    }
    // ■単体表示コントロール
    public void MoveOrAttackView(GameObject target) {
        Block block = new Block();
        Debug.Log(target);
        if (target != null) {
            int cd = GetMoveOrAttack(target);
            if(cd == 1) { 
                block.Focus(target, canMovePointColor);
            } else if (cd == 0) {
                block.Focus(target, Color.magenta);
            }
        }
    }

    /// <summary>
    /// 移動元から移動先まで移動が可能であるか。
    /// </summary>
    public bool GetCanMovePoint(GameObject player, GameObject toBlock) {
        Vector2Int from = GetTilePosition(player);
        Vector2Int to = GetTilePosition(toBlock);
        Vector2Int answer = from - to;
        int point = answer.y + answer.x;
        if (point == 1 || point == -1) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 指定ポイント上にプレイヤーが存在し、なおかつ攻撃可能位置であるかを確認
    /// </summary>
    public bool GetCanAttackPoint(GameObject player, GameObject target) {
        return true;
    }

    /// <summary>
    /// 座標情報からオブジェクト情報に変換します。
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject GetPositionToBlock(Vector2Int position) {
        return map[position.y].x[position.x];
    }

    /// <summary>
    /// タイル座標を取得
    /// </summary>
    public Vector2Int GetTilePosition(GameObject target) {
        if(target == null) {
            return new Vector2Int(-1, -1);
        } else {
            int iX = 0;
            int iY = 0;
            Vector2Int ret = Vector2Int.zero;
            foreach (BlockPropertys bp in map) {
                iX = 0;
                foreach (GameObject o in bp.x) {
                    if (o == target) {
                        ret.y = iX;
                        ret.x = iY;
                        break;
                    }
                    ++iX;
                }
                ++iY;
            }
            return ret;
        }
    }

}