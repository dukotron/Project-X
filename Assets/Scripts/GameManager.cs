using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform genericBlock;

    private RaycastHit hit;

    void Start()
    {

    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 incomingVec = hit.normal;

                if (incomingVec == new Vector3(0, 1, 0))
                    Instantiate(genericBlock, new Vector3(hit.transform.position.x, hit.transform.position.y + hit.transform.localScale.y, hit.transform.position.z), Quaternion.identity);
                else if (incomingVec == new Vector3(0, -1, 0))
                    Instantiate(genericBlock, new Vector3(hit.transform.position.x, hit.transform.position.y - hit.transform.localScale.y, hit.transform.position.z), Quaternion.identity);
                else if (incomingVec == new Vector3(0, 0, 1))
                    Instantiate(genericBlock, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + hit.transform.localScale.z), Quaternion.identity);
                else if (incomingVec == new Vector3(0, 0, -1))
                    Instantiate(genericBlock, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - hit.transform.localScale.z), Quaternion.identity);
                else if (incomingVec == new Vector3(1, 0, 0))
                    Instantiate(genericBlock, new Vector3(hit.transform.position.x + hit.transform.localScale.x, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
                else if (incomingVec == new Vector3(-1, 0, 0))
                    Instantiate(genericBlock, new Vector3(hit.transform.position.x - hit.transform.localScale.x, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);

                print("Block Added");
            }
        }
    }
}
