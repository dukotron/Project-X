using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    public float dragSpeed = 2f;
    public float moveSpeed = 2f;

    private Vector3 dragOrigin;
    private float xDir;
    private float yDir;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 4f;
        if (!Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 2f;

        if (Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1))
        {
            Cursor.visible = true;
            return;
        }

        Vector3 mouseDir = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

        xDir += Mathf.Clamp(mouseDir.x, -0.3f, 0.3f) * dragSpeed;
        yDir += Mathf.Clamp(-mouseDir.y, -0.2f, 0.2f) * dragSpeed;

        //unity uses x rotation for up/down so placed yDir from mouse in x............
        transform.rotation = Quaternion.Euler(yDir, xDir, 0f);
    }


}