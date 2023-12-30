using UnityEngine;

public class VehiclePowerDial : MonoBehaviour
{
  [SerializeField] private Renderer[] powerLights;
  [SerializeField] Material redOff;
  [SerializeField] Material redOn;
  [SerializeField] Material greenOff;
  [SerializeField] Material greenOn;
  [SerializeField] Material yellowOff;
  [SerializeField] Material yellowOn;

  private float smoothedPower = 0;
  private float smoothedPowerVelocity;

  void Update()
  {
    int redThreshold = Mathf.RoundToInt(VehiclePower.enginePowerDrain / 10f);
    int yellowThreshold = Mathf.RoundToInt((VehiclePower.enginePowerDrain + VehiclePower.lightsPowerDrain) / 10f);

    smoothedPower = Mathf.SmoothDamp(
      current: smoothedPower,
      target: VehiclePower.usablePower / 10,
      currentVelocity: ref smoothedPowerVelocity,
      smoothTime: 1);
    for (int i = 0; i < 10; i++)
    {
      if (i < redThreshold)
      {
        powerLights[i].material =
          smoothedPower > i ? redOn : redOff;
      }
      else if (i < yellowThreshold)
      {
        powerLights[i].material =
          smoothedPower > i ? yellowOn : yellowOff;
      }
      else
      {
        powerLights[i].material =
          smoothedPower > i ? greenOn : greenOff;
      }
    }
  }
}
