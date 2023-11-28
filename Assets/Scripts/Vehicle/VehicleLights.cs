using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VehicleLights : MonoBehaviour
{
  [Serializable]
  public class LightSet
  {
    public string name;
    public Light[] lights;
    public VehicleLightSwitch lightSwitch;
    /// <summary>Has the user set the light on?</summary>
    public bool switchedOn;
    /// <summary>Should the Light component be on in the scene?
    /// If both highbeams and front lights are on, only one
    /// will be enabled.</summary>
    public bool enableLight;
    public float cost;
    public float maxIntensity = 1;
  }

  [SerializeField] private LightSet cabinLightSet;
  [SerializeField] private LightSet frontLightSet;
  [SerializeField] private LightSet highbeamsLightSet;
  [SerializeField] private LightSet sideLightSet;
  [SerializeField] private LightSet emptyLightSet;
  private List<LightSet> allLights;

  public UnityEvent<bool> cabinLightsChanged = new();

  private void Awake()
  {
    allLights = new()
    {
      cabinLightSet, frontLightSet,
      highbeamsLightSet, sideLightSet
    };
  }

  private void Start()
  {
    SetOn(cabinLightSet, true);
    SetOn(frontLightSet, true);
    SetOn(highbeamsLightSet, false);
    SetOn(sideLightSet, false);
    SetOn(emptyLightSet, true);
  }

  private void SetOn(LightSet lightSet, bool on)
  {
    lightSet.switchedOn = on;
    lightSet.lightSwitch.SetOn(on);

    // Highbeams only turn on if front lights are also on.
    // Turning on highbeams disables the front lights.
    if (lightSet == frontLightSet || lightSet == highbeamsLightSet)
    {
      frontLightSet.enableLight = 
        frontLightSet.switchedOn
        && !highbeamsLightSet.switchedOn;
      highbeamsLightSet.enableLight = 
        frontLightSet.switchedOn
        && highbeamsLightSet.switchedOn;
      Debug.Log($"front on {frontLightSet.switchedOn} enabled {frontLightSet.enableLight}, high on {highbeamsLightSet.switchedOn} enabled {highbeamsLightSet.enableLight}");
      foreach (var light in frontLightSet.lights)
        light.gameObject.SetActive(frontLightSet.enableLight);
      foreach (var light in highbeamsLightSet.lights)
        light.gameObject.SetActive(highbeamsLightSet.enableLight);
    }
    else
    {
      lightSet.enableLight = on;
      foreach (var light in lightSet.lights)
        light.gameObject.SetActive(lightSet.enableLight);
    }

    // This only works for normal light set logic. Need custom logic for the front/highbeams.
    if (lightSet.enableLight)
      VehiclePower.DrainPower(lightSet.cost);
    else
      VehiclePower.RestorePower(lightSet.cost);
    
    if (lightSet == cabinLightSet)
      cabinLightsChanged?.Invoke(cabinLightSet.switchedOn);
  }

  public void OnSwitchClicked(int index)
  {
    if (index == 0)
      SetOn(highbeamsLightSet, !highbeamsLightSet.switchedOn);
    else if (index == 1)
      SetOn(frontLightSet, !frontLightSet.switchedOn);
    else if (index == 2)
      SetOn(cabinLightSet, !cabinLightSet.switchedOn);
    else if (index == 3)
      SetOn(sideLightSet, !sideLightSet.switchedOn);
    else
      SetOn(
        lightSet: emptyLightSet, 
        on: !emptyLightSet.lightSwitch.inOnPosition);
  }

  private void Update()
  {
//    Debug.Log($"high {highbeamsLightSet.enableLight}, front {frontLightSet.enableLight}, cabin {cabinLightSet.enableLight}");
    foreach (var set in allLights)
    {
      if (!set.enableLight) continue;
      foreach (var light in set.lights)
        light.intensity = set.maxIntensity * VehiclePower.powerFractionAvailable;
    }
  }
}
