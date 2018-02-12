using UnityEngine;

namespace NonaEngine {
    public class Block : MonoBehaviour {
        [SerializeField] private Position position;
        [SerializeField] private BlockType blockType;
        [SerializeField] private bool isBase;

        private void Start() {
            string objName = this.gameObject.name;
            objName = objName.Replace("Sand (", "");
            objName = objName.Replace(")", "");
            int coreNumber = int.Parse(objName);
            position.height = coreNumber % MapProperty.maxHeight;
            position.width = (coreNumber - position.height) / MapProperty.maxHeight;
        }

        public Position GetPosition() {
            return position;
        }

        public BlockType GetBlockType() {
            return blockType;
        }

        public void SetBlockType(BlockType type) {
            blockType = type;
        }

        public bool IsBase() {
            return isBase;
        }

    }

    [System.Serializable]
    public class Position {
        public int width;
        public int height;
    }
}