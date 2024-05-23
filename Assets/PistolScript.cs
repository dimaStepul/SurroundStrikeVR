using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolScript : MonoBehaviour
{
    public HandGrabInteractable interactable;
    public OVRInput.Button shootButton;
    public AudioSource shootSound;
    public GameObject bullet;
    public float bulletForwardOffset;
    public float bulletUpOffset;
    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable.State == InteractableState.Select && OVRInput.GetDown(shootButton, OVRInput.Controller.RTouch))
        {
            shootSound.Play();
            var pos = transform.position + transform.forward * bulletForwardOffset + transform.up * bulletUpOffset;
            var newBullet = Instantiate(bullet);
            newBullet.transform.position = pos;
            newBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            StartCoroutine(DestroyBulletWithDelay(newBullet));
        }
    }

    IEnumerator DestroyBulletWithDelay(GameObject bullet)
    {
        yield return new WaitForSeconds(5f);
        Destroy(bullet);
    }
}
