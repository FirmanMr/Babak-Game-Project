using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;
    private bool isTurboActive; // Menandakan apakah turbo/nos aktif atau tidak

    // Pengaturan
    [SerializeField] private float motorForce, breakForce, maxSteerAngle, turboForce;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Roda
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        // Input untuk mengendalikan kemudi
        horizontalInput = SimpleInput.GetAxis("Horizontal");

        // Input untuk mengendalikan percepatan
        verticalInput = SimpleInput.GetAxis("Vertical");

        // Input untuk pengereman
        isBreaking = Input.GetKey(KeyCode.Space);

        // Input untuk mengaktifkan/menonaktifkan turbo/nos
        isTurboActive = Input.GetKey(KeyCode.LeftShift); // Misalkan turbo diaktifkan dengan tombol Shift kiri.
    }

    private void HandleMotor()
    {
        // Mengatur gaya motor pada roda depan untuk menggerakkan kendaraan
        float currentMotorForce = isTurboActive ? motorForce + turboForce : motorForce; // Menggunakan turbo jika aktif
        frontLeftWheelCollider.motorTorque = verticalInput * currentMotorForce;
        frontRightWheelCollider.motorTorque = verticalInput * currentMotorForce;

        // Mengatur gaya pengereman
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        // Mengatur gaya pengereman pada masing-masing roda
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        // Mengatur sudut kemudi sesuai dengan input horizontal
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        // Memperbarui posisi dan rotasi roda visual agar sesuai dengan roda fisik
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        // Mendapatkan posisi dan rotasi roda fisik
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        // Mengatur posisi dan rotasi roda visual sesuai dengan roda fisik
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void ActivateTurbo(bool isActive)
    {
        isTurboActive = isActive;

        Debug.Log("Turbo is " + (isActive ? "active" : "inactive"));
        // Logika tambahan jika ada
    }

}
