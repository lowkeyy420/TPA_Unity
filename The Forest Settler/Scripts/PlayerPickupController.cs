using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    private MeshCollider collider;
    [SerializeField]
    private Transform player, weaponContainer;

    [SerializeField]
    private float pickupRange;

    [SerializeField]
    private bool weaponEquipped;

    private static bool slotAvailable;

    public static bool getWeaponStatus()
    {
        return slotAvailable;
    }


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<MeshCollider>();

        //check weapon from begining
        if (!weaponEquipped)
        {
            rb.isKinematic = false;
            collider.isTrigger = false;
            slotAvailable = true;
        }

        if (weaponEquipped)
        {
            rb.isKinematic = true;
            collider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If player in range and E is pressed + no weapon equipped + slot weapon available
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!weaponEquipped && distanceToPlayer.magnitude <= pickupRange && Input.GetKeyDown(KeyCode.E) && slotAvailable) {
            useWeapon();
        }

        // If player is holding a weapon && pressed Q
        if(weaponEquipped && Input.GetKey(KeyCode.Q)){
            dropWeapon();
        }
    }

    void useWeapon()
    {
        weaponEquipped = true;
        slotAvailable = false;

        // set weapon to a default position and child of camera
        transform.SetParent(weaponContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        collider.isTrigger = true;

    }

    void dropWeapon()
    {
        weaponEquipped = false;
        slotAvailable = true;

        // set weapon parent null
        transform.SetParent(null);

        rb.isKinematic = false;
        collider.isTrigger = false;

    }
}
