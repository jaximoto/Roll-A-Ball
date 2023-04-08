using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight = 0;
    public float speed = 0;
    public float jumpCooldown = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    

    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int count;
    private bool jumping = false;
    private bool onGround = true;
    private bool hasSecondJump = true;
    private float timeSinceJump = 0;
    private int jumpNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        
        SetCountText();
        winTextObject.SetActive(false);
        timeSinceJump = jumpCooldown;
    }

    void OnMove(InputValue movementValue) 
    {
        
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue value)
    {

        float newValue = value.Get<float>();
        if (newValue == 1.0f)
        {
            jumping = true;
            
            
        }
        else
        {
            jumping = false;
            
        }
        
        //Debug.Log(jumping);
        
        
    }

    void SetCountText() 
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 13) 
        {
            winTextObject.SetActive(true);
        }
    }

    

    void FixedUpdate() 
    {
        // Moving
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        //Jumping
        timeSinceJump += Time.deltaTime;
        Debug.Log("Jumping boolean:" + jumping);
        Debug.Log("onGround boolean:" + onGround);
        if (jumping && onGround && timeSinceJump > jumpCooldown && jumpNumber == 0)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            onGround = false;
            timeSinceJump = 0;
            jumpNumber++;

        }
        else if (jumping && hasSecondJump && timeSinceJump > jumpCooldown && jumpNumber < 1)
        {
            Debug.Log("condition happened");
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            hasSecondJump = false;
            timeSinceJump = 0;
            jumpNumber++;
        }
        if (rb.velocity.y == 0.0f)
        {
            onGround = true;
            hasSecondJump = true;
            jumpNumber = 0;
        }
        Debug.Log(jumpNumber);
    }

    private void OnTriggerEnter(Collider other) 
    {
        
        if (other.gameObject.CompareTag("PickUp"))
        {
            
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
       
        
    }
}
