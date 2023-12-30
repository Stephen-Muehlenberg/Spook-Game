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
    /// <summary>Has the user set the physical light switch on?</summary>
    public bool switchedOn;
    /// <summary>Should the light-emitting component be on in the scene?
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
  public UnityEvent anyLightChanged = new();

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
    allLights.ForEach(lightSet => SetOn(
      lightSet: lightSet,
      on: lightSet.switchedOn,
      isInitialSetting: true));
  }

  private void SetOn(LightSet lightSet, bool on, bool isInitialSetting = false)
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
      foreach (var light in frontLightSet.lights)
        light.gameObject.SetActive(frontLightSet.enableLight);
      foreach (var light in highbeamsLightSet.lights)
        light.gameObject.SetActive(highbeamsLightSet.enableLight);
    }
    // Other lights just turn on or off as normal.
    else
    {
      lightSet.enableLight = on;
      foreach (var light in lightSet.lights)
        light.gameObject.SetActive(lightSet.enableLight);
    }

    if (lightSet.enableLight)
    {
      if (lightSet == highbeamsLightSet)
      {
        if (frontLightSet.switchedOn)
        {
          VehiclePower.lightsPowerDrain -= frontLightSet.cost;
          VehiclePower.lightsPowerDrain += highbeamsLightSet.cost;
        }
      }
      else if (lightSet == frontLightSet)
      {
        VehiclePower.lightsPowerDrain += highbeamsLightSet.switchedOn
          ? highbeamsLightSet.cost
          : frontLightSet.cost;
      }
      else
        VehiclePower.lightsPowerDrain += lightSet.cost;
    }
    else
    {
      // If we're setting the lights' initial states,
      // don't "restore" power to lights that were
      // never on to begin with.
      if (!isInitialSetting)
      {
        if (lightSet == highbeamsLightSet)
        {
          if (frontLightSet.switchedOn)
          {
            VehiclePower.lightsPowerDrain -= highbeamsLightSet.cost;
            VehiclePower.lightsPowerDrain += frontLightSet.cost;
          }
        }
        else if (lightSet == frontLightSet)
        {
          VehiclePower.lightsPowerDrain -= highbeamsLightSet.switchedOn
            ? highbeamsLightSet.cost
            : frontLightSet.cost;
        }
        else
          VehiclePower.lightsPowerDrain -= lightSet.cost;
      }
    }

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
    anyLightChanged?.Invoke();
  }

  private void Update()
  {
    foreach (var set in allLights)
    {
      if (!set.enableLight) continue;
      foreach (var light in set.lights)
        light.intensity = set.maxIntensity * VehiclePower.powerFractionAvailable;
    }
  }
}
