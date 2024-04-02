using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shadow : MonoBehaviour
{
    // Reference to the player object
    public Transform player;
    // Prefab for the shadow sprite
    public GameObject shadowPrefab;

    // Reference to the instantiated shadow
    private GameObject shadowInstance;

    private void Start()
    {
        // Instantiate the shadow prefab at the start
        shadowInstance = Instantiate(shadowPrefab, player.position, Quaternion.identity);
    }

    private void Update()
    {
        // Create a Ray object
        Ray ray = new Ray(player.position, Vector3.down);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Update the shadow's position to the hit point
            Vector3 shadowPosition = hit.point;

            // Add a small offset to the y-coordinate
            shadowPosition.y += 0.05f;
            shadowInstance.transform.position = shadowPosition;

            // Rotate shadow to horizontal orientation
            shadowInstance.transform.rotation = Quaternion.Euler(90, 0, 0);

            //Change opcaity of the shadow sprite based on raycast distance
            SpriteRenderer sr = shadowInstance.GetComponent<SpriteRenderer>();
            float alpha = Mathf.Clamp(1 - hit.distance / 3.0f, 0.1f, 0.9f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

            //Raycast Debug
            //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

        }
    }
}