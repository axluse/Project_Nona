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
    [SerializeField] private GameObject characterSelecter;
    [SerializeField] private Button chara1Btn;
    [SerializeField] private Button chara2Btn;
    [SerializeField] private Button chara3Btn;
    [SerializeField] private Button summonWallBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private TweenFadeAlphaAndScale left;
    [SerializeField] private TweenFadeAlphaAndScale right;
    [SerializeField] private GameObject wallObj;

    [SerializeField] private Text costText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text atkText;

    [SerializeField] private GameObject damageObj;
    [SerializeField] private Text dmgText;

    [SerializeField] private GameObject gameOverLeft;
    [SerializeField] private GameObject gameOverRight;

    [SerializeField] private GameObject se_Damage;
    [SerializeField] private GameObject se_Heal;
    [SerializeField] private GameObject se_Teleport;

    [SerializeField] private AudioClip batan_1;
    [SerializeField] private AudioClip batan_2;
    [SerializeField] private AudioClip aa;
    [SerializeField] private AudioClip movese;
    [SerializeField] private AudioClip summonWall;

    private List<WallProperty> wallProps = new List<WallProperty>();

    private int p1Cost = 0;
    private int p2Cost = 0;

    private int p1Hp = 10;
    private int p2Hp = 10;

    private int p1Atk = 5;
    private int p2Atk = 5;

    private int p1SkillCost = 3;
    private int p2SkillCost = 3;

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

    private void Update() {
        if(phaseSelecter.activeInHierarchy) {
            if(Input.GetKeyDown(KeyCode.M)) {
                OnController(1);
            }
            if (Input.GetKeyDown(KeyCode.S) && Application.isEditor) {
                AddCost(GetPlayingNumber());
                AddCost(GetPlayingNumber());
                AddCost(GetPlayingNumber());
            }
        }
    }

    public bool IsPlayer1Turn() {
        if (TurnHandler.turnType == TurnHandler.TurnType.player1) {
            return true;
        } else {
            return false;
        }
    }

    public int GetPlayingNumber() {
        if (TurnHandler.turnType == TurnHandler.TurnType.player1) {
            return 1;
        } else {
            return 2;
        }
    }

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
                    BlockEffect.ResetColor();
                    AddCost(playerNumber);  // 対象プレイヤーのコストを１追加

                    if(playerNumber == 1) {
                        Move(GamePropertys.p1, target);
                    } else {
                        Move(GamePropertys.p2, target);
                    }
                    
                    turnEndMGR.OnStart();

                // 指定ポジションに敵プレイヤーが存在する場合は攻撃モードに移行
                } else if (target.GetComponent<Block>().GetBlockType() == BlockType.OnPlayer2 || target.GetComponent<Block>().GetBlockType() == BlockType.OnPlayer1) {
                    ControllStop();
                    BlockEffect.ResetColor();
                    AddCost(playerNumber);
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
        return (Mathf.Abs(a.height - b.height) + Mathf.Abs(a.width - b.width));
    }

    /// <summary>
    /// コントローラーからの入力を受付
    /// </summary>
    public void OnController(int mode) {
        phaseSelecter.SetActive(false);
        switch (mode) {
            case 1:
                phase = ControllPhase.Move;
                #region 対象ブロックの色を変更する
                int playerNumber = 1;
                if(TurnHandler.turnType == TurnHandler.TurnType.player2) {
                     playerNumber = 2;
                }

                List<Block> cashes = mapManager.GetDecision(playerNumber);
                foreach (Block b in cashes) {
                    if (b.GetBlockType() == BlockType.Field) {
                        BlockEffect.ChangeColor(b.gameObject, Color.green);
                    } else if (b.GetBlockType() == BlockType.OnPlayer1 ||
                                  b.GetBlockType() == BlockType.OnPlayer2) {
                        BlockEffect.ChangeColor(b.gameObject, Color.magenta);
                    }
                }
                #endregion
                break;
            case 2:
                phase = ControllPhase.Wall;
                break;
            case 3:
                Skill skill = Skill.Damage_5;
                if(TurnHandler.turnType == TurnHandler.TurnType.player1) {
                    skill = GamePropertys.p1.GetComponent<Character>().skill;
                } else {
                    skill = GamePropertys.p2.GetComponent<Character>().skill;
                }
                UseSkill(skill);
                break;
            case 4:

                Respawn();
                break;
        }
    }

    /// <summary>
    /// 指定キャラクター番号にキャラを変更
    /// </summary>
    public void OnCharacterChange(int number) {
        if(IsPlayer1Turn()) {
            GamePropertys.p1_useCh = number;
            switch (number) {
                case 1:
                    GamePropertys.p1 = GamePropertys.p1_ch1;
                    break;
                case 2:
                    GamePropertys.p1 = GamePropertys.p1_ch2;
                    break;
                case 3:
                    GamePropertys.p1 = GamePropertys.p1_ch3;
                    break;
            }
            Character cht= GamePropertys.p1.GetComponent<Character>();
            p1Hp = cht.hp;
            p1Atk = cht.atk;
            
        } else {
            GamePropertys.p2_useCh = number;
            switch (number) {
                case 1:
                    GamePropertys.p2 = GamePropertys.p2_ch1;
                    break;
                case 2:
                    GamePropertys.p2 = GamePropertys.p2_ch2;
                    break;
                case 3:
                    GamePropertys.p2 = GamePropertys.p2_ch3;
                    break;
            }
        }
    }

    /// <summary>
    /// ターン開始処理
    /// </summary>
    public void OnTurnStart() {
        // ３ターン過ぎた壁の消滅
        for(int i = 0; i < wallProps.Count; ++i) {
            wallProps[i].turn--;
            if (wallProps[i].turn <= 0) {
                StartCoroutine(IEDieWall(wallProps[i]));
                wallProps.RemoveAt(i);
            }            
        }

        if (TurnHandler.turnType == TurnHandler.TurnType.player1) {
            left.StartTween();
            costText.text = p1Cost.ToString();
            // コストが足りない場合は壁生成を使えないようにする
            if(p1Cost < 3) {
                summonWallBtn.interactable = false;
                skillBtn.interactable = false;
            } else {
                summonWallBtn.interactable = true;
                skillBtn.interactable = true;
            }
            hpText.text = p1Hp.ToString();
            atkText.text = p1Atk.ToString();

        } else if (TurnHandler.turnType == TurnHandler.TurnType.player2) {
            right.StartTween();
            costText.text = p2Cost.ToString();
            // コストが足りない場合は壁生成を使えないようにする
            if (p2Cost < 3) {
                summonWallBtn.interactable = false;
                skillBtn.interactable = false;
            } else {
                summonWallBtn.interactable = true;
                skillBtn.interactable = true;
            }
            hpText.text = p2Hp.ToString();
            atkText.text = p2Atk.ToString();
        }
        StartCoroutine(IETurnStart());
    }

    /// <summary>
    /// キャラクターを設定する
    /// </summary>
    public void SetCharacter(int playerNumber,Character chara) {
        if(playerNumber == 1) {
            p1Hp = chara.hp;
            p1Atk = chara.atk;
        } else {
            p2Hp = chara.hp;
            p2Atk = chara.atk;
        }
    }

    /// <summary>
    /// 戦闘
    /// </summary>
    public bool Battle() {
        int dmg = 0;
        bool ret = false;
        if(TurnHandler.turnType == TurnHandler.TurnType.player1) {
            dmg = p1Atk;
            
            // ATK Damageボーナス判定
            if(Random.Range(0,100) > 70) {
                dmg += Random.Range(0, 3);
            }

            p2Hp -= dmg;
            if(p2Hp <= 0) {
                p2Hp = 0;
                ret = true;
            }

            DamageUI(GamePropertys.p2, dmg);

        } else {
            dmg = p2Atk;

            // ATK Damageボーナス判定
            if (Random.Range(0, 100) > 70) {
                dmg += Random.Range(0, 3);
            }

            p1Hp -= dmg;
            if (p1Hp <= 0) {
                p1Hp = 0;
                ret = true;
            }

            DamageUI(GamePropertys.p1, dmg);
        }

        return ret;
    }

    public bool Damage(int num) {
        if (TurnHandler.turnType == TurnHandler.TurnType.player1) {
            p2Hp -= num;
            DamageUI(GamePropertys.p2, num);
            se_Damage.transform.parent = GamePropertys.p2.transform;
            se_Damage.transform.localPosition = Vector3.zero;
            se_Damage.transform.Translate(0, 0.5f, 0);
            se_Damage.GetComponent<ParticleSystem>().Stop();
            se_Damage.GetComponent<ParticleSystem>().Play();
            se_Damage.GetComponent<AudioSource>().Play();
            if (p2Hp <= 0) {
                p2Hp = 0;
                return true;
            }
            return false;
        } else {
            p1Hp -= num;
            DamageUI(GamePropertys.p1, num);
            se_Damage.transform.parent = GamePropertys.p1.transform;
            se_Damage.transform.localPosition = Vector3.zero;
            se_Damage.transform.Translate(0, 0.5f, 0);
            se_Damage.GetComponent<ParticleSystem>().Stop();
            se_Damage.GetComponent<ParticleSystem>().Play();
            se_Damage.GetComponent<AudioSource>().Play();
            if (p1Hp <= 0) {
                p1Hp = 0;
                return true;
            }
            return false;
        }
    }

    public void Heal(int num) {

    }

    public void GameOver() {

    }

    public void Teleport(GameObject target) {
        if(TurnHandler.turnType == TurnHandler.TurnType.player1) {
            mapManager.GetPlayerOnBlock(1).GetComponent<Block>().SetBlockType(BlockType.Field);
            target.GetComponent<Block>().SetBlockType(BlockType.OnPlayer1);
            GamePropertys.p1.transform.position = target.transform.position;
            GamePropertys.p1.transform.Translate(0,0.5f,0);
        } else {
            mapManager.GetPlayerOnBlock(2).GetComponent<Block>().SetBlockType(BlockType.Field);
            target.GetComponent<Block>().SetBlockType(BlockType.OnPlayer2);
            GamePropertys.p2.transform.position = target.transform.position;
            GamePropertys.p2.transform.Translate(0, 0.5f, 0);
        }
    }

    public void TurnEnd() {
        turnEndMGR.OnStart();
        OnTurnStart();
    }

    /// <summary>
    /// 出撃中キャラクターを死亡状態に切り替え
    /// </summary>
    public void Die() {
        if(IsPlayer1Turn()) {
            switch(GamePropertys.p1_useCh) {
                case 1:
                    GamePropertys.p1ch1_die = true;
                    break;
                case 2:
                    GamePropertys.p1ch2_die = true;
                    break;
                case 3:
                    GamePropertys.p1ch3_die = true;
                    break;
            }
        } else {
            switch (GamePropertys.p2_useCh) {
                case 1:
                    GamePropertys.p2ch1_die = true;
                    break;
                case 2:
                    GamePropertys.p2ch2_die = true;
                    break;
                case 3:
                    GamePropertys.p2ch3_die = true;
                    break;
            }
        }
    }

    /// <summary>
    /// リスポーン
    /// </summary>
    public void Respawn() {
        if (IsPlayer1Turn()) {
            Teleport(mapManager.player1Base[Random.Range(0, mapManager.player1Base.Count)]);
        } else {
            Teleport(mapManager.player2Base[Random.Range(0, mapManager.player2Base.Count)]);
        }
        TurnEnd();
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
        GetComponent<AudioSource>().PlayOneShot(movese);
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

        // もりあげるSE
        GetComponent<AudioSource>().PlayOneShot(summonWall);

        yield return new WaitForSeconds(1.0f);
        turnEndMGR.OnStart();
        yield return new WaitForSeconds(0.3f);

        // WallPropertyを作成
        WallProperty wp = new WallProperty();
        wp.turn = 6;
        wp.block = target.GetComponent<Block>();
        wp.wallObj = cash;

        // WallPropertyを保存
        wallProps.Add(wp);

        // ターンエンド
        OnTurnStart();
    }

    private IEnumerator IEDieWall(WallProperty wp) {
        yield return null;
        GameObject cash = wp.wallObj;
        wp.block.SetBlockType(BlockType.Field);
        cash.transform.DOLocalMoveY(0.0f, 1.0f);
        yield return new WaitForSeconds(1.1f);
        Destroy(cash);
    }

    #endregion

    #region 攻撃
    public void Attack() {
        if(TurnHandler.turnType == TurnHandler.TurnType.player1) {
            StartCoroutine(IEAttack(GamePropertys.p1, GamePropertys.p2));
        } else {
            StartCoroutine(IEAttack(GamePropertys.p2, GamePropertys.p1));
        }
    }

    private IEnumerator IEAttack(GameObject player, GameObject target) {
        if (Battle()) {
            yield return null;
            player.GetComponent<Animator>().SetInteger("animation", 13);
            target.GetComponent<Animator>().SetFloat("AnimationSpeed", 1.0f);
            target.GetComponent<Animator>().SetInteger("animation", 7);
            yield return new WaitForSeconds(0.6f);
            GetComponent<AudioSource>().PlayOneShot(aa);
            player.GetComponent<Animator>().SetInteger("animation", 1);
            yield return new WaitForSeconds(0.2f);
            GetComponent<AudioSource>().PlayOneShot(batan_2);
            yield return new WaitForSeconds(0.6f);
            target.GetComponent<Animator>().SetFloat("AnimationSpeed", 0.0f);
            turnEndMGR.OnStart();
            Die();
        } else {
            yield return null;
            player.GetComponent<Animator>().SetInteger("animation", 13);
            yield return new WaitForSeconds(0.6f);
            GetComponent<AudioSource>().PlayOneShot(aa);
            target.GetComponent<Animator>().SetInteger("animation", 5);
            yield return new WaitForSeconds(0.5f);
            player.GetComponent<Animator>().SetInteger("animation", 1);
            target.GetComponent<Animator>().SetInteger("animation", 1);
            yield return new WaitForSeconds(0.4f);
            turnEndMGR.OnStart();
            OnTurnStart();
        }
    }

    private IEnumerator IEDead(GameObject player) {
        yield return null;

    }
    #endregion

    #region スキル
    public void UseSkill(Skill skill) {
        switch(skill) {
            case Skill.Damage_5:
                StartCoroutine(IESkillDamage(15, GamePropertys.p2));
                break;
            case Skill.Heal_5:

                break;
            case Skill.RandomTeleport_Enemy:
                break;
        }
    }

    private IEnumerator IESkillDamage(int dmg, GameObject target) {
        if (Damage(dmg)) {
            yield return null;
            target.GetComponent<Animator>().SetFloat("AnimationSpeed", 1.0f);
            target.GetComponent<Animator>().SetInteger("animation", 6);
            yield return new WaitForSeconds(0.875f);
            GetComponent<AudioSource>().PlayOneShot(batan_1);
            yield return new WaitForSeconds(0.5f);
            GetComponent<AudioSource>().PlayOneShot(batan_2);
            yield return new WaitForSeconds(0.6f);
            target.GetComponent<Animator>().SetFloat("AnimationSpeed", 0.0f);
            turnEndMGR.OnStart();
            Die();
        } else {
            yield return new WaitForSeconds(2.0f);
            turnEndMGR.OnStart();
            OnTurnStart();
        }
    }
    
    
    #endregion

    #region Damage表示
    public void DamageUI(GameObject damageTarget, int damageNum) {
        Debug.Log(TurnHandler.turn + ":" + TurnHandler.turnType.ToString());

        int dmg = damageNum;

        Color dmgTextCol = Color.red;

        // マイナス値はヒールとして判定
        if (dmg < 0) {
            dmgTextCol = Color.green;
        }

        dmgTextCol.a = 0;

        // 絶対値
        dmg = Mathf.Abs(dmg);

        dmgText.color = dmgTextCol;

        damageObj.transform.parent = damageTarget.transform;
        damageObj.transform.localPosition = Vector3.zero;

        // DamageUI 表示
        dmgText.text = dmg.ToString();
        dmgText.GetComponent<TweenFadeAlphaAndScale2>().StartTween();
    }

    #endregion

}

public class WallProperty {
    public int turn = 3;
    public Block block;
    public GameObject wallObj;
}