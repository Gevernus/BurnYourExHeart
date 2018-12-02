using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEnemy : Circle {

    void Start() {
        GetComponent<TrailRenderer>().enabled = false;
    }
// Update is called once per frame
    protected void FixedUpdate() {
        var parent = GetParent();
        if (parent != null && transform.childCount == 0 && parent.gameObject.name.StartsWith("Enemy")) {
            var heading = transform.position - parent.position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            GetComponent<Rigidbody2D>().velocity = Vector2.Perpendicular(new Vector2(direction.x, direction.y)) * speed;
        }
    }

    private Transform GetParent() {
        Transform parent = transform;
        while (parent.parent != null) {
            parent = parent.parent;
        }

        return parent;
    }
}