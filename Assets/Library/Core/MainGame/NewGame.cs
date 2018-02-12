using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;

public class NewGame : MonoBehaviour {

    public Map map;
    public GameObject p1_1;
    public GameObject p1_2;
    public GameObject p1_3;
    public GameObject p2_1;
    public GameObject p2_2;
    public GameObject p2_3;

    private void Start () {
        if(p1_1 != null) {
            InsertGameData();
        }
        // player1を自動ランダム配置
        List<GameObject> p1Data = NewGameSpawn(map.player1Base, p1_1);
        GamePropertys.p1 = p1Data[0];
        GamePropertys.p1_position = p1Data[1].GetComponent<Block>().GetPosition();
        p1Data[1].GetComponent<Block>().SetBlockType(BlockType.OnPlayer1);

        // player2を自動ランダム配置
        List<GameObject> p2Data = NewGameSpawn(map.player2Base, p2_1);
        GamePropertys.p2 = p2Data[0];
        GamePropertys.p2_position = p2Data[1].GetComponent<Block>().GetPosition();
        GamePropertys.p2.transform.Rotate(0, 180, 0);
        p2Data[1].GetComponent<Block>().SetBlockType(BlockType.OnPlayer2);

        // ターン追加
        TurnHandler.turn++;
    }

    public List<GameObject> NewGameSpawn(List<GameObject> playerBase, GameObject player) {
        int range = playerBase.Count;
        range = Random.Range(0, range);
        GameObject target = playerBase[range];
        GameObject obj = Instantiate(player, Vector3.zero, Quaternion.identity);
        Transform before = obj.transform;
        obj.transform.parent = target.transform;
        obj.transform.position = target.transform.position;
        obj.transform.rotation = target.transform.rotation;
        obj.transform.Translate(0, 0.5f, 0);
        obj.transform.parent = this.transform;
        List<GameObject> ret = new List<GameObject>();
        ret.Add(obj);
        ret.Add(target);
        return ret;
    }

    /// <summary>
    /// ゲームデータのインサート処理
    /// </summary>
    private void InsertGameData() {
        GamePropertys.p1_ch1 = p1_1;
        GamePropertys.p1_ch2 = p1_2;
        GamePropertys.p1_ch3 = p1_3;
        GamePropertys.p2_ch1 = p2_1;
        GamePropertys.p2_ch2 = p2_2;
        GamePropertys.p2_ch3 = p2_3;
    }
	
}
