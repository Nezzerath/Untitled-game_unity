using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    #region Button Variable Setup
    // Button Variable Setup
    [SerializeField] private Button Character;
    [SerializeField] private Button Social;
    [SerializeField] private Button Map;
    [SerializeField] private Button Settings;
    [SerializeField] private Button Messages;
    #endregion


    #region Menu Variable Setup
    // Menu variable Setup
    [SerializeField] private GameObject tooltip;
    [SerializeField] public GameObject menu;
    [SerializeField] private bool subMenuOpen;
    #endregion

    [SerializeField] private playermovementinput playerInput;


    [SerializeField] public Slider healthBar;
    [SerializeField] public Slider StaminaBar;
    [SerializeField] public AnimationCurve fillCurve;

    void Start()
    {
        tooltip.SetActive(false);
        subMenuOpen = false;
        playerInput = GetComponentInParent<playermovementinput>();
        


    }

    // Update is called once per frame
    void Update()
    {
        if (menu.activeInHierarchy == false)
        {
            tooltip.SetActive (false);
            //subMenuOpen = false;
        }

        HandleHealth();
        HandleStamina();

        
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

    public void HandleHealth()
    {
        
        var sliderValue = EvaluateCurve(fillCurve, playerInput.currentHealth / playerInput.maxHealth);
        healthBar.value = sliderValue;
    }

    private float EvaluateCurve(AnimationCurve curve, float position)
    {
        return curve.Evaluate(position);
    }

    public void HandleStamina()
    {
        var sliderValue = playerInput.currentStamina / playerInput.maxStamina;
        StaminaBar.value = Convert.ToSingle(sliderValue);
    }


}
