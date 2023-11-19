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
    smoothedPower = Mathf.SmoothDamp(
      current: smoothedPower,
      target: VehiclePower.powerGenerated / 10f,
      currentVelocity: ref smoothedPowerVelocity,
      smoothTime: 1);
    for (int i = 0; i < 3; i++)
      powerLights[i].material =
        smoothedPower > i ? redOn : redOff;
    for (int i = 3; i < 6; i++)
      powerLights[i].material =
        smoothedPower > i ? yellowOn : yellowOff;
    for (int i = 6; i < 10; i++)
      powerLights[i].material =
        smoothedPower > i ? greenOn : greenOff;
  }
}
