using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SpeedLevel = VehicleDriveSystem.SpeedLevel;

public class VehicleSpeedLights : MonoBehaviour
{
  [SerializeField] Renderer lightReverse;
  [SerializeField] Renderer lightNeutral;
  [SerializeField] Renderer lightForwardOne;
  [SerializeField] Renderer lightForwardTwo;
  [SerializeField] Material redOff;
  [SerializeField] Material redOn;
  [SerializeField] Material greenOff;
  [SerializeField] Material greenOn;
  [SerializeField] Material yellowOff;
  [SerializeField] Material yellowOn;

  public void SetSpeed(SpeedLevel speedLevel)
  {
    lightReverse.material = speedLevel == SpeedLevel.Reverse ? yellowOn : yellowOff;
    lightNeutral.material = speedLevel == SpeedLevel.None ? redOn : redOff;
    lightForwardOne.material = speedLevel >= SpeedLevel.Low ? greenOn : greenOff;
    lightForwardTwo.material = speedLevel == SpeedLevel.High ? greenOn : greenOff;
  }
}
