using Fusion;

[System.Serializable]
public struct NetInput : INetworkInput
{
    public byte buttons; // bit0: JumpHeld, bit1: JumpReleased, bit2: Left, bit3: Right
    public float horizontal; // -1..1
}


public static class InputButtons
{
    public const byte JumpHeld = 1 << 0;
    public const byte JumpReleased = 1 << 1;
    public const byte Left = 1 << 2;
    public const byte Right = 1 << 3;
}