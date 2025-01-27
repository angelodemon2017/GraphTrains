using UnityEngine;

public interface IMovable
{
    float GetBaseMoveSpeed { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns>is done target</returns>
    bool Move(Vector3 target, float speed);
}