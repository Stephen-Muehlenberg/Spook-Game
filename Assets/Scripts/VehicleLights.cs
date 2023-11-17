using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLights : MonoBehaviour
{
  [Serializable]
  public class LightSet
  {
    public Light[] lights;
    public bool on;
    public float cost;
  }

  [SerializeField] private LightSet cabinMain;
  [SerializeField] private LightSet cabinEmergency;
  [SerializeField] private LightSet frontMain;
  [SerializeField] private LightSet frontHigh;
  [SerializeField] private LightSet sideMain;
  [SerializeField] private LightSet sideHigh;

  public bool cabinMainOn => cabinMain.on;
  public bool cabinEmergencyOn => cabinEmergency.on;

  private void Start()
  {
    SetCabinMain(true);
    SetFrontMain(true);
    SetSideMain(true);
  }

  public void SetCabinMain(bool on) => SetOn(cabinMain, on);
  public void SetCabinEmergency(bool on) => SetOn(cabinEmergency, on);
  public void SetFrontMain(bool on) => SetOn(frontMain, on);
  public void SetFrontHigh(bool on) => SetOn(frontHigh, on);
  public void SetSideMain(bool on) => SetOn(sideMain, on);
  public void SetSideHigh(bool on) => SetOn(sideHigh, on);

  private void SetOn(LightSet lightSet, bool on)
  {
    if (lightSet.on == on) return;

    lightSet.on = on;
    foreach (var light in lightSet.lights)
      light.enabled = on;

    if (on)
      VehiclePower.DrainPower(lightSet.cost);
    else
      VehiclePower.RestorePower(lightSet.cost);
  }
}
