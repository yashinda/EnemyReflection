using UnityEngine;

public class Water : MonoBehaviour
{
    public bool waterIsDown = false;
    public Animator animatorWater;

    public void TogglePool()
    {
        waterIsDown = !waterIsDown;

        if (waterIsDown)
        {
            animatorWater.SetBool("isDown", true);
            animatorWater.SetBool("isUp", false);
        }
        else
        {
            animatorWater.SetBool("isUp", true);
            animatorWater.SetBool("isDown", false);
        }
    }
}