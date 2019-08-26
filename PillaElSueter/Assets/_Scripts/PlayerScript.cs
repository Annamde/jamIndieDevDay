using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    CharacterController characterController;

    Camera cam;

    float xMovement, zMovement, yaw;
    public float movSpeed, lateralSpeed;

    public bool canMove = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        yaw = transform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        if (canMove)
        {
            Movement();
            Rotation();
        }
    }

    public void Movement()
    {
        Vector3 movement = Vector3.zero;
        Vector3 fallMovement = Vector3.zero;
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");

        if (Mathf.Abs(xMovement) > 0 || Mathf.Abs(zMovement) > 0)
        {
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = transform.right;
            right.y = 0;
            right.Normalize();

            movement = forward * zMovement + right * xMovement;
        }

        if (!characterController.isGrounded)
            movement.y = Physics.gravity.y * Time.deltaTime * 10;

        Vector3 controllerMovement = movement.normalized * movSpeed * Time.deltaTime;
        characterController.Move(controllerMovement);
    }

    public void Rotation()
    {
        float hRotation = Input.GetAxis("Mouse X");
        yaw += hRotation * lateralSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(transform.localRotation.x, yaw, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="sueter")
        {
            GameManager.score += 50; //eso luego se pone con una variable y eso, pero era pa ponerle algo
            Destroy(other.gameObject);
        }
    }
}
