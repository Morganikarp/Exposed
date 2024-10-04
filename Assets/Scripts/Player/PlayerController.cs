using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2d;
    AudioSource audioSource;
    public AudioClip TourchToggle;
    public AudioClip TourchBreak;
    Vector3 MovementVector;
    float Speed = 5f;
    GameObject Flashlight;
    GameObject EnemySpawner;

    public bool hasFlashlight;

    public float objectivesClear;

    public float flashDuration;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Flashlight = transform.GetChild(0).gameObject;
        EnemySpawner = transform.GetChild(1).gameObject;

        Flashlight.SetActive(false);
        objectivesClear = 0;

        flashDuration = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.angularVelocity = 0f;
        MovementVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);

        Vector3 MousePos = new(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Vector3 DirVect = MousePos - transform.position;
        float Angle = Mathf.Atan2(DirVect.y, DirVect.x) * 180 / Mathf.PI;

        transform.eulerAngles = new(0, 0, Angle - 90);

        rb2d.velocity = new Vector3(MovementVector.x * Speed, MovementVector.y * Speed, 0f);

        if (hasFlashlight)
        {

            bool off = false;
            if (Input.GetMouseButton(0) && flashDuration > 0f)
            {
                Flashlight.SetActive(true);

                flashDuration -= Time.deltaTime;

                if (flashDuration < 0f)
                {
                    off = true;
                }

            }

            else if (off)
            {
                return;
            }

            else
            {
                Flashlight.SetActive(false);
                if (flashDuration <= 10)
                {
                    flashDuration += Time.deltaTime;
                }
            }

            if (Input.GetMouseButtonDown(0) && !off)
            {
                audioSource.PlayOneShot(TourchToggle);
            }

            if (Input.GetMouseButtonUp(0))
            {
                audioSource.PlayOneShot(TourchToggle);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Base" && objectivesClear == 3)
        {
            SceneManager.LoadScene(2);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Base")
        {
            BaseRoofController cont = collision.gameObject.GetComponent<BaseRoofController>();
            cont.Inside = true;
            hasFlashlight = false;
            Flashlight.SetActive(false);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Base")
        {
            BaseRoofController cont = collision.gameObject.GetComponent<BaseRoofController>();
            cont.Inside = false;
            hasFlashlight = true;
            EnemySpawner.GetComponent<FishSpawnerController>().SpawnEnemy = true;
        }
    }
}
