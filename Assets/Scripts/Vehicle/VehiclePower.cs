using UnityEngine;

public class VehiclePower : MonoBehaviour
{
  public static float powerGenerated { get; private set; }
//  public static float ghostPowerInhibited;
  public static float powerFractionAvailable { get; private set; }

  private static float randomSeedA, randomSeedB;

  /// <summary>Total amount of power generated by engine. Typically ranges from 97 to 103.</summary>
//  private static float percentPowerGenerated;
  /// <summary>Percentage of normal max power currently used by the engine.</summary>
  public static float enginePowerDrain;
  /// <summary>Percentage of normal max power currently used by the lights.</summary>
  public static float lightsPowerDrain;
  /// <summary>Percentage of normal max power currently drained by ghosts.</summary>
  public static float ghostPowerDrain;
  /// <summary>Power generated minus power drained (engine, lights, and ghost).</summary>
  public static float powerAvailable { get; private set; }

  /// <summary>Percentage of normal max power currently usable by vehicle. Equals generated - ghost drain.</summary>
  public static float usablePower { get; private set; }
  private float internalPowerDrain;

  private void Awake()
  {
    randomSeedA = Random.value;
    randomSeedB = Random.value;
  }

  // Cached to avoid re-allocation each frame.
  private float randomA, randomB;

  private void Update()
  {
    // Randomise power generated. Averages 100.
    randomA = Mathf.PerlinNoise(Time.time, randomSeedA);
    randomB = Mathf.PerlinNoise(Time.time * 4, randomSeedB);    
    powerGenerated = Mathf.Clamp((randomA + randomB - 0.05f) * 100, 0, 100);

    // Remove power drained by ghosts.
    usablePower = powerGenerated - ghostPowerDrain;
    internalPowerDrain = enginePowerDrain + lightsPowerDrain;
    if (powerGenerated - ghostPowerDrain <= 0)
      powerFractionAvailable = 0;
    else if (internalPowerDrain == 0)
      powerFractionAvailable = 1;
    else
      powerFractionAvailable = Mathf.Clamp01(usablePower / internalPowerDrain);

    //Debug.Log($"generated {powerGenerated:n1}, usable {usablePower:n1}, used {internalPowerDrain:n1} (engine {enginePowerDrain:n1}, lights {lightsPowerDrain:n1}), spare {sparePower:n1}, fraction {powerFractionAvailable:n1}");
  }
}
