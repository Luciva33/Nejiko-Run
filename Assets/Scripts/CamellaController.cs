using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //this.player = GameObject.Find("cat");

    }

    // Update is called once per frame
    // void LateUpdate()
    // {
    //     Vector3 playerPos = this.player.position;
    //     transform.position = new Vector3(
    //         transform.position.x,
    //         playerPos.y,
    //         transform.position.z
    //     );

    // } 

    void LateUpdate()
    {
        Vector3 playerPos = this.player.position;
        /*
        transform.position = new Vector3(
            transform.position.x,
            playerPos.y,
            transform.position.z
        );
        */
        Vector3 targetPos = new Vector3(
            transform.position.x,
            3,
            playerPos.z - 10

        );
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            0.02f);


    }
}

