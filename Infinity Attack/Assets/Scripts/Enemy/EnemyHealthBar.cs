using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    public Slider Slider;
    public Color Low;
    public Color High;
    public Vector3 OffsetMoveToRight;
    public Vector3 OffsetMoveToLeft;
    public Vector3 OffsetAttackRight;
    public Vector3 OffsetAttackLeft;
    public Vector3 OffsetTakeDamageLeft;
    public Vector3 OffsetTakeDamageRight;

    public void SetHealth(int health, int maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;

        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }

    public void SetPositionHealthBarMoveToRight()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetMoveToRight);
    }

    public void SetPositionHealthBarMoveToLeft()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetMoveToLeft);
    }

    public void SetPositionHealthBarAttackRight()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetAttackRight);
    }

    public void SetPositionHealthBarAttackLeft()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetAttackLeft);
    }

    public void SetPositionHealthBarTakeDamageLeft()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetTakeDamageLeft);
    }

    public void SetPositionHealthBarTakeDamageRight()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetTakeDamageRight);
    }
}
