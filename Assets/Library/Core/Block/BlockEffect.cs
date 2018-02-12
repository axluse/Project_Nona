using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonaEngine {
    public static class BlockEffect {
        public static Queue<GameObject> colorChangedQueue = new Queue<GameObject>();
        public static Color defaultSelectableColor = new Color(29, 224, 74);

        public static void ChangeColor (GameObject target, Color color) {
            target.GetComponent<MeshRenderer>().material.color = color;
            colorChangedQueue.Enqueue(target);
        }

        public static void Focus(GameObject target ) {
            ChangeColor(target, defaultSelectableColor);
        }

        public static void ResetColor() {
            foreach(GameObject go in colorChangedQueue) {
                ChangeColor(go, Color.white);
            }
        }

    }
}