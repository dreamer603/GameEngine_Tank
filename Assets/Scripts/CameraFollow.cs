using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform camStartPosition;
    public float posY;
    public Transform tank;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = camStartPosition.position;
        posY = camStartPosition.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (tank == null)
        {
            return;
        }
        transform.position = new Vector3(tank.position.x, posY, tank.position.z + -8);
        transform.LookAt(tank);
    }   

    private void Move()
    {
        float dirZ = Input.GetAxis("Vertical");
        float dirX = Input.GetAxis("Horizontal");

        Vector3 rot = new Vector3(0, dirX, 0).normalized;

        gameObject.transform.position = new Vector3(dirZ, 0, 0);

        gameObject.transform.Rotate(rot * 4f * Time.fixedDeltaTime);
    }
}
