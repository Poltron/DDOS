using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;     //array (list) of all back- and forgrounds to be parallaxed
    private float[] parallaxScales;  //the proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;    //how smooth the parallax is going to be, Must be above 0 otherwize the parallax will not work
    public bool FirstBackgroundIsFixed;

    private Transform cam;  //reference to the camera's transform
    private Vector3 previousCamPos;     //the position of the camera in the previous frame

    //called before Start(), using to assign references.
    void Awake()
    {
        //set up camera the reference
        cam = transform;
    }

    // Use this for initialization
    void Start()
    {
        // store previous frame
        previousCamPos = cam.position;

        //declares the length of the array
        parallaxScales = new float[backgrounds.Length];

        //assigning coresponding parallaxScales
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //for each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (i == 0 && FirstBackgroundIsFixed)
            {
                backgrounds[i].position = new Vector3(cam.position.x, backgrounds[i].position.z, backgrounds[i].position.z);
            }

            //create a target position which is the backgrounds current position with it's target x position
            Vector3 parallax = (previousCamPos - cam.position) * (parallaxScales[i] / smoothing);

            //fade batween current position and the target position using lerp
            backgrounds[i].position = new Vector3(backgrounds[i].position.x + parallax.x, backgrounds[i].position.y + parallax.y, backgrounds[i].position.z);
        }

        //set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}

/*

Code written by joedanhol, find this on GitHub at https://github.com/joedanhol/Parallax2D :D Last test done 27/04/15
Feel free to edit this at your own will, this code was made to be compleatly hackable, and feel free to message me at
joedanhol@gmail.com for any help required.

*/
