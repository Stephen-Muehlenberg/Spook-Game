using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
  [SerializeField] private Rigidbody vehicle;
  [SerializeField] private Transform steeringWheel;
  [SerializeField] private VehicleSpeedLights speedLights;
  [SerializeField] private float[] accelerationPerSpeedLevel;
  [SerializeField] private float steerSpeed = 6f;
  [SerializeField] private Transform particleSpawnPoint;

  private int speedLevel;
  private float currentAcceleration;
  private int MAX_SPEED_LEVEL;
  private float steerAmount;
  private float steerVelocity;

  private void Awake()
  {
    MAX_SPEED_LEVEL = accelerationPerSpeedLevel.Length;
    speedLights.SetSpeed(0);
  }

  private void Update()
  {
    UpdateSteeringInput();
    UpdateAccelerationInput();
  //  LogStats();

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
      if (speedLevel < MAX_SPEED_LEVEL)
      {
        speedLevel++;
        currentAcceleration = accelerationPerSpeedLevel[speedLevel - 1];
        speedLights.SetSpeed(speedLevel);
      }
    }
    else if (Input.GetKeyUp(KeyCode.S))
    {
      if (speedLevel > 0)
      {
        speedLevel--;
        if (speedLevel == 0)
          currentAcceleration = 0;
        else
          currentAcceleration = accelerationPerSpeedLevel[speedLevel - 1];
        speedLights.SetSpeed(speedLevel);
      }
    }
  }

  private void ApplySteeringAndAcceleration()
  {
    if (speedLevel > 0)
      vehicle.AddForce(Time.fixedDeltaTime
        * currentAcceleration
        * transform.forward);

    if (steerAmount != 0 && vehicle.velocity.sqrMagnitude > 0)
      vehicle.AddTorque(Time.fixedDeltaTime
        * steerAmount
        * Mathf.Clamp(vehicle.velocity.magnitude, 0, 1.7f)
        * 200_000f
        * Vector3.up);
  }

  private void LogStats()
  {
    Debug.Log($"{vehicle.velocity.magnitude}m/s");
  }
}
