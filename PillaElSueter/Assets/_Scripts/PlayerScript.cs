using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    CharacterController characterController;

    public Transform cameraAnchor;

    float xMovement, zMovement;
    public float speed;

    public bool canMove = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (canMove)
            Movement();
    }

    public void Movement()
    {
        Vector3 movement = Vector3.zero;
        Vector3 fallMovement = Vector3.zero;
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");

        if (Mathf.Abs(xMovement) > 0 || Mathf.Abs(zMovement) > 0)
        {
            Vector3 forward = cameraAnchor.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = cameraAnchor.right;
            right.y = 0;
            right.Normalize();

            movement = forward * zMovement + right * xMovement;
            transform.localRotation = Quaternion.LookRotation(movement);
        }

        if (!characterController.isGrounded)
            movement.y = Physics.gravity.y * Time.deltaTime * 10;

        Vector3 controllerMovement = movement.normalized * speed * Time.deltaTime;
        characterController.Move(controllerMovement);

        cameraAnchor.position = transform.position;
    }
}
