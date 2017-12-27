using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidDemo : MonoBehaviour
{
    public static BoidDemo Instance;

    public BoidController BoidController;
    public OctreeController OctreeController;

    private void Awake()
    {
        Instance = this;
        OctreeController.Init();
        BoidController.Init();
    }

    private void Update()
    {
        BoidController.Update();
    }
}
