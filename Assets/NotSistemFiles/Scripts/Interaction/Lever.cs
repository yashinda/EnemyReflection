using System.Collections;
using UnityEngine;

public class Lever : Interactable
{
    public Water[] leverWater;
    public bool leverActive = false;
    
    private bool isMoving = false; 

    public override void Interact(PlayerInteraction player = null)
    {
        if (player == null || isMoving)
            return;
        
        StartCoroutine(ToggleLever());
    }

    private IEnumerator ToggleLever()
    {
        isMoving = true;
        leverActive = !leverActive;
        
        foreach (Water water in leverWater)
        {
            if (water != null)
                water.TogglePool();
        }
        
        float duration = 1.0f;
        float elapsedTime = 0.0f;
        
        // Плавный поворот рычага
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float speed = 55.0f * Time.deltaTime;
            
            if (leverActive)
                transform.Rotate(speed, 0f, 0f);
            else
                transform.Rotate(-speed, 0f, 0f);

            yield return null;
        }

        isMoving = false;
    }
}