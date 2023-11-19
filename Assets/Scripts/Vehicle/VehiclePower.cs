using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VehiclePower : MonoBehaviour
{
  public static float powerGenerated { get; private set; }
  public static float powerRequired { get; private set; }
  public static float ghostPowerInhibited;
  public static float powerFractionAvailable { get; private set; }

  private static float randomSeedA, randomSeedB;

  private void Awake()
  {
    randomSeedA = Random.value;
    randomSeedB = Random.value;
  }

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
    var randomA = Mathf.PerlinNoise(Time.time, randomSeedA);
    var randomB = Mathf.PerlinNoise(Time.time * 4, randomSeedB);
    powerGenerated = Mathf.Clamp((randomA + randomB - 0.05f) * 100, 0, 100) - ghostPowerInhibited;

    if (powerGenerated == 0)
      powerFractionAvailable = 0;
    else if (powerRequired == 0)
      powerFractionAvailable = 1;
    else
      powerFractionAvailable = Mathf.Clamp01(powerGenerated / powerRequired);

  //  Debug.Log($"PWR {powerFractionAvailable * 100:n1}% ({powerGenerated:n1}/{powerRequired})");
  }
}
