using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
public class CameraSystem : MonoBehaviour
{
    public EntityManager entityManager;
    public Entity player;
    [SerializeField] private float speed = 1;
    void Update()
    {
        if (player == null)
            return;

        Vector3 playerPosition = entityManager.GetComponentData<Translation>(player).Value;
        playerPosition.y += 8;
        playerPosition.z -= 10;

        Vector3 dir = playerPosition - transform.localPosition;
        //dir = dir.normalized;
        if (dir.magnitude > 0.1f)
            transform.localPosition = transform.localPosition + Time.deltaTime * speed * dir;


    }



}