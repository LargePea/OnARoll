using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralex : MonoBehaviour
{
    private float length;
    private float startPosX;
    private float startPosY;
    private Vector3 camStartPos;
    [SerializeField] GameObject cam;
    [SerializeField] float parallaxStrengthHorizontal;
    [SerializeField] float parallaxStrengthVertical;

    
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        camStartPos = cam.transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void Update()
    {
        //Create parralex effect by having the background follow the camera less and less with the weaker the parralex strength
        float tempX = ((cam.transform.position.x - camStartPos.x) * (1 - parallaxStrengthHorizontal));
        float distX = ((cam.transform.position.x - camStartPos.x) * parallaxStrengthHorizontal);
        float distY = ((cam.transform.position.y - camStartPos.y) * parallaxStrengthVertical);

        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        //move background if camera position is ahead or behind the length of the background
        if (tempX > startPosX + length) startPosX += length;
        else if (tempX < startPosX - length) startPosX -= length;

        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
