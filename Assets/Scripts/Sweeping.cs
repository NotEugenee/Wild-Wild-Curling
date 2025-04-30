using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Sweeping : MonoBehaviour
{
    public Stone targetStone;
    public float sweepDrag; // Lower drag value when sweeping
    public float maxDrag = 0.15f; // Maximum drag value
    public float minDrag = 0.02f; // Minimum drag value
    public float normalDrag = 0.2f; // Normal drag value
    public float sweepDuration = 2f; // How long sweeping lasts
    public ParticleSystem sweepEffect; // Particle effect for sweeping
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public InputActionReference velocityActionRight;
    private float sweepTimer = 0f;
    private bool isSweeping = false;

    public float shakeThreshold = 1.2f; // Threshold for shake detection
    private Vector3 lastRightVelocity;

    public float frequencyWindow = 1.0f; // Frequency window for shake detection

    private List<float> frequencyData = new List<float>();

    public AudioSource sweepAudioSource;
    public AudioClip sweepClip; // Audio clip for sweeping sound


    void Update()
    {
        bool leftTrigger = leftController.selectAction.action.ReadValue<float>() > 0.8f;
        bool rightTrigger = rightController.selectAction.action.ReadValue<float>() > 0.8f;
        Vector3 rightVelocity = velocityActionRight.action.ReadValue<Vector3>();
        if (leftTrigger && rightTrigger && targetStone != null)
        {
        if ((rightVelocity - lastRightVelocity).magnitude > shakeThreshold)
        {
            Sweep();
        }
        lastRightVelocity = rightVelocity;
    }
    else
        {
            lastRightVelocity = Vector3.zero;
        }
        if (isSweeping)
        {
            sweepTimer -= Time.deltaTime;
            if (sweepTimer <= 0)
            {
                StopSweeping();
            }
    }
    }
    private void Sweep()
    {
        isSweeping = true;
        sweepTimer = sweepDuration;
        sweepDrag = targetStone.rb.drag * 0.98f;
        targetStone.SetDrag(sweepDrag); // Set lower drag when sweeping
        sweepEffect.Emit(10);// Play the particle effect
        sweepAudioSource.PlayOneShot(sweepClip); // Play the sweeping sound
    }
    private void StopSweeping()
    {
        isSweeping = false;
        targetStone.SetDrag(normalDrag); // Reset to normal drag
        sweepTimer = 0f;
        sweepEffect.Stop(); // Stop the particle effect
    }

}
