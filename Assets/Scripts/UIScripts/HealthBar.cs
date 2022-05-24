using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    bool facingLeft = true;
    public YellowHealthBar yellowHealthBar;

    public void SetMaximumHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        yellowHealthBar.slider.maxValue = health;
        yellowHealthBar.slider.value = health;
    }

    public void SetHealth(float health)
    {
        if (slider.value > health)
        {
            StartCoroutine(HealthDepletionDelay());
        }
        if (slider.value < health)
        {
            yellowHealthBar.slider.value = health;
        }
        slider.value = health;
    }

    IEnumerator HealthDepletionDelay()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = (int)yellowHealthBar.slider.value; i >= slider.value; i--)
        {
            yellowHealthBar.slider.value -= 1f;
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
