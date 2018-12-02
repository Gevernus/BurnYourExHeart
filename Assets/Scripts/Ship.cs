using System.Collections;
using UnityEngine;

public abstract class Ship : MonoBehaviour {
    public float createSphereDelay;
    private int sphereCount;
    private bool stopSpawn;

    protected void Start() {
        stopSpawn = false;
        sphereCount = GetSphereInitCount();
        StartCoroutine(CreateCircles());
    }

    IEnumerator CreateCircles() {
        for (;;) {
            if (stopSpawn) {
                break;
            }

            Transform child = transform;
            for (var i = 0; i <= sphereCount; i++) {
                if (child.childCount == 0) {
                    AddCircle(child);
                    yield return new WaitForSeconds(createSphereDelay);
                }
                else {
                    child = child.GetChild(0);
                }
            }

            yield return new WaitForSeconds(createSphereDelay);
        }
    }

    public void ReleaseSpheres() {
        stopSpawn = true;
        Transform child = transform;
        while (child.childCount > 0) {
            child = child.GetChild(0);
            child.SetParent(GameObject.Find("Bullets").transform);
            child.gameObject.tag = "Bullet";
            Destroy(child.gameObject.GetComponent<DistanceJoint2D>());
            child.GetComponent<TrailRenderer>().enabled = true;
        }
    }

    private void AddCircle(Transform parent) {
        if (parent.childCount == 0) {
            var circle = GetSphereFab().Instantiate(parent.transform);
            circle.transform.position =
                new Vector3(parent.transform.position.x + 0.2f, parent.transform.position.y + 0.2f);
            var joint = circle.AddComponent<DistanceJoint2D>();
            joint.enableCollision = true;
            joint.connectedBody = parent.GetComponent<Rigidbody2D>();
            if (!IsAutoDistance()) {
                circle.gameObject.tag = "EnemySphere";
                joint.autoConfigureDistance = false;
                joint.distance = 0.21f;
            }
            else {
                circle.gameObject.tag = "FriendSphere";
            }
        }
        else {
            AddCircle(parent.GetChild(0));
        }
    }

    protected abstract Prefab GetSphereFab();

    protected abstract int GetSphereInitCount();

    protected abstract bool IsAutoDistance();
}