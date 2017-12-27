using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Controllers/BoidController.asset", menuName = "Boid Controller")]
public class BoidController : ScriptableObject
{
    public Boid BoidPrefab;
    public int SpawnNumber = 10;
    public float SpawnRadius = 10f;
    public float SeparationRadius = 5f;
    public float CohesionRadius = 12f;
    public float AlignmentRadius = 7f;
    [Range(0f, 10f)] public float SeparationStrength = 1f;
    [Range(0f, 10f)] public float CohesionStrength = 1f;
    [Range(0f, 10f)] public float AlignmentStrength = 1f;
    public bool Debug;

    private List<Boid> boids;
    private OctreeController octreeController;

    public void Init()
    {
        octreeController = BoidDemo.Instance.OctreeController;
        boids = new List<Boid>();
        for (int i = 0; i < SpawnNumber; i++)
        {
            Boid b = Instantiate(BoidPrefab);

            b.transform.position = Random.insideUnitSphere * SpawnRadius;
            b.transform.rotation = Random.rotation;

            boids.Add(b);
            octreeController.Tree.Add(b, b.transform.position);
        }
    }

    public void Update()
    {
        FindNeighbors();
        ApplySeparationBehavior();
        ApplyAlignmentBehavior();
        ApplyCohesionBehavior();
        MoveBoids();
    }

    private void FindNeighbors()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Boid b = boids[i];

            octreeController.Tree.GetNearbyNonAlloc(b.transform.position, CohesionRadius, b.Neighbors);
            b.Neighbors.Remove(b);

            // TODO - Filtering: Can boid see other boids?
        }
    }

    private void ApplySeparationBehavior()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Boid b = boids[i];
            int count = 0;
            Vector3 avgDir = Vector3.zero;
            Vector3 dest;

            for (int j = 0; j < b.Neighbors.Count; j++)
            {
                Boid n = b.Neighbors[j];
                Vector3 toNeighbor = n.transform.position - b.transform.position;
                float dist = toNeighbor.magnitude;

                if (dist < SeparationRadius)
                {
                    float falloffFactor = 1f - dist / SeparationRadius;

                    avgDir += toNeighbor * falloffFactor;
                    count++;
                }
            }

            if (count == 0)
            {
                continue;
            }

            avgDir /= count;
            dest = b.transform.position + avgDir * -1f;
            b.SteerTowards(dest, SeparationStrength);
            if (Debug)
            {
                DebugExtension.DebugArrow(dest, avgDir * -1f * SeparationStrength, Color.yellow, 0.1f);
            }
        }
    }

    private void ApplyCohesionBehavior()
    {
        // Steer towards local average position
        for (int i = 0; i < boids.Count; i++)
        {
            Boid b = boids[i];
            Vector3 avgPos = Vector3.zero;

            if (b.Neighbors.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < b.Neighbors.Count; j++)
            {
                avgPos += b.Neighbors[j].transform.position;
            }

            avgPos /= b.Neighbors.Count;
            b.SteerTowards(avgPos, CohesionStrength);
            if (Debug)
            {
                DebugExtension.DebugPoint(avgPos, Color.blue, 0.1f);
            }
        }
    }

    private void ApplyAlignmentBehavior()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Boid b = boids[i];
            Vector3 avgDir = b.transform.forward;
            Vector3 dest;
            int count = 0;

            for (int j = 0; j < b.Neighbors.Count; j++)
            {
                Boid n = b.Neighbors[j];
                Vector3 toNeighbor = n.transform.position - b.transform.position;
                float dist = toNeighbor.magnitude;

                if (dist < AlignmentRadius)
                {
                    float falloffFactor = 1f - dist / AlignmentRadius;

                    avgDir += n.transform.forward * falloffFactor;
                    count++;
                }
            }

            if (count == 0)
            {
                continue;
            }

            avgDir /= count;
            dest = b.transform.position + avgDir;
            b.SteerTowards(dest, AlignmentStrength);

            if (Debug)
            {
                DebugExtension.DebugArrow(dest, avgDir * AlignmentStrength, Color.green, 0.1f);
            }
        }
    }


    private void MoveBoids()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Boid b = boids[i];
            b.MoveForward();
        }
    }
}
