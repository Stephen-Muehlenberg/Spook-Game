using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinLightFlicker : MonoBehaviour
{
  [SerializeField] Light cabinLight;

  private void Update()
  {
    cabinLight.intensity = Mathf.Clamp01(
      (14 * Mathf.PerlinNoise1D(Time.time * 2)) - 2.5f);
  }
}
