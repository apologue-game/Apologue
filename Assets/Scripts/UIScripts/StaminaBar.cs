using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    bool facingLeft = true;
    public float regenerationDelay;
    public static bool inCombat = false;

    public int maxStamina = 100;
    public float currentStamina;
    public float nextStaminaValue;
    public DarkGreenStaminaBar darkGreenStaminaBar;

    private void Awake()
    {
        InvokeRepeating(nameof(StaminaRegeneration), 0f, 0.35f);
    }

    void Start()
    {
        currentStamina = maxStamina;
        SetMaximumStamina(maxStamina);
        darkGreenStaminaBar.slider.maxValue = maxStamina;
        darkGreenStaminaBar.slider.value = maxStamina;
    }

    private void Update()
    {
        SetStamina(currentStamina);
    }

    void StaminaRegeneration()
    {
        if (inCombat)
        {
            if (Time.time >= regenerationDelay && slider.value <= slider.maxValue)
            {
                if (currentStamina + 10 > maxStamina)
                {
                    currentStamina = maxStamina;
                }
                else
                {
                    currentStamina += 10;
                }
            }
            return;
        }
        currentStamina = maxStamina;
        slider.value = currentStamina;
        darkGreenStaminaBar.slider.value = currentStamina;
    }

    public void SetMaximumStamina(int stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    public void SetStamina(float stamina)
    {
        if (!inCombat)
        {
            return;
        }
        if (slider.value > stamina)
        {
            StartCoroutine(StaminaDepletionDelay());
        }
        if (slider.value < stamina)
        {
            darkGreenStaminaBar.slider.value = stamina;
        }
        slider.value = stamina;
    }

    IEnumerator StaminaDepletionDelay()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = (int)darkGreenStaminaBar.slider.value; i >= slider.value; i--)
        {
            darkGreenStaminaBar.slider.value -= 1f;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
