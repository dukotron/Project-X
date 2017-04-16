using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RectTransform collapsible;
    public GameObject buildInventory;
    public Transform inventoryItem;
    public Transform inventoryItemParent;
    public Image selectedImage;
    public Text selectedText;

    private Transform selectedItem;
    private RaycastHit hit;

    private bool expandBuild = false;
    private Dictionary<string, Transform> allBlocksAndNames = new Dictionary<string, Transform>();

    void Start()
    {
        GameObject[] blocksList = Resources.LoadAll<GameObject>("Prefabs/Blocks");
        foreach (var obj in blocksList)
        {
            allBlocksAndNames.Add(obj.name, obj.transform);

            Transform newButton = Instantiate(inventoryItem);
            newButton.name = obj.name;
            newButton.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Prefabs/Blocks/" + obj.name + "Img");
            newButton.GetComponentInChildren<Text>().text = obj.name;
            newButton.GetComponent<Button>().onClick.AddListener(() => EquipInventoryItem());
            newButton.SetParent(inventoryItemParent, false);
        }
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
            ColorBlock cb = button.colors;
            cb.normalColor = new Color32(0x5A, 0x59, 0x5D, 0xE1);
            button.colors = cb;
        }
        else
        {
            expandBuild = false;
            StartCoroutine(DelayInventory(false, 0f));
            ColorBlock cb = button.colors;
            cb.normalColor = new Color32(0x2F, 0x2F, 0x2F, 0xFF);
            button.colors = cb;
        }
    }

    public IEnumerator DelayInventory(bool state, float time)
    {
        yield return new WaitForSeconds(time);
        buildInventory.SetActive(state);
    }

    public void EquipInventoryItem()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        selectedItem = allBlocksAndNames[clickedButton.name];
        selectedImage.sprite = clickedButton.transform.FindChild("Image").GetComponent<Image>().sprite;
        selectedText.text = clickedButton.name;
    }
}
