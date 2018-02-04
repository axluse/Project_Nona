using UnityEngine;

namespace NonaEngine.Input {
    public static class RaycastObject {

        public static GameObject Get(Vector3 targetPos) {
            GameObject result = null;
            Ray ray = Camera.main.ScreenPointToRay(targetPos);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit)) {
                result = hit.collider.gameObject;
            }

            return result;
        }
    }

    public static class Mouse {
        public static bool Clamp() {
            Vector3 mouse = UnityEngine.Input.mousePosition;
            mouse = Camera.main.ScreenToViewportPoint(mouse);
            float screenPosX = Screen.width;
            float screenPosY = Screen.height;
            if(mouse.x <= screenPosX && mouse.y <= screenPosY) {
                return true;
            }
            return false;
        }
    }

    public static class Convert {
        public static Vector3Int VectorInt(Vector3 vector) {
            Vector3Int ret = Vector3Int.zero;
            ret.x = (int)vector.x;
            ret.y = (int)vector.y;
            ret.z = (int)vector.z;
            return ret;
        }
    }
}