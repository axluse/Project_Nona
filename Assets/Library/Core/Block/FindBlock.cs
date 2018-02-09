using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindBlock  {

    public static MapData mapData { get; set; }

    public static void GenerateMap(Transform childTransform) {
        int i = 0;
        MapData mapData = new MapData();

        foreach (Transform child in childTransform.transform) {
            if(i < mapData.mapY) {

            }
            ++i;
        }
    }
}

[Serializable] public class MapData {
    public int mapX = 8;
    public int mapY = 7;
    public List<List<GameObject>> data = new List<List<GameObject>>();
}