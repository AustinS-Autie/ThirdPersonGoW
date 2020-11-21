using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField]
    float moveSpeed = 3.0f;

    [Header("References")]
    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    BoxCollider swordCollider;

    Rigidbody rb;
    Animator anim;

    bool startedCombo = false;
    float timeSinceButtonPressed = 0.0f;

    //new variables
    [SerializeField]
    GameObject axeObj;
    int axeThrow = 0;
    bool axeInHand = true;
    bool startedCombo2 = false;
    Transform axeMoveTo;

    float animTiming = 0;
    int rotVal = 0;

    Vector3 initialAxePos;
    Quaternion initialAxeRot;
    bool axeCol;
    float direction;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        axeMoveTo = axeObj.transform.parent;
        initialAxeRot = Quaternion.Euler(axeObj.transform.rotation.x - 15,
            axeObj.transform.rotation.y + 90, axeObj.transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        direction = 0;
        
        int mirror = 1;

        if (v < 0)
        {
            mirror = -1;
            direction += 180;
        }

        direction += h * 90 * mirror;



        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (v * moveSpeed * Time.deltaTime));

        var camForward = mainCamera.forward;
        var camRight = mainCamera.right;

        camForward.y = 0;
        camForward.Normalize();
        camRight.y = 0;
        camRight.Normalize();

        var moveDirection = (camForward * v * moveSpeed) + (camRight * h * moveSpeed);


        //transform.LookAt(transform.position + camForward);

        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        anim.SetFloat("moveSpeed", Mathf.Abs(moveDirection.magnitude));
        transform.rotation = Quaternion.Euler(mainCamera.rotation.x,direction,mainCamera.rotation.z);
        rb.rotation = transform.rotation;

        if (startedCombo==false)
            ThrowAxe();

        if (axeThrow==0)
            SwordComboFunction();
            


    }

    public void ThrowAxe()
    {

        axeCol = axeObj.GetComponent<SwordPhysics>().isColliding;


        if (animTiming > 0 && !axeCol)
            animTiming -= 1 * Time.deltaTime;




        if (axeThrow == 0)
        {
            axeObj.transform.localRotation = Quaternion.Euler(
                initialAxeRot.x + 70,
                initialAxeRot.y,
                initialAxeRot.z);
        }


        if (Input.GetMouseButtonDown(0) && axeThrow == 0)
        {
            Debug.Log("Axe 0");
            anim.SetTrigger("axeThrow");
            anim.speed = 1;
            axeThrow = 1;
            startedCombo2 = true;
            axeInHand = false;

            
            initialAxePos = new Vector3(axeObj.transform.position.x,
                axeObj.transform.position.y, axeObj.transform.position.z);

            initialAxeRot = Quaternion.Euler(axeObj.transform.rotation.x,
                axeObj.transform.rotation.y, axeObj.transform.rotation.z);
            

        }

        if (axeThrow == 2)
        {
            if (!axeCol)
            {
                if (direction == 0)
                {
                    axeObj.transform.position = Vector3.Lerp(
                    axeObj.transform.position,
                    axeObj.transform.position + new Vector3(
                    0, 0.5f, 40), 0.0075f);
                }
                else
                {
                    axeObj.transform.position = Vector3.Lerp(
                    axeObj.transform.position,
                    axeObj.transform.position + new Vector3(
                    Input.GetAxis("Horizontal") * 40, 0.5f, Input.GetAxis("Vertical") * 40), 0.0075f);
                }
                
                TurnOnSwordCollider();
            }
            else
            {
                TurnOffSwordCollider();
            }

            //Debug.Log("AxeAttack");

        }

        if (axeThrow == 2 && (Input.GetMouseButtonUp(0) || animTiming <= 0))
        {
            if (animTiming <= 0)
                anim.SetTrigger("axeReturn");
            axeThrow = 3;
            //move axe back
            Debug.Log("Axe 3");
        }



        if (axeThrow == 2)
        {

            axeObj.transform.rotation = Quaternion.Euler(
                (axeMoveTo.rotation.x * -1) + 90,
                (axeMoveTo.rotation.y * -1) + rotVal, 
                axeMoveTo.rotation.z * -1);


            if (!axeCol)
            {
                rotVal += 12;
            }
        }

        if(axeThrow==3 && Vector3.Distance(axeObj.transform.position, axeMoveTo.position) > 1f)
            {
                axeThrow = 4;
            Debug.Log("Axe 4");
        }

        if (axeThrow == 4)
        {
            axeObj.GetComponent<SwordPhysics>().ResetTargetMove();
            
            if (Vector3.Distance(axeObj.transform.position, axeMoveTo.position) > 1f)
            {
                if (!swordCollider.enabled)
                {
                    TurnOnSwordCollider();
                    axeObj.GetComponent<SwordPhysics>().SetColliding(false);
                }
                axeObj.transform.position = Vector3.Lerp(axeObj.transform.position, axeMoveTo.position, 0.125f);

                axeObj.transform.rotation = Quaternion.Euler(
                axeMoveTo.rotation.x + 90,
                axeMoveTo.rotation.y + rotVal,
                axeMoveTo.rotation.z);

                rotVal += 12;
                //Debug.Log(Vector3.Distance(axeObj.transform.position, axeMoveTo.position) );
            }
            else
            {
                axeObj.transform.parent = axeMoveTo;
                axeThrow = 5;
                Debug.Log("Axe 5");
            }

        }

        if (axeThrow == 5)
        {
            Debug.Log("Axe Reset");
            anim.speed = 1;
            anim.SetTrigger("stopCombo2");
            axeThrow = 0;
            startedCombo2 = false;
            axeInHand = true;
            TurnOffSwordCollider();
            animTiming = 0;

            axeObj.transform.position = axeMoveTo.position +
            (axeMoveTo.transform.right / 15) + (axeMoveTo.transform.up / -30) +
            (axeMoveTo.transform.forward / 20);

            axeObj.GetComponent<SwordPhysics>().isColliding = false;


            //reset everything
        }



        //if (axeThrow > 0)
        //    transform.position = new Vector3(transform.position.x, 0, transform.position.z);

    }

    public void TossAxe()
    {
        if (axeThrow == 1)
        {
            axeObj.transform.position = new Vector3(axeObj.transform.position.x,
                1, axeObj.transform.position.z+1f);

            axeObj.transform.rotation = Quaternion.Euler(
                transform.rotation.x,
                axeObj.transform.rotation.y, transform.rotation.z);

            axeObj.transform.parent = null;
            axeThrow = 2;
            animTiming = 1.25f;
        }
    }
    /*
    public void AxeClear()
    {

        if (animTiming > 0)
            animTiming -= 1;

        if (axeThrow == 0)
        {
            axeObj.transform.position = axeMoveTo.position + (axeMoveTo.transform.right / 15) + (axeMoveTo.transform.up / -30) + (axeMoveTo.transform.forward / 20);
        }

        if (Input.GetButtonDown("Fire1") && axeThrow == 0)
        {
            anim.SetTrigger("axeThrow");
            axeThrow = 1;
            startedCombo2 = true;
            axeInHand = false;
            animTiming = 150;
        }

        if ( (axeThrow == 1 && Input.GetButtonUp("Fire1") ) || animTiming == 0)
        {
            if (animTiming > 0)
            {
                axeThrow = 3;
                return;
            }
            anim.SetTrigger("axeReturn");
            axeThrow = 2;
            //move axe back
        }

        if (axeThrow == 1)
        {
            axeObj.transform.position += transform.forward / 16;
            TurnOnSwordCollider();

        }

        if (axeThrow == 2)
        {
            if (Vector3.Distance(axeObj.transform.position, axeMoveTo.position) > 0.25f)
            {
                axeMoveTo = axeObj.transform.parent;
                axeObj.transform.position = Vector3.Lerp(axeObj.transform.position, axeMoveTo.position, 0.25f);
                Debug.Log(Vector3.Distance(axeObj.transform.position, axeMoveTo.position));
            }
            else
            {
                anim.speed = 1;
                axeThrow = 3;
                Debug.Log("Axe3");
            }

        }

        if (axeThrow == 3)
        {
            anim.speed = 1;
            anim.SetTrigger("stopCombo2");
            axeThrow = 0;
            startedCombo2 = false;
            axeInHand = true;
            TurnOffSwordCollider();
            animTiming = 0;
            //reset everything
            attackInput = 0;
        }


        if (axeThrow > 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

    }
    */

    public void HoldAxePosition()
    {
        anim.speed = 0;
    }

    public void SwordComboFunction()
    {
        if (Input.GetButtonDown("Jump") && !startedCombo)
        {
            anim.SetTrigger("swordCombo");
            startedCombo = true;
        }

        if (Input.GetButtonDown("Jump") && startedCombo)
        {
            timeSinceButtonPressed = 0;
        }

        timeSinceButtonPressed += Time.deltaTime;
    }

    public void PotentialComboEnd()
    {
        TurnOffSwordCollider();

        if (timeSinceButtonPressed < 0.5f)
            return;

        anim.SetTrigger("stopCombo");
        startedCombo = false;
        timeSinceButtonPressed = 0;

    }

    public void EndOfCombo()
    {
        startedCombo = false;
        timeSinceButtonPressed = 0;
        TurnOffSwordCollider();
    }

    public void TurnOnSwordCollider()
    {
        swordCollider.enabled = true;
    }

    public void TurnOffSwordCollider()
    {
        swordCollider.enabled = false;
    }

    public bool GetAxeCol()
    {
        return axeCol;
    }

}
