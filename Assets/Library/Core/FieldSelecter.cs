using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NonaEngine.Input;

namespace NonaEngine {
    public class FieldSelecter : MonoBehaviour {

        [SerializeField] private GameObject focusViewObj;

        public string selectableTagName = "Field";
        public bool isSelect { get; set; }
        public Color selectableColor = Color.green;

        private Block block = new Block();

        private void Start() {
            focusViewObj.SetActive(false);
            isSelect = true;
        }

        public void Updater() {
            if (isSelect) {
                if (UnityEngine.Input.GetMouseButtonDown(0)) {
                    block.FocusReset();
                    GameObject hoverObj;
                    if (hoverObj = RaycastObject.Get(UnityEngine.Input.mousePosition)) {
                        if (hoverObj.tag == selectableTagName) {
                            block.Focus(hoverObj, selectableColor);
                            ExecuteEvents.Execute<BlockClickHandler>(
                                target: gameObject,
                                eventData: null,
                                functor: (reciever, eventData) => reciever.OnClickBlock(hoverObj)
                            );
                        }
                    }
                }

                #region デバッグ用
                if (UnityEngine.Input.GetMouseButton(0)) {
                    Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(ray, out hit)) {
                        focusViewObj.transform.position = hit.point;
                        focusViewObj.SetActive(true);
                    }
                }

                if (UnityEngine.Input.GetMouseButtonUp(0)) {
                    focusViewObj.SetActive(false);
                }
                #endregion

            }
        }
    }

    public interface BlockClickHandler : IEventSystemHandler {
        void OnClickBlock(GameObject obj);//受け取るようのメソッドを定義
    }
}