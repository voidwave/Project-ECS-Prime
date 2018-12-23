using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
public class CameraSystem : MonoBehaviour
{
    public EntityManager entityManager;
    public Entity player;
    public float speed = 1;
    void Update()
    {
        if (player == null)
            return;

        Vector3 playerPosition = entityManager.GetComponentData<Position>(player).Value;
        playerPosition.y += 8;
        playerPosition.z -= 10;
        
        Vector3 dir = playerPosition - transform.localPosition;
        dir = dir.normalized;
        
        transform.localPosition = transform.localPosition + Time.deltaTime * speed * dir;


    }



}