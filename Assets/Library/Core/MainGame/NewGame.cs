using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NonaEngine;
using DG.Tweening;

public class NewGame : MonoBehaviour {

    public Image FadePanel;
    public Map map;
    public AudioSource bgmManager;
    public GameObject csn;
    public GameObject csn_head_left;
    public GameObject csn_head_right;
    public GameObject p1_1;
    public GameObject p1_2;
    public GameObject p1_3;
    public GameObject p2_1;
    public GameObject p2_2;
    public GameObject p2_3;

    private void Start () {
        TurnHandler.turnType = TurnHandler.TurnType.player1;
        FadePanel.gameObject.SetActive(true);
        if(p1_1 != null) {
            InsertGameData();
        }
        ViewCharaChange();
    }

    public void ViewCharaChange() {
        csn.SetActive(true);
        if(TurnHandler.turnType == TurnHandler.TurnType.player1) {
            csn_head_left.SetActive(true);
            csn_head_right.SetActive(false);
        } else {
            csn_head_left.SetActive(false);
            csn_head_right.SetActive(true);
        }

    }

    public void OnCharacterSelect(int charaNumber) {
        if(TurnHandler.turnType == TurnHandler.TurnType.player1) {
            GamePropertys.p1_useCh = charaNumber;
            switch(charaNumber) {
                case 1:
                    GamePropertys.p1 = p1_1;
                    break;
                case 2:
                    GamePropertys.p1 = p1_2;
                    break;
                case 3:
                    GamePropertys.p1 = p1_3;
                    break;
            }
            ViewCharaChange();
        } else {
            GamePropertys.p2_useCh = charaNumber;
            switch (charaNumber) {
                case 1:
                    GamePropertys.p2 = p2_1;
                    break;
                case 2:
                    GamePropertys.p2 = p2_2;
                    break;
                case 3:
                    GamePropertys.p2 = p2_3;
                    break;
            }
            StartCoroutine(IEStart());
        }
    }

    private IEnumerator IEStart() {
        yield return new WaitForSeconds(1f);

        // player1を自動ランダム配置
        List<GameObject> p1Data = NewGameSpawn(map.player1Base, GamePropertys.p1);
        GamePropertys.p1 = p1Data[0];
        GamePropertys.p1_position = p1Data[1].GetComponent<Block>().GetPosition();
        p1Data[1].GetComponent<Block>().SetBlockType(BlockType.OnPlayer1);

        // player2を自動ランダム配置
        List<GameObject> p2Data = NewGameSpawn(map.player2Base, GamePropertys.p2);
        GamePropertys.p2 = p2Data[0];
        GamePropertys.p2_position = p2Data[1].GetComponent<Block>().GetPosition();
        GamePropertys.p2.transform.Rotate(0, 180, 0);
        p2Data[1].GetComponent<Block>().SetBlockType(BlockType.OnPlayer2);

        yield return null;
        // ターン追加
        TurnHandler.turn++;

        yield return null;

        DOTween.ToAlpha(
            () => FadePanel.color,
            color => FadePanel.color = color,
            0.0f,
            1.0f
        );

        yield return new WaitForSeconds(1.1f);

        // 曲再生
        bgmManager.Play();

        // コントロールパネル表示
        this.GetComponent<NonaDriver>().OnTurnStart();

        FadePanel.gameObject.SetActive(false);
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
