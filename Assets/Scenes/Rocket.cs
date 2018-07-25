using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
    
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;


    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // todo: somewhere stop sound on death
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();

        }
    }

    void RespondToThrustInput()
    {
        Thrust();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }  // ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(success);
                Invoke("LoadNextLevel", 1f);  // parameterize time
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                Invoke("LoadFirstLevel", 1f);  // parameterize time
                break;
        }
    }

    private void LoadNextLevel() // todo: allow for more than 2 levels
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = Time.deltaTime * mainThrust;

        rigidBody.freezeRotation = false;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)      // so it doesn´t layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
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