using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class FishController : MonoBehaviour
{
    GameObject Player;
    PlayerController PlayerCon;
    AudioSource audioSource;
    public AudioClip deathNoise;
    public float Aggression;

    float Speed;
    float Endurance;

    public float currentEndurance;

    bool Flee;
    Vector3 fleePos;
    Vector3 fleeDir;

    bool restart;
    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        Player = GameObject.Find("Player");
        PlayerCon = Player.GetComponent<PlayerController>();

        switch (Aggression)
        {
            case 0:
                Speed = .001f;
                Endurance = 1f;
                break;
            case 1:
                Speed = .002f;
                Endurance = 1.5f;
                break;
            case 2:
                Speed = .004f;
                Endurance = 2f;
                break;
            case 3:
                Speed = .006f;
                Endurance = 4f;
                break;

        }

        currentEndurance = Endurance;
        Flee = false;

        restart = false;
    }
    private void Update()
    {
        if (!Flee) 
        {
            Vector3 vectorToTarget = transform.position - Player.transform.position;
            float angle = -Mathf.Atan2(vectorToTarget.x, vectorToTarget.y) * Mathf.Rad2Deg + 180f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;

            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed);
            Vector3 fleePos = (transform.position - Player.transform.position).normalized * 2;
        }

        else if (Flee)
        {
            Vector3 vectorToTarget = transform.position - Player.transform.position;
            float angle = -Mathf.Atan2(vectorToTarget.x, vectorToTarget.y) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + fleePos, .05f);
            if (transform.position == fleeDir)
            {
                Destroy(gameObject);
            }
        }

        if (restart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            StartCoroutine("Death");
        }

        if (collision.gameObject.tag == "Base")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            currentEndurance -= Time.deltaTime;

            if (currentEndurance < 0f)
            {
                fleeDir = fleePos + transform.position;
                Flee = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            currentEndurance = Endurance;
        }
    }

    IEnumerator Death()
    {
        audioSource.PlayOneShot(deathNoise);
        yield return new WaitForSeconds(5f);
        restart = true;
    }
}
