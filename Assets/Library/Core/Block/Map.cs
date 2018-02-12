using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;

public class Map : MonoBehaviour {
    public List<Block> blocks = new List<Block>();
    public GameObject mapDataParent;

    public List<GameObject> player1Base = new List<GameObject>();
    public List<GameObject> player2Base = new List<GameObject>();

    public int maxWidth = 8;
    public int maxHeight = 7;

    private void Awake() {
        MapProperty.maxHeight = maxHeight;
        MapProperty.maxWidth = maxWidth;
    }

    private void Start() {
        CreateData();
    }

    /// <summary>
    /// マップデータプール自動生成
    /// </summary>
    public void CreateData() {
        int iteration = 0;
        Transform cash = mapDataParent.transform.GetChild(0);

        while (cash = mapDataParent.transform.GetChild(iteration)) {
            try {
                Block b = cash.GetComponent<Block>();
                blocks.Add(b);
                ++iteration;
            } catch (System.Exception e) {
                Debug.LogWarning(e);
            }
        }
    }

    #region Blockメソッド取得
    public Block GetBlock(GameObject target) {
        return target.GetComponent<Block>();
    }

    public Block GetBlockByTransform(Transform target) {
        return GetBlock(target.gameObject);
    }

    #endregion

    /// <summary>
    /// 指定フィールドオブジェクトデータのブロック情報を参照してポジションを取得します。
    /// </summary>
    public Position GetTilePosition (GameObject target) {
        Block b = GetBlock(target);
        return b.GetPosition();
    }

    /// <summary>
    ///  対象プレイヤーの現在位置を基準とした1マス分のベクトルを指定して対象地点の情報を返却します。
    ///  
    /// ●ベクトル指定はテンキーに準拠
    /// 1|2|3
    /// 4|5|6
    /// 7|8|9
    /// 
    /// </summary>
    public Block GetDecision(int playerNumber, int vector) {
        GameObject playerBlock = GetPlayerOnBlock(playerNumber);
        if (playerBlock) {
            Block b = GetBlock(playerBlock);
            Position pos = b.GetPosition();
            Position findPos = pos;
            switch (vector) {
                #region 1
                case 1:
                    findPos.height++;
                    findPos.width++;
                    if(GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if(ret != null) {
                            return ret;
                        }
                    }
                    
                break;
                #endregion
                #region 2
                case 2:
                    findPos.height++;
                    if (GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if (ret != null) {
                            return ret;
                        }
                    }
                    break;
                #endregion
                #region 3
                case 3:
                    findPos.height++;
                    findPos.width--;
                    if (GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if (ret != null) {
                            return ret;
                        }
                    }
                break;
                #endregion
                #region 4
                case 4:
                    findPos.width++;
                    if (GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if (ret != null) {
                            return ret;
                        }
                    }
                    break;
                #endregion
                #region 5
                case 5:
                    if (GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if (ret != null) {
                            return ret;
                        }
                    }
                    break;
                #endregion
                #region 6
                case 6:
                    findPos.width--;
                    if (GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if (ret != null) {
                            return ret;
                        }
                    }
                    break;
                #endregion
                #region 7
                case 7:
                    findPos.height--;
                    findPos.width++;
                    if (GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if (ret != null) {
                            return ret;
                        }
                    }
                    break;
                #endregion
                #region 8
                case 8:
                    findPos.height--;
                    if (GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if (ret != null) {
                            return ret;
                        }
                    }
                    break;
                #endregion
                #region 9
                case 9:
                    findPos.height--;
                    findPos.width--;
                    if (GetIsField(findPos)) {
                        Block ret = GetPosToBlock(findPos);
                        if (ret != null) {
                            return ret;
                        }
                    }
                    break;
                #endregion
            }
            return null;
        } else {
            return null;
        }
    }

    /// <summary>
    /// 指定位置にフィールドが存在するかを確認する
    /// </summary>
    public bool GetIsField(Position pos) {
        if(pos.height >= maxHeight) {
            return false;
        }
        if(pos.height <= 0) {
            return false;
        }
        if(pos.width >= maxWidth) {
            return false;
        }
        if(pos.width <= 0) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 指定位置のBlock情報を取得します
    /// </summary>
    public Block GetPosToBlock(Position target) {
        foreach(Block b in blocks) {
            Position findPos = b.GetPosition();
            if (findPos.height == target.height &&
                findPos.width == target.width) {
                return b;
            }
        }
        return null;
    }

    /// <summary>
    /// 指定プレイヤー番号の現在地点ブロック情報を返却します.
    /// </summary>
    /// <param name="playerNumber">プレイヤー番号</param>
    public GameObject GetPlayerOnBlock(int playerNumber) {
        foreach(Block b in blocks) {
            if(playerNumber == 1) {
                if (b.GetBlockType() == BlockType.OnPlayer1) {
                    return b.gameObject;
                }
            } else {
                if (b.GetBlockType() == BlockType.OnPlayer2) {
                    return b.gameObject;
                }
            }
        }
        return null;
    }

}

public static class MapProperty {
    public static int maxWidth = 8;
    public static int maxHeight = 7;
}