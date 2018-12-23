using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine.Rendering;

public static class GameManager
{
    private static Mesh PlayerMesh = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshFilter>().sharedMesh;
    private static Material PlayerMaterial = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshRenderer>().sharedMaterial;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Initilize()
    {
        Debug.Log("GameManager Initilize();");
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();

        var playerArchetype = entityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(Position),
            typeof(Rotation),
            typeof(MoveSpeed),
            typeof(MeshInstanceRenderer)
        );

        var player = entityManager.CreateEntity(playerArchetype);

        entityManager.SetSharedComponentData(player, new MeshInstanceRenderer
        {
            mesh = PlayerMesh,
            material = PlayerMaterial,
            castShadows = ShadowCastingMode.On
        });

        entityManager.SetComponentData(player, new MoveSpeed{ Value = 1 });

    }

}
