using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1f;
    [SerializeField] AudioClip mainEngine;

    Rigidbody rigitBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    // Use this for initialization
    void Start () {
        rigitBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstScene", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoaFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigitBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
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
        rigitBody.freezeRotation = true; // take control of the rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigitBody.freezeRotation = false; // resume physical of the rotation
    }
}
