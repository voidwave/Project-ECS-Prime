
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine.Rendering;
using Unity.Collections;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    private Mesh PlayerMesh;// = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshFilter>().sharedMesh;
    private Material PlayerMaterial;// = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshRenderer>().sharedMaterial;
    public CameraSystem cameraSystem;
    void Awake()
    {
        PlayerMesh = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshFilter>().sharedMesh;
        PlayerMaterial = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshRenderer>().sharedMaterial;

    }
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    void Start()
    {
        Debug.Log("GameManager Initilize();");
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();

        //Player Spawn
        var playerArchetype = entityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(Position),
            typeof(Rotation),
            //typeof(Team),
            typeof(MovementSpeed),
            typeof(Health),
            typeof(MaxHealth),
            typeof(Power),
            typeof(DamageReduction),
            typeof(PlayerInput),
            typeof(Heading),
            //typeof(Target),
            typeof(MeshInstanceRenderer)
        );

        var player = entityManager.CreateEntity(playerArchetype);

        entityManager.SetSharedComponentData(player, new MeshInstanceRenderer
        {
            mesh = PlayerMesh,
            material = PlayerMaterial,
            castShadows = ShadowCastingMode.On
        });

        entityManager.SetComponentData(player, new Position { Value = new float3(0, 0, 0) });
        //entityManager.SetComponentData(player, new UnitStats { team = 0, Health = 100, MaxHealth = new Stat(100), DamageReduction = new Stat(0), Power = new Stat(10), MovementSpeed = new Stat(1) });
        //entityManager.SetComponentData(player, new Team { Value = 0 });
        entityManager.SetComponentData(player, new MovementSpeed { Value = new Stat(1) });
        //Bots Spawn
        var enemyArchetype = entityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(Position),
            typeof(Rotation),
            //typeof(Team),
            typeof(MovementSpeed),
            typeof(Health),
            typeof(MaxHealth),
            typeof(Power),
            typeof(DamageReduction),
            typeof(Heading),
            //typeof(Target),
            typeof(MeshInstanceRenderer)
        );

        NativeArray<Entity> enemyEntities = new NativeArray<Entity>(10000, Allocator.Temp);
        entityManager.CreateEntity(enemyArchetype, enemyEntities);

        MeshInstanceRenderer enemyMeshRenderer = new MeshInstanceRenderer
        {
            mesh = PlayerMesh,
            material = PlayerMaterial,
            castShadows = ShadowCastingMode.On
        };

        for (int i = 0; i < enemyEntities.Length; i++)
        {
            entityManager.SetSharedComponentData(enemyEntities[i], enemyMeshRenderer);
            entityManager.SetComponentData(enemyEntities[i], new Position { Value = new float3(UnityEngine.Random.Range(-100, 100), 0, UnityEngine.Random.Range(-100, 100)) });
            //entityManager.SetComponentData(enemyEntities[i], new Team { Value = 1 });
            entityManager.SetComponentData(enemyEntities[i], new MovementSpeed { Value = new Stat(1) });
        }

        enemyEntities.Dispose();

        cameraSystem.entityManager = entityManager;
        cameraSystem.player = player;
    }

}

