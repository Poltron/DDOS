using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour {

    public void Play()
    {
        GameManager gm = GameManager.Instance;

        if(gm)
        {
            gm.SetState(GameManager.ESTATE.SELECTION_PHASE);
        }
    }
}
