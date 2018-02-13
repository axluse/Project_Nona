using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NonaEngine;

public class TurnEnd : MonoBehaviour {

	public void OnStart () {
        TurnHandler.TurnType turnType = TurnHandler.turnType;
        switch(turnType) {
            // プレイヤー１
            case TurnHandler.TurnType.player1:
                TurnHandler.turnType = TurnHandler.TurnType.player2;
                break;

            // プレイヤー２
            case TurnHandler.TurnType.player2:
                TurnHandler.turnType = TurnHandler.TurnType.player1;
                TurnHandler.turn++;
                break;
        }
	}
}
