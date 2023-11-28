using System;
using UnityEngine;

public class VehicleLightSwitch : MonoBehaviour
{
  public bool inOnPosition => transform.localRotation.eulerAngles.z < 180;

  public void SetOn(bool on)
  {
    transform.localRotation = Quaternion.Euler(
      0,
      0,
      on ? 45 : -45);
  }
}
