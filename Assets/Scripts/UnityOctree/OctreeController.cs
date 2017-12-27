using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Controllers/OctreeController.asset", menuName = "Octree Controller")]
public class OctreeController : ScriptableObject
{
    public float InitialWorldSize = 15f;
    public Vector3 InitialPosition;
    public float MinSize = 1f;

    private PointOctree<Boid> tree;

    public PointOctree<Boid> Tree { get { return tree; } }

    public void Init()
    {
        tree = new PointOctree<Boid>(InitialWorldSize, InitialPosition, MinSize);
    }

    // temp
    void OnDrawGizmos()
    {
        tree.DrawAllBounds(); // Draw node boundaries
        tree.DrawAllObjects(); // Mark object positions
    }
}
