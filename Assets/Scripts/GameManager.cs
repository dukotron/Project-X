using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RectTransform collapsible;
    public GameObject buildInventory;

    private Transform selectedItem;
    private RaycastHit hit;

    private bool expandBuild = false;

    void Start()
    {

    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetMouseButtonDown(0) && selectedItem != null)
            {
                Vector3 incomingVec = hit.normal;

                if (incomingVec == new Vector3(0, 1, 0))
                    Instantiate(selectedItem, new Vector3(hit.transform.position.x, hit.transform.position.y + hit.transform.localScale.y, hit.transform.position.z), Quaternion.identity);
                else if (incomingVec == new Vector3(0, -1, 0))
                    Instantiate(selectedItem, new Vector3(hit.transform.position.x, hit.transform.position.y - hit.transform.localScale.y, hit.transform.position.z), Quaternion.identity);
                else if (incomingVec == new Vector3(0, 0, 1))
                    Instantiate(selectedItem, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + hit.transform.localScale.z), Quaternion.identity);
                else if (incomingVec == new Vector3(0, 0, -1))
                    Instantiate(selectedItem, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - hit.transform.localScale.z), Quaternion.identity);
                else if (incomingVec == new Vector3(1, 0, 0))
                    Instantiate(selectedItem, new Vector3(hit.transform.position.x + hit.transform.localScale.x, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
                else if (incomingVec == new Vector3(-1, 0, 0))
                    Instantiate(selectedItem, new Vector3(hit.transform.position.x - hit.transform.localScale.x, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);

                print("Block Added");
            }
        }

        //In the inspector, offesetMax.x is right, offsetMax.y is top, offsetMin.x is left, offsetMin.y is bottom, we're changing right and bottom
        //so we get a cool minimize/maximize effect
        if (expandBuild)
        {
            collapsible.offsetMax = new Vector2(Mathf.Lerp(collapsible.offsetMax.x, 0, Time.deltaTime * 5), collapsible.offsetMax.y);
            collapsible.offsetMin = new Vector2(collapsible.offsetMin.x, Mathf.Lerp(collapsible.offsetMin.y, 0, Time.deltaTime * 5));
        }
        else
        {
            collapsible.offsetMax = new Vector2(Mathf.Lerp(collapsible.offsetMax.x, -300, Time.deltaTime * 5), collapsible.offsetMax.y);
            collapsible.offsetMin = new Vector2(collapsible.offsetMin.x, Mathf.Lerp(collapsible.offsetMin.y, 440, Time.deltaTime * 5));
        }
    }

    public void ExpandBuild(Button button)
    {
        if (!expandBuild) {
            expandBuild = true;
            StartCoroutine(DelayInventory(true, 0.9f));
            button.image.color = new Color32(0xCE, 0xCE, 0xCE, 0x96);
        }
        else
        {
            expandBuild = false;
            StartCoroutine(DelayInventory(false, 0f));
            button.image.color = new Color32(0xFF, 0xFF, 0xFF, 0xE1);
        }
    }

    public IEnumerator DelayInventory(bool state, float time)
    {
        yield return new WaitForSeconds(time);
        buildInventory.SetActive(state);
    }
}
