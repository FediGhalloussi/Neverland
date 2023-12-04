using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateGameActiver : MonoBehaviour
{
    [SerializeField] private GameObject part1;

    [SerializeField] private GameObject part2;
    
    public void Active1()
    {
        part1.SetActive(true);
    }

    public void Active2()
    {
        part2.SetActive(true);
    }
}
