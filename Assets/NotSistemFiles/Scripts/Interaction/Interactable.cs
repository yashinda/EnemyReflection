using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
     public Canvas canvas;
     public  MeshRenderer targetRenderer; 
     private MaterialPropertyBlock _propBlock;
     
     private const int OutlineMaterialIndex = 1; 
     private void Awake()
     {
          _propBlock = new MaterialPropertyBlock();
     }

     private void Start()
     {
          ToggleUI(false);
     }

     public void ToggleUI(bool isVisible)
     {
          if (canvas != null)
          {
               canvas.gameObject.SetActive(isVisible);
          }
          
          if (targetRenderer != null)
          {
               targetRenderer.GetPropertyBlock(_propBlock, OutlineMaterialIndex);
               _propBlock.SetFloat("_Scale", isVisible ? 1.15f : 0.0f);
               targetRenderer.SetPropertyBlock(_propBlock, OutlineMaterialIndex);
          }
     }
     
     public abstract void Interact(PlayerInteraction player = null);
}
