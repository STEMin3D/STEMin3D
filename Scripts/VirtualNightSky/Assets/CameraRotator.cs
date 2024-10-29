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
            transform.Rotate(new Vector3(del.y, -del.x, 0)*Time.deltaTime*10f);
        }
    }
}
