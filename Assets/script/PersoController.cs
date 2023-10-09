using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersoController : MonoBehaviour {

    Animator persoAnimator;
    AudioSource persoAudioSource;
    CapsuleCollider persoCapsule;

    float axisH, axisV;

    [SerializeField]
    float walkSpeed = 2f, runSpeed = 8f, rotSpeed = 100f, jumpForce = 350;

    Rigidbody rb;

    const float timeout = 60.0f;
    [SerializeField] float countdown = timeout;

   
    [SerializeField] AudioClip sndJump, sndImpact, sndLeftFoot, sndRightFoot;
    bool switchFoot = false;

    [SerializeField] bool isJumping = false;

    private void Awake()
    {
        persoAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        persoAudioSource = GetComponent<AudioSource>();
        persoCapsule = GetComponent<CapsuleCollider>();
    }

    void Update () {

        axisH = Input.GetAxis("Horizontal");
        axisV = Input.GetAxis("Vertical");

        if(axisV>0)
        {
            if(Input.GetKey(KeyCode.LeftControl))
            {
                transform.Translate(Vector3.forward * runSpeed * axisV * Time.deltaTime);
                persoAnimator.SetFloat("run", axisV);
            }
            else
            {
                transform.Translate(Vector3.forward * walkSpeed * axisV * Time.deltaTime);
                persoAnimator.SetBool("walk", true);
                persoAnimator.SetFloat("run", 0);
            }            
        }
        else
        {
            persoAnimator.SetBool("walk", false);
        }

        if (axisH != 0 && axisV == 0)
        {
            persoAnimator.SetFloat("h", axisH);
        }
        else
        {
            persoAnimator.SetFloat("h", 0);
        }


        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * axisH);

        if(axisV < 0)
        {
            persoAnimator.SetBool("walkBack", true);
            persoAnimator.SetFloat("run", 0);
            transform.Translate(Vector3.forward * walkSpeed * axisV * Time.deltaTime);
        }
        else
        {
            persoAnimator.SetBool("walkBack", false);            

        }

        //Idle Dance Twerk

        if(axisH==0 && axisV==0)
        {
            countdown -= Time.deltaTime;

            if(countdown<=0)
            {
                persoAnimator.SetBool("dance", true);
                transform.Find("AudioDance").GetComponent<AudioSource>().enabled = true;
            }
        }
        else
        {
            countdown = timeout;
            persoAnimator.SetBool("dance", false);
            transform.Find("AudioDance").GetComponent<AudioSource>().enabled = false;
        }

        //Debug Dead 

        if(Input.GetKeyDown(KeyCode.AltGr))
        {
            PersoDead();
        }

        //curve de saut
        if(isJumping)
        persoCapsule.height = persoAnimator.GetFloat("colheight");
              
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce);
            persoAudioSource.pitch = 1f;
            persoAnimator.SetTrigger("jump");
            persoAudioSource.PlayOneShot(sndJump);
        }
    }

    public void PersoDead()
    {
        persoAnimator.SetTrigger("dead");
        GetComponent<PersoController>().enabled = false;

    }

    public void PlaySoundImpact()
    {
        persoAudioSource.pitch = 1f;
        persoAudioSource.PlayOneShot(sndImpact);
    }

    public void PlayFootStep()
    {
        if(!persoAudioSource.isPlaying)
        {
            switchFoot = !switchFoot;

            if(switchFoot)
            {
                persoAudioSource.pitch = 2f;
                persoAudioSource.PlayOneShot(sndLeftFoot);
            }
            else
            {
                persoAudioSource.pitch = 2f;
                persoAudioSource.PlayOneShot(sndRightFoot);
            }
        }
    }

    public void SwitchIsJumping()
    {
        isJumping = false;
    }
}
