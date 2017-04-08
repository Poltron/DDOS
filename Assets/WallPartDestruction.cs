using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPartDestruction : MonoBehaviour {

    Timer t = new Timer();

	// Update is called once per frame
	void Update ()
    {
        t.ElapseTime(Time.deltaTime);
	}

    public void LaunchWall(float timeBeforeDestruction)
    {
        GetComponent<SpriteRenderer>().enabled = true;
        t.SetTimer(timeBeforeDestruction);
        t.OnEndTimer += () => { Destroy(gameObject); };

    }
}
