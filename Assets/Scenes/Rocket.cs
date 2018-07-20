using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource;
	
    // Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();	
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            default:
                print("Ooops");
                break;
        }
    }

    private void Thrust()
    {
        float thrustThisFrame = Time.deltaTime * mainThrust;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.freezeRotation = false;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!audioSource.isPlaying)      // so it doesn´t layer
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;  // take manual control of rotation

        float rotationThisFrame = Time.deltaTime * rcsThrust;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;  // let physics resume control
    }

}
