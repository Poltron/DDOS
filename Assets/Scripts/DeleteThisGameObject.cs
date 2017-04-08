using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteThisGameObject : MonoBehaviour {

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
