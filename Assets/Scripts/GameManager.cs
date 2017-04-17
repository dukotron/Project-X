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
    private Transform loadSelectedShip;
    private RaycastHit hit;

    private bool expandBuild = false;
    private Dictionary<string, Transform> loadedBlocksAndNames = new Dictionary<string, Transform>();
    private List<Transform> loadedButtons = new List<Transform>();
    private List<Transform> buildAreas = new List<Transform>();

    void Start()
    {
        LoadInventoryTab("Prefabs/ShipTypes/");
    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetMouseButtonDown(0) && selectedItem != null && Input.GetMouseButton(1))
            {
                Vector3 placePos = hit.transform.position + hit.normal;
                Transform parent = null;
                bool blocked = true;

                foreach (Transform tr in buildAreas)
                {
                    if (tr.FindChild("BuildArea(Clone)").GetComponentInChildren<BoxCollider>().bounds.Contains(placePos)
                        && tr.FindChild("BuildArea(Clone)").GetComponentInChildren<BoxCollider>().bounds.Contains(Camera.main.transform.position))
                    {
                        blocked = false;
                        parent = tr;
                    }

                }

                if (blocked)
                    return;

                Instantiate(selectedItem, placePos, Quaternion.identity, parent);

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
        selectedItem = loadedBlocksAndNames[clickedButton.name];
        selectedImage.sprite = clickedButton.transform.FindChild("Image").GetComponent<Image>().sprite;
        selectedText.text = clickedButton.name;
    }

    public void ShowShipsTab()
    {
        LoadInventoryTab("Prefabs/ShipTypes/");
    }

    public void ShowBlocksTab()
    {
        LoadInventoryTab("Prefabs/Blocks/");
    }

    public void SpawnNewShip()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        loadSelectedShip = loadedBlocksAndNames[clickedButton.name];
        buildAreas.Add(Instantiate(loadSelectedShip, Camera.main.transform.position, Quaternion.identity));
    }

    private void LoadInventoryTab(string path)
    {
        GameObject[] blocksList = Resources.LoadAll<GameObject>(path);
        loadedBlocksAndNames.Clear();

        foreach (Transform t in loadedButtons)
            Destroy(t.gameObject);
        loadedButtons.Clear();

        foreach (var obj in blocksList)
        {
            loadedBlocksAndNames.Add(obj.name, obj.transform);

            Transform newButton = Instantiate(inventoryItem);
            newButton.name = obj.name;
            newButton.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(path + obj.name + "Img");
            newButton.GetComponentInChildren<Text>().text = obj.name;

            if (path == "Prefabs/ShipTypes/")
                newButton.GetComponent<Button>().onClick.AddListener(() => SpawnNewShip());
            else
                newButton.GetComponent<Button>().onClick.AddListener(() => EquipInventoryItem());

            newButton.SetParent(inventoryItemParent, false);
            loadedButtons.Add(newButton);
        }
    }
}
