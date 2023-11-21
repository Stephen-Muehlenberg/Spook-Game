using UnityEngine;

public class VehicleDriveSystem : MonoBehaviour
{
  public enum SpeedLevel { Reverse = -1, None, Low, High }

  [SerializeField] private Rigidbody vehicle;
  [SerializeField] private Transform steeringWheel;
  [SerializeField] private VehicleSpeedLights speedLights;
  [SerializeField] private float[] accelerationPerSpeedLevel;
  [SerializeField] private float steerSpeed = 6f;
  [SerializeField] private Transform particleSpawnPoint;

  [SerializeField] private float[] powerUsePerSpeedLevel;

  private SpeedLevel speedLevel;
  private float currentAcceleration;
  private float steerAmount;
  private float steerVelocity;

  private void Awake()
  {
    speedLevel = SpeedLevel.None;
    SetSpeedLevel(SpeedLevel.None);
  }

  private void Update()
  {
    UpdateSteeringInput();
    UpdateAccelerationInput();

    // Move particles ahead of vehicle so we don't leave the particle
    // spawn area.
    particleSpawnPoint.localPosition = 2 * vehicle.velocity;
  }

  private void FixedUpdate()
  {
    ApplySteeringAndAcceleration();
  }

  private void UpdateSteeringInput()
  {
    if (Input.GetKey(KeyCode.A))
    {
      steerAmount -= Time.deltaTime * steerSpeed;
      if (steerAmount < -1)
        steerAmount = -1;
      steerVelocity = 0;
    }
    else if (Input.GetKey(KeyCode.D))
    {
      steerAmount += Time.deltaTime * steerSpeed;
      if (steerAmount > 1)
        steerAmount = 1;
      steerVelocity = 0;
    }
    else
    {
      steerAmount = Mathf.SmoothDamp(steerAmount, 0, ref steerVelocity, 0.5f);
    }

    steeringWheel.localRotation = Quaternion.Euler(steerAmount * 45, 0, 0);
  }

  private void UpdateAccelerationInput()
  {
    if (Input.GetKeyUp(KeyCode.W))
    {
      if (speedLevel < SpeedLevel.High)
        SetSpeedLevel(speedLevel + 1);
    }
    else if (Input.GetKeyUp(KeyCode.S))
    {
      if (speedLevel > SpeedLevel.Reverse)
        SetSpeedLevel(speedLevel - 1);
    }
  }

  public void SetSpeedLevel(SpeedLevel newLevel)
  {
    // Restore power used by previous speed level.
    VehiclePower.RestorePower(powerUsePerSpeedLevel[(int)speedLevel + 1]);

    speedLevel = newLevel;
    currentAcceleration = accelerationPerSpeedLevel[(int)speedLevel + 1];
    VehiclePower.DrainPower(powerUsePerSpeedLevel[(int)speedLevel + 1]);
    speedLights.SetSpeed(speedLevel);
  }

  private void ApplySteeringAndAcceleration()
  {
    if (speedLevel != SpeedLevel.None)
      vehicle.AddForce(Time.fixedDeltaTime
        * currentAcceleration
        * transform.forward
        * VehiclePower.powerFractionAvailable);

    var forwardVelocity = Vector3.Dot(transform.forward, vehicle.velocity);
    var rotationalVelocity = Vector3.Dot(transform.up, vehicle.angularVelocity);
    
    if (steerAmount != 0 && vehicle.velocity.sqrMagnitude > 0)
    {
      var torqueToApply = Time.fixedDeltaTime
          * steerAmount
          * Mathf.Clamp(vehicle.velocity.magnitude, 0, 0.9f)
          * 380_000f
          * (forwardVelocity > 0 ? 1 : -1)
          * Vector3.up;
      if (rotationalVelocity > 1 && torqueToApply.y > 0) return;
      if (rotationalVelocity < -1 && torqueToApply.y < 0) return;
      vehicle.AddTorque(torqueToApply);
    }
  }
}
