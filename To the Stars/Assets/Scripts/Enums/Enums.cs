using UnityEngine;
using System.Collections;

public enum Position
{
	Left, Right, Top, Bottom, TopLeft, BottomRight, Center, CenterHalfTop, CenterHalfBottom
}

public enum Direction
{
    Down = -1, Up = 1
}

public enum InputType
{
    JumpUp, JumpDown, DashUp, DashDown, OpenUmbrella, CloseUmbrella
}

public enum PlayerState
{
    Idle, JumpingDown, JumpingUp, DashingDown, DashingUp
}

public enum LanePos
{
    Top, Middle, Bottom
}

public enum StarState
{
    Sad,     // Unlit star
    Happy,   // Lighten star after being touch by the player
    Lost     // Star that the player missed to lighten
}

public enum GameOverCause
{
    CloudStrike,        // The player collided with a cloud
    UmbrellaFrontCloud, // Cloud hit on the front with the umbrella
    UmbrellaHitStar,    // Star hit with the umbrella
    LostStar,           // The player missed to take a star
}

public enum MenuLevelState
{
    Locked,
    Unlocked
}