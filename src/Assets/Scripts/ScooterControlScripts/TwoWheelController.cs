using System;
using UnityEngine;

//Adapted from Unity's old Standard Asset for Vehicles

internal enum DriveType
{
    FrontWheelDrive,
    RearWheelDrive,
    AllWheelDrive
}
    
public class TwoWheelController : MonoBehaviour
{
    [SerializeField] private DriveType m_CarDriveType = DriveType.AllWheelDrive;
    [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[2];
    [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[2];
    //[SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[2];
    [SerializeField] private Vector3 m_CentreOfMassOffset;
    [SerializeField] private float m_MaximumSteerAngle;
    [Range(0, 1)] [SerializeField] private float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
    [Range(0, 1)] [SerializeField] private float m_TractionControl; // 0 is no traction control, 1 is full interference
    [SerializeField] private float m_AccelerationTorqueOverAllWheels;
    [SerializeField] private float m_ReverseTorque;
    [SerializeField] private float m_MaxHandbrakeTorque;
    [SerializeField] private float m_Downforce = 100f;
    [SerializeField] private float m_Topspeed = 200;
    [SerializeField] private static int NoOfGears = 5;
    [SerializeField] private float m_RevRangeBoundary = 1f;
    [SerializeField] private float m_SlipLimit;
    [SerializeField] private float m_DecelerationTorque;
    [SerializeField] private float m_SpeedStabilizationDelta;

    private Quaternion[] m_WheelMeshLocalRotations;
    private Vector3 m_Prevpos, m_Pos;
    private float m_SteerAngle;
    private float m_targetSpeed = 0;
    private int m_GearNum;
    private float m_GearFactor;
    private float m_OldRotation;
    private float m_CurrentTorque;
    private Rigidbody m_Rigidbody;
    private const float k_ReversingThreshold = 0.01f;

    public bool Skidding { get; private set; }
    public float DriveInput { get; private set; }
    public float CurrentSteerAngle{ get { return m_SteerAngle; }}
    public float CurrentSpeed{ get { return m_Rigidbody.velocity.magnitude; }}
    public float MaxSpeed{get { return m_Topspeed; }}
    public float Revs { get; private set; }

    // Use this for initialization
    private void Start()
    {
        m_WheelMeshLocalRotations = new Quaternion[2];
        for (int i = 0; i < 2; i++)
        {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }
        m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

        m_MaxHandbrakeTorque = float.MaxValue;

        m_Rigidbody = GetComponent<Rigidbody>();
        m_CurrentTorque = m_AccelerationTorqueOverAllWheels - (m_TractionControl*m_AccelerationTorqueOverAllWheels);
    }

     
    // Main function that makes scooter move
    public void Move(float steering, float drive, float speedAdjustment, float handbrake)
    {
        // Rotate and turn wheel meshes --> only cosmetic!
        for (int i = 0; i < 2; i++)
        {
            Quaternion quat;
            Vector3 position;
            m_WheelColliders[i].GetWorldPose(out position, out quat);
            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = quat * Quaternion.Euler(0, -90, 0);
        }
        

        // Clamp input values to valid ranges
        steering = Mathf.Clamp(steering, -1, 1);
        DriveInput = drive = Mathf.Clamp(drive, -1, 1);
        handbrake = Mathf.Clamp(handbrake, 0, 1);

        //Set the steer on the front wheel
        //Assuming that wheel 0 is the front wheel (Scooter only steers with front wheel
        float speedPenaltySteering = 0.4f + 0.6f * (m_Topspeed - CurrentSpeed) / m_Topspeed;
        m_SteerAngle = steering*m_MaximumSteerAngle * speedPenaltySteering;
        m_WheelColliders[0].steerAngle = m_SteerAngle;

        /*
        //Code for manual leaning
        float angleY = transform.eulerAngles.y;
        float turnVelocity = 0;
        //Rotate in direction
        if (CurrentSpeed > 0.5f)
        {
            float targetAngleY = Mathf.Atan2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.z) * Mathf.Rad2Deg;
            angleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnVelocity, 0.2f);
            float targetAngleZ = -20 * Vector3.Dot(transform.right, m_Rigidbody.velocity); //* ((MaxSpeed * 1.25f - CurrentSpeed) / (MaxSpeed * 1.25f));
            transform.rotation = Quaternion.Euler(0f, angleY, targetAngleZ);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }
        */
        

        //Helps with reducing sudden direction changes
        SteerHelper(); 
        //Main forward drive
        ApplyDrive(drive, speedAdjustment, handbrake);
        //Limits top speed
        CapSpeed();
        //Stabilizes bike in upwards direction
        Stabilizer(steering);

        //Applies the handbrake.
        //Assuming that wheel 1 is the rear wheel.
        if (handbrake > 0f)
        {
            var hbTorque = handbrake*m_MaxHandbrakeTorque;
            m_WheelColliders[0].brakeTorque = hbTorque;
            m_WheelColliders[1].brakeTorque = hbTorque;
            //Debug.Log("HB" + "Target: " + m_targetSpeed + ", Current: " + CurrentSpeed + "; Motor: " + m_WheelColliders[0].motorTorque + ", Brake:" + m_WheelColliders[0].brakeTorque);
        }

        //Traction calculations
        AddDownForce();
        TractionControl(); //Reduce torque to prevent slipping -> depends on m_TractionControl

        //Retrospective calculations involving gears, seem to have no immediate relevance for driving behavior
        CalculateRevs();
        GearChanging();

        //CheckForWheelSpin(); currently no effects desired
    }

    // Key method that controls forward/backward drive force
    private void ApplyDrive(float drive, float SpeedAdjustment, float handbrake)
    {
        if (drive > 0.5f) 
        {
            m_targetSpeed += 2 * SpeedAdjustment;
            m_targetSpeed = Mathf.Clamp(m_targetSpeed, 0f, m_Topspeed);

            if (SpeedsClose(m_targetSpeed, CurrentSpeed, m_SpeedStabilizationDelta) & handbrake < float.Epsilon)
            {
                m_Rigidbody.velocity = m_targetSpeed * m_Rigidbody.velocity.normalized;
            }
            else if (CurrentSpeed < m_targetSpeed) //Accelerate
            {
                switch (m_CarDriveType)
                {
                    case DriveType.AllWheelDrive:
                        for (int i = 0; i < 2; i++)
                        {
                            m_WheelColliders[i].brakeTorque = 0f;
                            m_WheelColliders[i].motorTorque = m_CurrentTorque / 2f;
                        }
                        break;

                    case DriveType.FrontWheelDrive:
                        m_WheelColliders[0].brakeTorque = 0f;
                        m_WheelColliders[0].motorTorque = m_CurrentTorque;
                        break;

                    case DriveType.RearWheelDrive:
                        m_WheelColliders[1].brakeTorque = 0f;
                        m_WheelColliders[1].motorTorque = m_CurrentTorque;
                        break;
                }
            }
            else //Decelerate
            {
                for (int i = 0; i < 2; i++)
                {
                    m_WheelColliders[i].motorTorque = 0f;
                    m_WheelColliders[i].brakeTorque = m_DecelerationTorque;
                }
            }

        }
        else
        {
            m_targetSpeed = CurrentSpeed;
            for (int i = 0; i < 2; i++)
            {
                m_WheelColliders[i].motorTorque = 0f;
                m_WheelColliders[i].brakeTorque = 0f;
            }
            
        }

        //Reverse (possible only if currently standing)
        if (CurrentSpeed <= k_ReversingThreshold & drive < -0.5f)
        {
            Debug.Log("Reversing");
            for (int i = 0; i < 2; i++)
            {
                m_WheelColliders[i].brakeTorque = 0f;
                m_WheelColliders[i].motorTorque = -m_ReverseTorque;
            }
        }

        //Debug.Log("Target: " + m_targetSpeed + ", Current: " + CurrentSpeed + "; Motor: " + m_WheelColliders[0].motorTorque + ", Brake:" + m_WheelColliders[0].brakeTorque);
    }

    void Stabilizer(float h)
    {
        var deltaQuat = Quaternion.FromToRotation(m_Rigidbody.transform.up, Vector3.up);
        deltaQuat.ToAngleAxis(out var angle, out var axis);
        m_Rigidbody.AddTorque(-m_Rigidbody.angularVelocity * 1f, ForceMode.Acceleration);
        angle = Mathf.LerpAngle(0, angle, Time.fixedDeltaTime);
        m_Rigidbody.AddTorque(axis.normalized * angle * 1f, ForceMode.Acceleration);
    }


    //Actively limits speed to scooters topspeed
    private void CapSpeed()
    {
        float speed = m_Rigidbody.velocity.magnitude;

        if (speed > m_Topspeed)
        { 
            m_Rigidbody.velocity = m_Topspeed * m_Rigidbody.velocity.normalized;
        }
    }

    //Helps to avoid sharp direction changes, not as important
    private void SteerHelper()
    {
        for (int i = 0; i < 2; i++)
        {
            WheelHit wheelhit;
            m_WheelColliders[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; // wheels arent on the ground so dont realign the rigidbody velocity
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
        {
            var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
        }
        m_OldRotation = transform.eulerAngles.y;
    }


    // this is used to add more grip in relation to speed
    private void AddDownForce()
    {
        m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up*m_Downforce*
                                                        m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
    }

    // crude traction control that reduces the power to wheel if the car is wheel spinning too much
    private void TractionControl()
    {
        WheelHit wheelHit;
        switch (m_CarDriveType)
        {
            case DriveType.AllWheelDrive:
                // loop through all wheels
                for (int i = 0; i < 2; i++)
                {
                    m_WheelColliders[i].GetGroundHit(out wheelHit);

                    AdjustTorque(wheelHit.forwardSlip);
                }
                break;

            case DriveType.RearWheelDrive:
                m_WheelColliders[1].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;

            case DriveType.FrontWheelDrive:
                m_WheelColliders[0].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;
        }
    }

    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
        {
            m_CurrentTorque -= 10 * m_TractionControl;
        }
        else
        {
            m_CurrentTorque += 10 * m_TractionControl;
            if (m_CurrentTorque > m_AccelerationTorqueOverAllWheels)
            {
                m_CurrentTorque = m_AccelerationTorqueOverAllWheels;
            }
        }
    }



    private void GearChanging()
    {
        float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
        float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
        float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

        if (m_GearNum > 0 && f < downgearlimit)
        {
            m_GearNum--;
        }

        if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
        {
            m_GearNum++;
        }
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }

    private bool SpeedsClose(float currentSpeed, float targetSpeed, float delta)
    {
        float diff = Mathf.Abs(currentSpeed - targetSpeed);
        return diff < delta;
    }


    private void CalculateGearFactor()
    {
        float f = (1 / (float)NoOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum / (float)NoOfGears;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }


    //For effects only (particles, sounds, skidmarks
    /*
    // checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
    private void CheckForWheelSpin()
    {
        // loop through all wheels
        for (int i = 0; i < 2; i++)
        {
            WheelHit wheelHit;
            m_WheelColliders[i].GetGroundHit(out wheelHit);

            // is the tire slipping above the given threshhold
            if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
            {
                m_WheelEffects[i].EmitTyreSmoke();

                // avoiding all four tires screeching at the same time
                // if they do it can lead to some strange audio artefacts
                if (!AnySkidSoundPlaying())
                {
                    m_WheelEffects[i].PlayAudio();
                }
                continue;
            }

            // if it wasnt slipping stop all the audio
            if (m_WheelEffects[i].PlayingAudio)
            {
                m_WheelEffects[i].StopAudio();
            }
            // end the trail generation
            m_WheelEffects[i].EndSkidTrail();
        }
    }
    */

    /*
    private bool AnySkidSoundPlaying()
    {
        for (int i = 0; i < 2; i++)
        {
            if (m_WheelEffects[i].PlayingAudio)
            {
                return true;
            }
        }
        return false;
    }
    */

}
