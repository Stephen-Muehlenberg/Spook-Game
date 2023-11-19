using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFilter : MonoBehaviour
{
  [SerializeField] private Renderer[] levelLights;
  [SerializeField] private Material lightOn;
  [SerializeField] private Material lightOff;
  
  private ParticleSystem sparks;

  private void Awake()
  {
   // sparks = GetComponent<ParticleSystem>();
  //  var emission = sparks.emission;
   // emission.enabled = false;
  }

  public void SetLevel(int level)
  {
    for (int i = 0; i < levelLights.Length; i++)
    {
      levelLights[i].material = level > i
        ? lightOn : lightOff;
    }
  }
}
