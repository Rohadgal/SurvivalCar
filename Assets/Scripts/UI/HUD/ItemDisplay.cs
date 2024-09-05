using TMPro;
using UnityEngine;

public class ItemDisplay : MonoBehaviour {
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private TMP_Text[] nameTexts;
    [SerializeField] private TMP_Text[] descriptionTexts;
    [SerializeField] private Item[] items;
    [SerializeField] private Transform[] spawnPositions;

    private void OnEnable() {
        ExperienceManager.OnLevelUp += showInventory;
    }

    private void OnDisable() {
        ExperienceManager.OnLevelUp -= showInventory;
    }

    private void Start() {
        inventoryUI.SetActive(false);
    }

    private void showInventory() {
        inventoryUI.SetActive(true);
        for (int i = 0; i < nameTexts.Length; i++) {
            Item randomItem = items[Random.Range(0, items.Length)];
            nameTexts[i].text = randomItem.itemName;
            descriptionTexts[i].text = randomItem.itemDescription;
            Transform uiItem = Instantiate(randomItem.UIRepresentation).transform;
            uiItem.transform.SetParent(spawnPositions[i]);
            uiItem.localPosition = Vector3.zero;
            uiItem.localScale = new Vector3(1, 1, 1);
            uiItem.GetComponent<InventoryItem>().setItem(randomItem);
        }
    }

    public void hideInventory() {
        inventoryUI.SetActive(false);
        foreach (Transform spawnPosition in spawnPositions) {
            if (spawnPosition.transform.childCount > 0) {
                Destroy(spawnPosition.GetChild(0).gameObject);
            }
        }
        Time.timeScale = 1f;
    }
}