using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float SteerSpeed = 1f;
    [HideInInspector] public List<Boid> Neighbors = new List<Boid>();

    private Quaternion lookRot;

    public void SteerTowards(Vector3 target, float modifier = 1f)
    {
        Vector3 dir = Vector3.Normalize(target - transform.position);

        lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, SteerSpeed * modifier * Time.deltaTime);
    }

    public void MoveForward()
    {
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;
    }
}
