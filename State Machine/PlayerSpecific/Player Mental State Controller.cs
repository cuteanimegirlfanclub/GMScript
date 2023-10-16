using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Player/Player Mental Controller")]
public class PlayerMentalStateController : StateMachineController
{
    //trying not to use monobehaviour


    //the player mental threshold are stored in the PlayerConfigure(or PlayerProperty) editor class
    //i will make player status scriptable object, and make it function as a configurator in the future
    //to make it editor friendly
}
