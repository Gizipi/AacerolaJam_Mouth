using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    public static bool aquiredBattery = false;
    public static bool batteryPluggedIn = false;
    public static bool crankFlipped = false;
    public static bool startGame = true;
    public static bool gameOver = false;
    public static bool dead = false;


    public static Vector3[] currentDirections = new Vector3[4];

    public static Vector3 currentForward = Vector3.forward; // make directions relative to monster facing direction
    public static Vector3 currentRight = Vector3.right;
    public static Vector3 currentBack = Vector3.back;
    public static Vector3 currentLeft = Vector3.left;

    public static AttackStage attackStage = AttackStage.none;
}
