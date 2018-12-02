using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SphereCounter : MonoBehaviour {
    public float enemySpheres;
    public float friendSpheres;
    public float loseKoef;
    public GameObject redBar;
    public GameObject blueBar;
    public GameObject world;
    public Sprite heart;
    public Sprite break1;
    public Sprite break2;
    public Button restart;
    public bool gameEnded;

    private float percentage = 0.5f;

    // Use this for initialization
    void Start() {
        restart.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!gameEnded) {
            enemySpheres = GameObject.FindGameObjectsWithTag("EnemySphere").Length;
            friendSpheres = GameObject.FindGameObjectsWithTag("FriendSphere").Length;
            var count = (Mathf.Clamp(enemySpheres, 1, 100) / 5 - Mathf.Clamp(friendSpheres, 1, 100) + loseKoef / 2) /
                        loseKoef;
            percentage = Mathf.Clamp(percentage + (count - percentage) / 10, 0, 1);
            blueBar.transform.localScale = new Vector3(blueBar.transform.localScale.x, 38 * percentage);
            blueBar.transform.localPosition = new Vector3(blueBar.transform.localPosition.x, 4 * (1 - percentage));
            redBar.transform.localScale = new Vector3(redBar.transform.localScale.x, 38 * (1 - percentage));
            redBar.transform.localPosition = new Vector3(redBar.transform.localPosition.x, -4 * percentage);
            if (percentage > .8) {
                if (percentage > .9) {
                    world.GetComponent<SpriteRenderer>().sprite = break2;
                }
                else {
                    world.GetComponent<SpriteRenderer>().sprite = break1;
                }
            }
            else {
                world.GetComponent<SpriteRenderer>().sprite = heart;
            }

            if (Mathf.Abs(percentage) == 1 || Mathf.Abs(percentage) == 0) {
                EndGame(Mathf.Abs(percentage) == 1);
            }
        }
    }

    private void EndGame(bool isLose) {
        gameEnded = true;
        restart.gameObject.SetActive(true);
        restart.transform.GetChild(0).GetComponent<Text>().text = isLose ? "HEART IS BROKEN" : "HEART IS FILLED";
        var ships = GameObject.FindGameObjectsWithTag("Ship");
        foreach (var i in ships) {
            i.GetComponent<Ship>().ReleaseSpheres();
        }

        world.GetComponent<Objects>().StopSpawn();
        GameObject.Find("Player").GetComponent<Movement>().Freeze();
    }

    public void Restart() {
        SceneManager.LoadScene("Main");
        gameEnded = false;
    }
}