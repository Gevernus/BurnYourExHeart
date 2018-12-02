using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Timers;
using UnityEngine;

public class СircleFriend : Circle {
    private readonly Prefab circleFab = new Prefab("Circle");
    private readonly Prefab circleEnemyFab = new Prefab("CircleEnemy");

    protected void Start() {
        if (transform.parent == null) {
            Vector3 movement = new Vector3(0f, -1f);
            GetComponent<Rigidbody2D>().velocity = movement * speed;
        }

        GetComponent<TrailRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (transform.parent == null) {
            if (other.gameObject.name == "Circle(Clone)") {
                Destroy(gameObject);
                AddCircle(other.transform, false, false);
            }

            if (other.gameObject.name.StartsWith("CircleEnemy")) {
                Destroy(gameObject);
                AddCircle(other.transform, false, true);
            }

            if (other.gameObject.name == "Player") {
                Destroy(gameObject);
                AddCircle(other.transform, true, false);
            }

            if (other.gameObject.name.StartsWith("Enemy")) {
                Destroy(gameObject);
                AddCircle(other.transform, true, true);
            }
        }
        else {
            if (other.gameObject.name.StartsWith("CircleEnemy")) {
                if (!GetComponent<Circle>().undestructable && !other.GetComponent<Circle>().undestructable) {
                    Unjoin(gameObject.transform);
                    Unjoin(other.gameObject.transform);
                }
            }

            if (other.gameObject.name.StartsWith("Enemy")) {
                if (!GetComponent<Circle>().undestructable) {
                    Unjoin(gameObject.transform);
                }
            }
        }
    }

    private void Unjoin(Transform component) {
        component.SetParent(GameObject.Find("Bullets").transform);
        component.gameObject.tag = "Bullet";
        Destroy(component.gameObject.GetComponent<DistanceJoint2D>());
        Destroy(component.gameObject, 2f);
        component.GetComponent<TrailRenderer>().enabled = true;
        for (var i = 0; i < component.childCount; i++) {
            var child = component.GetChild(i);
            Unjoin(child);
        }
    }

    private void AddCircle(Transform parent, bool tail, bool invert) {
        if (!tail || parent.childCount == 0) {
            var circle = invert
                ? circleEnemyFab.Instantiate(parent.transform)
                : circleFab.Instantiate(parent.transform);
            circle.transform.position =
                new Vector3(parent.transform.position.x + 0.2f, parent.transform.position.y + 0.2f);
            var joint = circle.AddComponent<DistanceJoint2D>();
            joint.enableCollision = true;
            joint.connectedBody = parent.GetComponent<Rigidbody2D>();
            if (invert) {
                circle.gameObject.tag = "EnemySphere";
                joint.autoConfigureDistance = false;
                joint.distance = 0.21f;
            }
            else {
                circle.gameObject.tag = "FriendSphere";
            }
        }
        else {
            AddCircle(parent.GetChild(0), true, invert);
        }
    }
}