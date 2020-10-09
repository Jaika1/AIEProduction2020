using UnityEngine;

/// <summary>
/// Contains important information and other variables for players and other NPCs
/// </summary>
public class Entity : MonoBehaviour
{
    [Tooltip("The rate at which the player will speed up while moving")]
    public float MovementSpeed = 256.0f;
}
