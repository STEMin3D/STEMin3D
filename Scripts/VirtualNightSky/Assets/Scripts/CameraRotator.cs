using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public Vector2 start;
    public Vector2 end;
    // Start is called before the first frame update
    void Start()
    {
        end = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        start = end;
        end = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            Vector2 del = end - start;
            if (del.y*Time.deltaTime*5f+Camera.main.transform.localEulerAngles.x>=275&&del.y * Time.deltaTime * 5f + Camera.main.transform.localEulerAngles.x<=360)
            {
                Camera.main.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, del.y * Time.deltaTime * 3f);
            }
            Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, -del.x * Time.deltaTime * 3f);
        }
    }
}
