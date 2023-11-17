using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePower : MonoBehaviour
{
  private static float powerGenerated;
  private static float powerRequired;
  public static float powerFractionAvailable { get; private set; }

  public static void DrainPower(float percent)
  {
    powerRequired += percent;
  }

  public static void RestorePower(float percent)
  {
    powerRequired -= percent;
  }

  private void Update()
  {
    var random = Mathf.PerlinNoise1D(Time.time);
    var a = (random * 20) - 1;
    powerGenerated = Mathf.Clamp01(a) * 100;

    if (powerGenerated == 0)
      powerFractionAvailable = 0;
    else if (powerRequired == 0)
      powerFractionAvailable = 1;
    else
      powerFractionAvailable = Mathf.Clamp01(powerGenerated / powerRequired);

    Debug.Log($"PWR {powerFractionAvailable * 100:n1}% ({powerGenerated}/{powerRequired})");
  }
}
