using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZombiesManager : MonoBehaviour
{
    public List<ZombieBehaviour> listOfZombies;

    public void OnSeePlayer(Vector3 position)
    {
        foreach (ZombieBehaviour zombie in listOfZombies)
        {
            zombie.SeePlayer(position);
        }
    }

    public void OnSeeBlood(Transform blood)
    {
        foreach (ZombieBehaviour zombie in listOfZombies)
        {
            zombie.SeeSmell(blood);
        }
    }
}
