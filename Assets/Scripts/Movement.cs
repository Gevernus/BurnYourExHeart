using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, yMin, yMax;
}

public class Movement : MonoBehaviour {
    public float baseSpeed;
    public float tilt;
    public Boundary boundary;
    private bool isFreeze;
    private readonly Prefab trace = new Prefab("Trace");

    void FixedUpdate() {
        if (!isFreeze) {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            if (Input.GetKeyDown(KeyCode.Space)) {
                var child = GetLast();
                if (child != transform.GetChild(0)) {
                    child.SetParent(GameObject.Find("Bullets").transform);
                    child.gameObject.tag = "Bullet";
                    child.GetComponent<TrailRenderer>().enabled = true;
                    Destroy(child.gameObject.GetComponent<DistanceJoint2D>());
                    Destroy(child.gameObject, 2f);
                }
            }

            float speed = baseSpeed;
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, moveVertical);
            rigidbody.velocity = movement * speed;

            rigidbody.position = new Vector3
            (
                Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
                Mathf.Clamp(rigidbody.position.y, boundary.yMin, boundary.yMax)
            );

            rigidbody.rotation = rigidbody.velocity.x * -tilt;
        }
    }

    private Transform GetLast() {
        var child = transform;
        while (child.childCount > 0) {
            child = child.GetChild(0);
        }

        return child;
    }

    public void Freeze() {
        isFreeze = true;
    }
}