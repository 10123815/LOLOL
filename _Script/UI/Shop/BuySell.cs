using UnityEngine;
using System.Collections;

public class BuySell : MonoBehaviour
{

    public GameObject shop;
    public GameObject inventory;

    // Use this for initialization
    void Start ( )
    {

    }

    // Update is called once per frame
    void Update ( )
    {

    }

    public void OpenShopWindow()
    {
        shop.SetActive(!shop.activeSelf);
        inventory.SetActive(false);
    }

    public void OpenInventoryWindow()
    {
        inventory.SetActive(!inventory.activeSelf);
        shop.SetActive(false);
    }
}
