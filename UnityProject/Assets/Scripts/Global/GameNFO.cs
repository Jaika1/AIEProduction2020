using UnityEngine.InputSystem;
/// <summary>
/// A public class that contains major game information that is to be kept between scenes (such as score).
/// </summary>
public static class GameNFO
{
    /// <summary>
    /// The maximum amount of players that can play the game.
    /// </summary>
    public const int PlayerLimit = 4;
    /// <summary>
    /// Gamepad instances representing the currently connected controllers that are in use in order of player
    /// </summary>
    public static Gamepad[] PlayerControllers = new Gamepad[PlayerLimit];
}
