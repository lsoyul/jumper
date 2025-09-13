using Fusion;
using UnityEngine;

public enum MoveState : byte { Air, Grounded }

public class PlayerMotor2D : NetworkBehaviour
{
    [Networked] public Vector2 Pos { get; set; }
    [Networked] public Vector2 Vel { get; set; }
    [Networked] public MoveState State { get; set; }
    [Networked] public int Facing { get; set; } // -1 or +1

    // Parameters for Jump
    [SerializeField] float minHold = 0.06f, maxHold = 1.2f;
    [SerializeField] float pMin = 6f, pMax = 16f, gamma = 1.4f;
    [SerializeField] float angleDeg = 60f;
    [SerializeField] float gravity = -32f;

    // 홀드 상태
    float holdT = -1f; int holdDir = 0;

    // 발판 레이어 마스크
    [SerializeField] LayerMask platformMask;

    public override void Spawned()
    {
        base.Spawned();

        // Networked <- Transform 초기화
        if (Object.HasStateAuthority)
        {
            Pos = transform.position;
            Vel = Vector2.zero;
            State = MoveState.Grounded;
            Facing = 1;
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (GetInput(out JumpInput input))
        {
            // 1. 입력
            bool left = input.Buttons.IsSet(0);
            bool right = input.Buttons.IsSet(1);

            // 2. 홀드 시작
            if (holdT < 0 && (left ^ right))
            {
                holdT = 0f;
                holdDir = left ? -1 : 1;
                Facing = holdDir;
            }

            // 3. 홀드 중 갱신
            if (holdT >= 0 && (left ^ right))
            {
                holdT += Runner.DeltaTime;
            }

            // 4. 홀드 종료
            if (holdT >= 0 && !(left ^ right))
            {
                float t = Mathf.Clamp01((holdT - minHold) / (maxHold - minHold));
                float power = Mathf.Lerp(pMin, pMax, Mathf.Pow(t, gamma));
                float rad = Mathf.Deg2Rad * angleDeg * holdDir;
                Vel = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad) * power);
                State = MoveState.Air;
                holdT = -1f; holdDir = 0;
            }
        }
    }
}
