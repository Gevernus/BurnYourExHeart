using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour {
    private readonly Prefab circleFab = new Prefab("Circle");
    public float delay;
    private bool stopSpawn;

    void Start() {
        stopSpawn = false;
        var coroutine = CreateCircles();
        StartCoroutine(coroutine);
    }

    IEnumerator CreateCircles() {
        for (;;) {
            if (stopSpawn) {
                break;
            }

            var circle = circleFab.Instantiate();
            circle.transform.position = new Vector3(Random.Range(-7.5f, 7.5f), 5);
            yield return new WaitForSeconds(delay);
        }
    }

    public void StopSpawn() {
        stopSpawn = true;
    }
}