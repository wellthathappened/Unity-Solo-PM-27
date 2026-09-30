using UnityEngine;

public class AdvancedEnemy : Enemy
{ 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Any function that exists in the original class (Enemy) will be overridden here IF present
    }

    // Update is called once per frame
    void Update()
    {
        // Using this update fucntion will stop the original from working
        // You can add new functionality here!
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Same goes for collision functions
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }
}
