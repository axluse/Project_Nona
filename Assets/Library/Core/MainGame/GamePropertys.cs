using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;

public static class GamePropertys  {
    public static string p1_name = "";
    public static int p1_useCh = 1;
    public static Position p1_position;
    public static GameObject p1;
    public static bool p1ch1_die;
    public static GameObject p1_ch1;
    public static bool p1ch2_die;
    public static GameObject p1_ch2;
    public static bool p1ch3_die;
    public static GameObject p1_ch3;

    public static string p2_name = "";
    public static int p2_useCh = 1;
    public static Position p2_position;
    public static GameObject p2;
    public static bool p2ch1_die;
    public static GameObject p2_ch1;
    public static bool p2ch2_die;
    public static GameObject p2_ch2;
    public static bool p2ch3_die;
    public static GameObject p2_ch3;

    public static int GetPartyCount(int playerNum) {
        int i = 0;
        if (playerNum == 1) {
            if(!p1ch1_die) {
                i++;
            }
            if (!p1ch2_die) {
                i++;
            }
            if (!p1ch3_die) {
                i++;
            }
        } else {
            if (!p2ch1_die) {
                i++;
            }
            if (!p2ch2_die) {
                i++;
            }
            if (!p2ch3_die) {
                i++;
            }
        }
        return i;
    }

}