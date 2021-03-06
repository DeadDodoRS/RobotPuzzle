﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveSides { FORWARD, BACKWARD }

public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    private float characterMoveForce;

    [SerializeField] private float rotateSpeed = 0.8f;

    private Rigidbody rbody;
    private BaseCommand currentCommand;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + transform.forward * Time.deltaTime * characterMoveForce);
    }

    public void Move(MoveSides side, BaseCommand command)
    {
        currentCommand = command;

        if (side == MoveSides.FORWARD)
            characterMoveForce = speed;
        else
            characterMoveForce = -speed;
    }

    public void Rotate(TurnArguments side, BaseCommand command)
    {
        currentCommand = command;

        if (side == TurnArguments.Right)
            StartCoroutine(RotateCharacter(Vector3.up * 90, rotateSpeed));
        else
            StartCoroutine(RotateCharacter(Vector3.up * -90, rotateSpeed));
    }

    private IEnumerator RotateCharacter(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }

        transform.rotation = toAngle;
        StopRunning();
    }

    public void StopRunning()
    {
        if (currentCommand is MoveCommand)
            characterMoveForce = 0;

        currentCommand.CommandEnd();
    }
}
