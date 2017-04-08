using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedWallDestruction : MonoBehaviour
{
    [SerializeField]
    private GameObject FX;

    [SerializeField]
    private SpriteRenderer part1;
    [SerializeField]
    private SpriteRenderer part2;
    [SerializeField]
    private SpriteRenderer part3;
    [SerializeField]
    private SpriteRenderer part4;

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
        part1.enabled = true;
        part1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        part1.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(randomMin, randomMax), 5));
        part2.transform.parent = null;
        part2.enabled = true;
        part2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        part2.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-randomMin, -randomMax), 5));
        part3.transform.parent = null;
        part3.enabled = true;
        part3.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        part3.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(randomMin, randomMax), 5));
        part4.transform.parent = null;
        part4.enabled = true;
        part4.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        part4.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-randomMin, -randomMax), 5));

        Destroy(gameObject);
    }
}
