using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    CharacterController characterController;

    Camera cam;

    float xMovement, zMovement, yaw;
    public float normalSpeed, energySpeed, lateralSpeed;
    public float pickingFastTime, pickingTelephoneTime, pickingTVTime, energyItemTime;
    float movSpeed;

    public float bottomRayHeight, topRayHeight;

    public float addTelephoneTime = 100;

    [HideInInspector] public bool pickingFast = false, pickingTelephone = false, pickingTV = false, hasEnergy = false;

    float pickingTimer = 0;
    float energyTimer = 0;

    private void Start()
    {
        movSpeed = normalSpeed;
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        yaw = transform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        if (!pickingTelephone && !pickingFast && !pickingTV)
        {
            Movement();
            Rotation();

            if(Input.GetButtonDown("Action"))
            {
                Interact();
            }
        }

        else if (pickingFast)
        {
            pickingTimer += Time.deltaTime;

            if(pickingTimer > pickingFastTime)
            {
                pickingTimer = 0;
                pickingFast = false;
                pickingTelephone = false;
                pickingTV = false;
                //anim de agacharse
            }
        }

        else if(pickingTelephone)
        {
            pickingTimer += Time.deltaTime;

            if (pickingTimer > pickingTelephoneTime)
            {
                pickingTimer = 0;
                pickingFast = false;
                pickingTelephone = false;
                pickingTV = false;
                GameManager.Instance.AddCurrentTime(addTelephoneTime); //
                //anim de agacharse
            }
        }
        else if(pickingTV)
        {
            pickingTimer += Time.deltaTime;

            if (pickingTimer > pickingTVTime)
            {
                pickingTimer = 0;
                pickingFast = false;
                pickingTelephone = false;
                pickingTV = false;
                //anim de agacharse
            }
        }
        if(hasEnergy)
        {
            energyTimer += Time.deltaTime;

            if(energyTimer>energyItemTime)
            {
                energyTimer = 0;
                hasEnergy = false;
                movSpeed = normalSpeed;
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
        Ray bottomRay = new Ray(transform.position + Vector3.up * bottomRayHeight, transform.forward);
        Ray topRay = new Ray(transform.position + Vector3.up * topRayHeight, transform.forward);

        RaycastHit hit;

        if(Physics.Raycast(bottomRay, out hit, 3))
        {


            if (hit.collider.tag == "sueter")
            {
                SueterInteraction(hit.collider.gameObject);  
            }

            if (hit.collider.tag == "energy")
            {
                EnergyItemInteraction(hit.collider.gameObject);
            }
        }

        if (Physics.Raycast(topRay, out hit, 3))
        {
            if (hit.collider.tag == "phone" && GameManager.Instance.isPhoneRinging)
            {
                TelephoneInteraction();
            }

            if (hit.collider.tag == "tv" && GameManager.Instance.isTVOn)
            {
                TVInteraction();
            }
        }
    }

    void SueterInteraction(GameObject sueter)
    {
        GameManager.Instance.score += 50; //eso luego se pone con una variable y eso, pero era pa ponerle algo
        GameManager.Instance.AddCurrentTime(2);
        GameManager.Instance.DisableSueter(sueter);
        pickingFast = true;
    }

    void EnergyItemInteraction(GameObject item)
    {
        movSpeed = energySpeed;
        pickingFast = true;
        hasEnergy = true;
        GameManager.Instance.DisableEnergyItem(item);
    }

    void TelephoneInteraction()
    {
        //GameManager.Instance.AddCurrentTime(addTelephoneTime);
        GameManager.Instance.DisableTelephone();
        pickingTelephone = true;
    }

    void TVInteraction()
    {
        GameManager.Instance.DisableTV();
        pickingTV = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.up * bottomRayHeight, transform.position + Vector3.up * bottomRayHeight + (transform.forward * 3));
        Gizmos.DrawLine(transform.position + Vector3.up * topRayHeight, transform.position + Vector3.up * topRayHeight + (transform.forward * 3));
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
