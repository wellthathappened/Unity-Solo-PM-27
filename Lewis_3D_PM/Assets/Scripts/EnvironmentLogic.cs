using UnityEngine;

public class EnvironmentLogic : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject, collision.gameObject.GetComponent<AudioSource>().clip.length);
        }
    }
}
