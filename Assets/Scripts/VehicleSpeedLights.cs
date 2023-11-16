using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpeedLights : MonoBehaviour
{
  [SerializeField] Renderer[] lights;
  [SerializeField] Material redOff;
  [SerializeField] Material redOn;
  [SerializeField] Material greenOff;
  [SerializeField] Material greenOn;
  [SerializeField] Material yellowOff;
  [SerializeField] Material yellowOn;

  public void SetSpeed(int speed)
  {
    Debug.Log($"speed {speed}");
    lights[0].material = speed == 0 ? redOn : greenOn;
    for (int i = 1; i <= 2; i++)
      lights[i].material = speed > 0 ? greenOn : greenOff;
    for (int i = 3; i <= 4; i++)
      lights[i].material = speed > 1 ? yellowOn : yellowOff;
    for (int i = 5; i <= 6; i++)
      lights[i].material = speed > 2 ? redOn : redOff;
  }
}
