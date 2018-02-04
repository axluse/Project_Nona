using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonaEngine {
    public class Block : MonoBehaviour{

        #region Focus
        private Queue<GameObject> focusObj = new Queue<GameObject>();

        public void Focus(GameObject target, Color col) {
            target.GetComponent<MeshRenderer>().material.color = col;
            focusObj.Enqueue(target);
        }

        public void FocusReset() {
            while (focusObj.Count > 0) {
                GameObject go = focusObj.Dequeue();
                go.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

        #endregion

    }
}