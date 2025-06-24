using UnityEngine;
using System;

public class ClockController : MonoBehaviour
{
    [Header("Clock Hands")]
    public Transform hourHand;
    public Transform minuteHand;

    // The current time displayed by the clock
    private DateTime currentTime;

    // Whether user is currently setting the time
    private bool isSettingTime = false;

    // Which hand is being set (0 = none, 1 = hour, 2 = minute)
    private int handBeingSet = 0;

    // Store the initial mouse position to calculate rotation delta
    private Vector3 lastMousePosition;

    // Speed to convert mouse drag to hand rotation degrees
    private float rotationSpeed = 0.5f;

    void Start()
    {
        // Initialize currentTime to system current time at start
        currentTime = DateTime.Now;

        /*//UpdateHands();*/
    }

    void Update()
    {
        if (!isSettingTime)
        {
            // Update current time by adding delta time
            currentTime = currentTime.AddSeconds(Time.deltaTime);

            UpdateHands();
        }
        else
        {
            HandleUserInput();
        }

        // For demonstration:
        // Press H to start setting hour hand
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartSettingHand(1);
        }

        // Press M to start setting minute hand
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartSettingHand(2);
        }

        // Press Escape or Enter to stop setting time and resume clock
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            StopSettingHand();
        }
    }

    void StartSettingHand(int hand)
    {
        isSettingTime = true;
        handBeingSet = hand;
        // Store mouse position on start
        lastMousePosition = Input.mousePosition;
    }

    void StopSettingHand()
    {
        isSettingTime = false;
        handBeingSet = 0;
    }

    void HandleUserInput()
    {
        if (handBeingSet == 0)
            return;

        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        lastMousePosition = Input.mousePosition;

        float rotationDelta = mouseDelta.x * rotationSpeed;

        if (handBeingSet == 1)
        {
            // Rotate hour hand around Z axis
            hourHand.Rotate(0, 0, -rotationDelta);

            // Update currentTime hour based on hourHand's rotation
            float hourAngle = hourHand.localEulerAngles.z;
            // Because in Unity, rotation around Z is clockwise when angle decreases
            hourAngle = 360 - hourAngle;
            if (hourAngle == 360) hourAngle = 0;

            int hour = Mathf.FloorToInt(hourAngle / 30f) % 12; // 360 / 12 = 30 degrees per hour
            int currentMinute = currentTime.Minute;

            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hour, currentMinute, currentTime.Second);
        }
        else if (handBeingSet == 2)
        {
            // Rotate minute hand around Z axis
            minuteHand.Rotate(0, 0, -rotationDelta);

            float minuteAngle = minuteHand.localEulerAngles.z;
            minuteAngle = 360 - minuteAngle;
            if (minuteAngle == 360) minuteAngle = 0;

            int minute = Mathf.FloorToInt(minuteAngle / 6f) % 60; // 360 / 60 = 6 degrees per minute
            int currentHour = currentTime.Hour;

            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentHour, minute, currentTime.Second);
        }
    }

    void UpdateHands()
    {
        // Calculate the rotation for hands based on currentTime

        // Hour hand rotates 30 degrees per hour + 0.5 degrees per minute
        float hourDegrees = (currentTime.Hour % 12) * 30f + (currentTime.Minute * 0.5f);

        // Minute hand rotates 6 degrees per minute
        float minuteDegrees = currentTime.Minute * 6f;

        // Set rotations (Unity's 2D clock face commonly rotates around Z axis)
        // Subtract from 360 to convert to clockwise rotation direction because default rotation is counterclockwise
        hourHand.localEulerAngles = new Vector3(0, 0, 360f - hourDegrees);
        minuteHand.localEulerAngles = new Vector3(0, 0, 360f - minuteDegrees);
    }
}

