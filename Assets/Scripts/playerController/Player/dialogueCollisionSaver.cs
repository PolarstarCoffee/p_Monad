using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogueCollisionSaver : MonoBehaviour
{
    private GameObject savedCollisionNPC;
    public GameObject dialogueIndicator;
    [SerializeField, Tooltip("How far you can be from an NPC and still talk to them")]
    private float talkDistance = 5f;
    
   

    // Start is called before the first frame update
    void Start()
    {
        savedCollisionNPC = null;
        dialogueIndicator = null;
     

    }

    public GameObject GetLastCollidedNPC()
    {
        if (Vector3.Distance(transform.position, savedCollisionNPC.transform.position) > talkDistance){
          
            return null;
        }

        else{
            return savedCollisionNPC;
           
        }
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    // save last npc you collided with
    //    if (hit.gameObject.tag == "NPC")
    //        savedCollisionNPC = hit.gameObject;
    //        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            savedCollisionNPC = other.gameObject;
            

            // update dialogue indicator
            if (dialogueIndicator != null)
            {
                dialogueIndicator.SetActive(false);
                dialogueIndicator = null;
            }
            dialogueIndicator = savedCollisionNPC.GetComponent<NPCDialogueManager>().dialougeIndicator;
            dialogueIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        dialogueIndicator.SetActive(false);
    }
}
