 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    /*  IDENTIFICAÇÃO idButton
        0 - Lampada
        1 - Atomo
        2 - Cerebro
        3 - Globo
         */
    public int idButton;
    public GameController gameController;

    void OnMouseDown(){
        if(gameController.gameState == GameState.RESPONDER){
            gameController.StartCoroutine("responder", idButton);
        }
    }
}
