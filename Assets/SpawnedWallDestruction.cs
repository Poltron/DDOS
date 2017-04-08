using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedWallDestruction : MonoBehaviour
{
    [SerializeField]
    private GameObject FX;

    [SerializeField]
    private WallPartDestruction part1;
    [SerializeField]
    private WallPartDestruction part2;
    [SerializeField]
    private WallPartDestruction part3;
    [SerializeField]
    private WallPartDestruction part4;

    [SerializeField]
    private float TimeBeforeWallPartsAreDestroyed;

    public float randomMin;
    public float randomMax;

    void Start ()
    {
	}
	
	void Update ()
    {
	}

    public void DoDestruction()
    {
        Instantiate(FX, part2.transform.position, Quaternion.identity);

        part1.transform.parent = null;
        part1.LaunchWall(TimeBeforeWallPartsAreDestroyed);
        part1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        part1.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(randomMin, randomMax), 5));

        part2.transform.parent = null;
        part2.LaunchWall(TimeBeforeWallPartsAreDestroyed);
        part2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        part2.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-randomMin, -randomMax), 5));

        part3.transform.parent = null;
        part3.LaunchWall(TimeBeforeWallPartsAreDestroyed);
        part3.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        part3.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(randomMin, randomMax), 5));

        part4.transform.parent = null;
        part4.LaunchWall(TimeBeforeWallPartsAreDestroyed);
        part4.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        part4.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-randomMin, -randomMax), 5));

        Destroy(gameObject);
    }
}
