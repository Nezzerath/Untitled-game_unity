using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Button Character;
    public Button Social;
    public Button Map;
    public Button Settings;
    public Button Messages;
    public GameObject tooltip;
    public GameObject menu;
    [SerializeField] private bool subMenuOpen;
    // Start is called before the first frame update
    void Start()
    {
        tooltip.SetActive(false);
        subMenuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.activeInHierarchy == false)
        {
            tooltip.SetActive (false);
            //subMenuOpen = false;
        }
    }

    public void Hover(Button button)
    {
        
        tooltip.transform.position = button.transform.position + new Vector3(35, -15, 0);
        tooltip.GetComponentInChildren<TMP_Text>().text = button.name.ToString();
        tooltip.SetActive(true);
        
        
    }

    public void UnHover()
    {
        tooltip.SetActive(false);
        subMenuOpen = true;
        
    }

    public void CharacterMenu()
    {
        tooltip.SetActive(false);
        subMenuOpen = true;
    }
    public void SocialMenu()
    {
        tooltip.SetActive(false);
        subMenuOpen = true;
    }
    public void MessagesMenu()
    {
        tooltip.SetActive(false);
        subMenuOpen = true;
    }
    public void MapMenu()
    {
        tooltip.SetActive(false);
        subMenuOpen = true;
    }
    public void SettingsMenu()
    {
        tooltip.SetActive(false);
        subMenuOpen = true;
    }


}
