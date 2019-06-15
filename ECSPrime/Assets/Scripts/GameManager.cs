
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine.Rendering;
using Unity.Collections;
using Unity.Mathematics;
//using Unity.Physics;
public class GameManager : MonoBehaviour
{
    private UnityEngine.Mesh PlayerMesh, EnemyMesh;// = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshFilter>().sharedMesh;
    private UnityEngine.Material PlayerMaterial, EnemyMaterial;// = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshRenderer>().sharedMaterial;
    [SerializeField] private CameraSystem cameraSystem;
    void Awake()
    {
        PlayerMesh = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshFilter>().sharedMesh;
        PlayerMaterial = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<MeshRenderer>().sharedMaterial;

        EnemyMesh = (Resources.Load("Prefabs/Enemy") as GameObject).GetComponent<MeshFilter>().sharedMesh;
        EnemyMaterial = (Resources.Load("Prefabs/Enemy") as GameObject).GetComponent<MeshRenderer>().sharedMaterial;

    }
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    void Start()
    {
        Debug.Log("GameManager Initilize();");
        var entityManager = World.Active.EntityManager;//GetOrCreateSystem<EntityManager>();

        //Player Spawn
        var playerArchetype = entityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(Translation),
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
            typeof(RenderMesh),
            typeof(Unity.Physics.PhysicsCollider)
        );

        var player = entityManager.CreateEntity(playerArchetype);

        entityManager.SetSharedComponentData(player, new RenderMesh
        {
            mesh = PlayerMesh,
            material = PlayerMaterial,
            castShadows = ShadowCastingMode.On
        });

        Unity.Entities.BlobAssetReference<Unity.Physics.Collider> collider = Unity.Physics.BoxCollider.Create(float3.zero, quaternion.identity, new float3(1, 1, 1), 0);
        Unity.Physics.PhysicsCollider physicsCollider = new Unity.Physics.PhysicsCollider
        {
            Value = collider
        };
        entityManager.SetComponentData(player, physicsCollider);
        entityManager.SetComponentData(player, new Translation { Value = new float3(0, 0, 0) });
        //entityManager.SetComponentData(player, new UnitStats { team = 0, Health = 100, MaxHealth = new Stat(100), DamageReduction = new Stat(0), Power = new Stat(10), MovementSpeed = new Stat(1) });
        //entityManager.SetComponentData(player, new Team { Value = 0 });
        entityManager.SetComponentData(player, new MovementSpeed { Value = 1 });
        //Bots Spawn
        var enemyArchetype = entityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(Rotation),
            //typeof(Team),
            typeof(MovementSpeed),
            typeof(Health),
            typeof(MaxHealth),
            typeof(Power),
            typeof(DamageReduction),
            typeof(Heading),
            //typeof(Target),
            typeof(RenderMesh),
            typeof(Unity.Physics.PhysicsCollider)
        );

        NativeArray<Entity> enemyEntities = new NativeArray<Entity>(10000, Allocator.Temp);
        entityManager.CreateEntity(enemyArchetype, enemyEntities);

        RenderMesh enemyMeshRenderer = new RenderMesh
        {
            mesh = EnemyMesh,
            material = EnemyMaterial,
            castShadows = ShadowCastingMode.On
        };



        for (int i = 0; i < enemyEntities.Length; i++)
        {
            entityManager.SetComponentData(enemyEntities[i], physicsCollider);
            entityManager.SetSharedComponentData(enemyEntities[i], enemyMeshRenderer);
            entityManager.SetComponentData(enemyEntities[i], new Translation { Value = new float3(UnityEngine.Random.Range(-100, 100), 0, UnityEngine.Random.Range(-100, 100)) });
            //entityManager.SetComponentData(enemyEntities[i], new Team { Value = 1 });
            entityManager.SetComponentData(enemyEntities[i], new MovementSpeed { Value = 1 });
        }

        enemyEntities.Dispose();

        cameraSystem.entityManager = entityManager;
        cameraSystem.player = player;
    }

}

