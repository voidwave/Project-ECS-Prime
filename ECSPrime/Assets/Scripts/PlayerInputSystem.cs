using UnityEngine;
using Unity.Entities;

public class PlayerInputSystem : ComponentSystem
{
    struct PlayerData
    {
        //public ComponentDataArray<CameraData> cameraData;
        public ComponentDataArray<PlayerInput> Input;
        public ComponentDataArray<Heading> heading;
        public ComponentDataArray<MovementSpeed> speed;
    }

    [Inject] private PlayerData m_Players;

    protected override void OnUpdate()
    {
        Heading playerheading;
        playerheading.Value.x = Input.GetAxis("Horizontal");
        playerheading.Value.y = 0;
        playerheading.Value.z = Input.GetAxis("Vertical");
        PlayerInput pi;
        pi.MousePosition.x = Input.mousePosition.x;
        pi.MousePosition.y = Input.mousePosition.y;
        pi.MousePosition.z = Input.mousePosition.z;
        //pi.FireCooldown = Mathf.Max(0.0f, m_Players.Input[i].FireCooldown - dt);
        m_Players.speed[0] = new MovementSpeed { Value = Input.GetKey(KeyCode.LeftShift) ? 5 : 1 };
        m_Players.Input[0] = pi;
        m_Players.heading[0] = playerheading;
    }

}