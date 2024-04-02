using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerDialouge : MonoBehaviour
{
    public GameObject savedCollisionNPC;
    public GameObject dialogueIndicator;
    [SerializeField, Tooltip("How far you can be from an NPC and still talk to them")]
    private float talkDistance = 5f;



    // Start is called before the first frame update
    void Start()
    {
        savedCollisionNPC = null;
        dialogueIndicator = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetNPC()
    {
        return savedCollisionNPC;
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            savedCollisionNPC = other.gameObject;
           


            // update dialogue indicator
            if (dialogueIndicator != null)
            {
                dialogueIndicator.SetActive(false);
                
            }
            dialogueIndicator = savedCollisionNPC.GetComponent<NPCDialogueManager>().dialougeIndicator;
            dialogueIndicator.SetActive(true);
        }
    }
}
