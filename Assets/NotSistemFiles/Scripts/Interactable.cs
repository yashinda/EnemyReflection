using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
     public Canvas canvas;
     public Material materialOutline;

     private void Start()
     {
          ToggleUI(false);
     }

     public void ToggleUI(bool isVisible)
     {
          canvas.gameObject.SetActive(isVisible);
          materialOutline.SetFloat("_Scale", isVisible ? 1.15f : 0.0f);
     }
     
     public abstract void Interact(PlayerInteraction player = null);
}
