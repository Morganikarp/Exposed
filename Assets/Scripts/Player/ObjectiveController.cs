using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveController : MonoBehaviour
{

    public bool Activated;

    public Sprite[] Sprites;
    SpriteRenderer Renderer;
    private void Start()
    {
        Activated = false;
        Renderer = GetComponent<SpriteRenderer>();

    }
    // Update is called once per frame
    void Update()
    {
        if (!Activated)
        {
            Renderer.sprite = Sprites[0];
        }
        else
        {
            Renderer.sprite = Sprites[1];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!Activated)
            {
                PlayerController playerController = collision.transform.GetComponent<PlayerController>();
                playerController.objectivesClear += 1;
                FishSpawnerController fishSpawnCont = collision.transform.GetChild(1).transform.GetComponent<FishSpawnerController>();
                fishSpawnCont.threatLevel += 1;
            }
            Activated = true;
        }
    }
}
