using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeathScript : MonoBehaviour
{
    //Flag for Player Death
    public bool showDeath;
    //Flag to Destroy Enemies that touch
    public bool destoryEnemies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*switch (other.gameObject.tag)
        {
            case "Player":
                other.gameObject.GetComponent<PlayerMovement>().Defeat(showDeath);
                break;

            case "Enemy":
                if (destoryEnemies)
                {
                    Destroy(other.gameObject);
                }
                break;
        }*/
    }
}
