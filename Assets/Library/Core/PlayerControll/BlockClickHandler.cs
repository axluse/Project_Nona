using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NonaEngine {
    public class BlockClickHandler : MonoBehaviour {

        /// <summary>
        /// オブジェクトを取得して適正な処理をコールするインフラメソッド
        /// </summary>
        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                GameObject obj = ClickObj();
                if(obj != null) {
                    if (obj.tag == "Field") {
                        if (TurnHandler.turn == 0) {
                            #region 最初のターン
                            if (TurnHandler.turnType == TurnHandler.TurnType.player1) {
                                ExecuteEvents.Execute<NonaInterface>(
                                  target: gameObject,
                                  eventData: null,
                                  functor: (reciever, eventData) => reciever.OnZero1(obj)
                                );
                            } else {
                                ExecuteEvents.Execute<NonaInterface>(
                                  target: gameObject,
                                  eventData: null,
                                  functor: (reciever, eventData) => reciever.OnZero2(obj)
                                );
                            }
                            #endregion
                        } else {
                            #region 通常ターン
                            if (TurnHandler.turnType == TurnHandler.TurnType.player1) {
                                ExecuteEvents.Execute<NonaInterface>(
                                  target: gameObject,
                                  eventData: null,
                                  functor: (reciever, eventData) => reciever.OnControll1(obj)
                                );
                            } else {
                                ExecuteEvents.Execute<NonaInterface>(
                                  target: gameObject,
                                  eventData: null,
                                  functor: (reciever, eventData) => reciever.OnControll2(obj)
                                );
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        /// <summary>
        /// クリックしたオブジェクト取得
        /// </summary>
        private GameObject ClickObj() {
            try {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    return hit.collider.gameObject;
                }
                return null;
            } catch(System.Exception e) {
                return null;
            }
        }
    }

    public interface NonaInterface : IEventSystemHandler {
        void OnControll1(GameObject target);
        void OnControll2(GameObject target);
        void OnZero1(GameObject target);
        void OnZero2(GameObject target);
    }
}