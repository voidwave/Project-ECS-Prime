using UnityEngine;
using Unity.Entities;

public class PlayerInputSystem : ComponentSystem
{
    struct PlayerData
    {
        //public ComponentDataArray<CameraData> cameraData;
        public ComponentDataArray<PlayerInput> Input;
    }

    [Inject] private PlayerData m_Players;

    protected override void OnUpdate()
    {
        PlayerInput pi;
        pi.MoveDir.x = Input.GetAxis("Horizontal");
        pi.MoveDir.y = 0.0f;
        pi.MoveDir.z = Input.GetAxis("Vertical");
        pi.MousePosition.x = Input.mousePosition.x;
        pi.MousePosition.y = Input.mousePosition.y;
        pi.MousePosition.z = Input.mousePosition.z;
        //pi.FireCooldown = Mathf.Max(0.0f, m_Players.Input[i].FireCooldown - dt);

        m_Players.Input[0] = pi;
    }

}