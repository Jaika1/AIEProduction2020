using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Entity))]
/// <summary>
/// Allows player movement via input from a pre-defined controller port
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Definition for our input actions
    /// </summary>
    private InputActions inputActions;
    /// <summary>
    /// The rigidbody that is attached to the GameObject this component is also attached to
    /// </summary>
    private Rigidbody rbody;
    /// <summary>
    /// The entity class inherited by the attached GameObject
    /// </summary>
    private Entity entity;
    public Gamepad playerGamepad;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();

        rbody = GetComponent<Rigidbody>();
        rbody.freezeRotation = true;

        inputActions = new InputActions();
        inputActions.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveMagnitude = inputActions.Player.Move.ReadValue<Vector2>();
        moveMagnitude = moveMagnitude.magnitude > 1.0f ? moveMagnitude.normalized : moveMagnitude; // Limits the total magnitude of the vector to 1 (x + y <= 1.0f)
        moveMagnitude *= entity.MovementSpeed * Time.deltaTime; // Muliply the movement vector by the speed and delta time
        rbody.velocity = new Vector3(moveMagnitude.x, rbody.velocity.y, moveMagnitude.y); // Apply the new velocity to the rigidbody
    }
}
