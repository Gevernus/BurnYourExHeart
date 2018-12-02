using System.Collections;
using UnityEngine;

public abstract class Circle : MonoBehaviour {
    public float speed;
    public bool undestructable;

    protected void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "World") {
            Destroy(gameObject);
        }
    }
}