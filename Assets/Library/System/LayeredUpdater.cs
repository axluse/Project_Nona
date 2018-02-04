using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LayeredUpdater : MonoBehaviour {

    public List<bool> activeLayer = new List<bool>();
    [SerializeField] private List<UnityEvent> events = new List<UnityEvent>();

    private void Start() {
        for (int i = 0; i < (activeLayer.Count - events.Count); i++) {
            activeLayer.Add(true);
        }
    }

    void Update () {
        int i = 0;
	    foreach(UnityEvent e in events) {
            if(activeLayer[i]) {
                e.Invoke();
            }
            ++i;
        }
	}
}
