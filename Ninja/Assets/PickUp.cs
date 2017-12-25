using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public enum Type
    {
        NULL = -1,
        HEALTH = 0,
        COIN = 1
    }

    [SerializeField] Type type = Type.NULL;
    [SerializeField] float amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TestPlayer player = other.gameObject.GetComponent<TestPlayer>();
            player.OnPickUp(type, amount);

            Destroy(gameObject);
        }
    }
}
