using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public bool isActive;


    public Text nameText;
    public Text dialogText;

    public Animator animator;


    private Queue<string> meningar;
    // Start is called before the first frame update
    void Start()

    {
        meningar = new Queue<string>();
    }

    private void Update()
    {
        if (isActive = true && Input.GetKeyDown(KeyCode.C)) 
        {
            DisplayNextMening();
        }
    }

    public void StartDialog (Dialog dialog)
    {
        isActive = true;
        animator.SetBool("IsOpen", true);

        nameText.text = dialog.name;

        meningar.Clear();

        foreach (string mening in dialog.meningar)
        {
            meningar.Enqueue(mening);
        }
        DisplayNextMening();
    }
    public void DisplayNextMening() 
    {
        if (meningar.Count == 0)
        {
            EndDialog();
            return;
        }

        string mening = meningar.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeMening(mening));
    }

    IEnumerator TypeMening (string mening)
    {
        dialogText.text = "";
        foreach (char letter in mening.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }

    void EndDialog()
    {
        isActive = false;
        animator.SetBool("IsOpen", false);
    }
    
}
