using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    bool facingLeft = true;
    public float regenerationDelay;
    public float currentTime;

    public int maxStamina = 100;
    public float currentStamina;
    public float nextStaminaValue;


    private void Awake()
    {
        InvokeRepeating(nameof(StaminaRegeneration), 0f, 0.5f);
    }

    void Start()
    {
        currentStamina = maxStamina;
        SetMaximumStamina(maxStamina);
    }

    private void Update()
    {
        SetStamina(currentStamina);
    }

    void StaminaRegeneration()
    {
        currentTime = Time.time;
        if (Time.time >= regenerationDelay && slider.value <= slider.maxValue)
        {
            if (currentStamina + 5 > maxStamina)
            {
                currentStamina = maxStamina;
            }
            else
            {
                currentStamina += 5;
            }
        }
    }

    public void SetMaximumStamina(int stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    public void SetStamina(float stamina)
    {
        slider.value = stamina;
    }

    public void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
