using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameActiver
{
    void ActivateGame();
    
    GameObject gameObject { get ; } 

}