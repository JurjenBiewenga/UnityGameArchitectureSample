using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Architecture;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class AbilityButton : MonoBehaviour
{
    public UnityGameEvent OnUseEvent;
    public AbilityData Ability;
    public AbilityData[] OtherAbilities;
    public Text CooldownText;
    public Button TargetButton;

    private void Awake()
    {
        CooldownText.gameObject.SetActive(false);
        TargetButton.onClick.RemoveAllListeners();
        TargetButton.onClick.AddListener(UseAbility);
    }

    void Update()
    {
        var otherAbilityInUse = OtherAbilities.Any(x => x.Active);
        if (Ability.Active)
        {
            TargetButton.interactable = false;
        }
        else if (Ability.OnCooldown)
        {
            CooldownText.gameObject.SetActive(true);
            CooldownText.text = Ability.RemainingCooldown.ToString("F1");
        }
        else if (!otherAbilityInUse)
        {
            TargetButton.interactable = true;
            CooldownText.gameObject.SetActive(false);
        }

        if (otherAbilityInUse)
        {
            TargetButton.interactable = false;
            if (!Ability.OnCooldown)
            {
                CooldownText.gameObject.SetActive(false);
            }
        }
    }

    void UseAbility()
    {
        OnUseEvent.Invoke();
    }
}