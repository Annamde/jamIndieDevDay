using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    CharacterController characterController;

    Camera cam;

    float xMovement, zMovement, yaw;
    public float movSpeed, lateralSpeed, pickingTime;

    [HideInInspector] public bool pickingItem = false, pickingSueter = false;

    float pickingTimer = 0;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        yaw = transform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        if (!pickingSueter && !pickingItem)
        {
            Movement();
            Rotation();

            if(Input.GetButtonDown("Action"))
            {
                Interact();
            }
        }

        else
        {
            pickingTimer += Time.deltaTime;

            if(pickingTimer > pickingTime)
            {
                pickingTimer = 0;
                pickingItem = false;
                pickingSueter = false;
                //anim de agacharse
            }
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

    void Interact()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.3f, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 5))
        {
            if (hit.collider.tag == "sueter")
            {
                PickSueter(hit.collider.gameObject);  
            }

            if (hit.collider.tag == "item")
            {
                PickItem(hit.collider.gameObject);
            }
        }
    }

    void PickSueter(GameObject sueter)
    {
        GameManager.score += 50; //eso luego se pone con una variable y eso, pero era pa ponerle algo
        GameManager.Instance.AddCurrentTime(2);
        GameManager.Instance.DisableSueter(sueter);
        pickingSueter = true;
    }

    void PickItem(GameObject item)
    {
        GameManager.Instance.AddCurrentTime(10);
        GameManager.Instance.DisableItem(item);
        pickingItem = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f, transform.position + Vector3.up * 0.3f + (transform.forward * 3));
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag=="sueter")
    //    {
    //        GameManager.score += 50; //eso luego se pone con una variable y eso, pero era pa ponerle algo
    //        GameManager.Instance.AddCurrentTime(2);
    //        GameManager.Instance.DisableSueter(other.gameObject);
    //    }

    //    if(other.tag == "item")
    //    {
    //        GameManager.Instance.AddCurrentTime(10);
    //        GameManager.Instance.DisableItem(other.gameObject);
    //    }
    //}
}
