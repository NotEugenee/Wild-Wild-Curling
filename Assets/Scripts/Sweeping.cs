using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeping : MonoBehaviour
{
    public Stone targetStone;
    public float sweepDrag; // Lower drag value when sweeping
    public float maxDrag = 0.15f; // Maximum drag value
    public float minDrag = 0.02f; // Minimum drag value
    public float normalDrag = 0.2f; // Normal drag value
    public float sweepDuration = 2f; // How long sweeping lasts
    public ParticleSystem sweepEffect; // Particle effect for sweeping

    private float sweepTimer = 0f;
    private bool isSweeping = false;

    public float shakeThreshold = 1.2f; // Threshold for shake detection
    private Vector3 lastRightVel;

    public float frequencyWindow = 1.0f; // Frequency window for shake detection

    private List<float> frequencyData = new List<float>();
    // Update is called once per frame
    void Update()
    {
        bool leftTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.8f;
        bool rightTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.8f;
        if (leftTrigger && rightTrigger && targetStone != null)
        {
           Vector 3 rightVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        
        if ((rightVelocity - lastRightVelocity).magnitude > shakeThreshold)
        {
            Sweeping();
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
    private void Sweeping()
    {
        isSweeping = true;
        sweepTimer = sweepDuration;
        sweepDrag = targetStone.rb.drag * .0.98f;
        targetStone.SetDrag(sweepDrag); // Set lower drag when sweeping
        sweepEffect.Emit(10);// Play the particle effect
    }
    private void StopSweeping()
    {
        isSweeping = false;
        targetStone.SetDrag(normalDrag); // Reset to normal drag
        sweepTimer = 0f;
        sweepEffect.Stop(); // Stop the particle effect
    }
}
