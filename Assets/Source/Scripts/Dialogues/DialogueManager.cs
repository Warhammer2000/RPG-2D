using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[System.Serializable]
public class DialogueManager : MonoBehaviour
{
    [Inject] private Inventory inventory;   


    [SerializeField] private GameObject UI;
    [SerializeField] private Image Portrate;
    [SerializeField] private Text Name;
    [SerializeField] private Text ReplicasText;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Text[] buttonText;

    [SerializeField] private DialogueSettings settings;
    [SerializeField] private Dialogue dialog;

    [SerializeField] private int replicasIndex;


    [SerializeField] private int textIndex;
    [SerializeField] private bool stopReplicas = false;
    [SerializeField] private GameObject ItemPref;

    [Inject] private PlayerController playerController;
    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        UI = transform.GetChild(0).gameObject;
        Portrate = UI.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        Name = UI.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        ReplicasText = UI.transform.GetChild(2).GetComponent<Text>();

        buttons = new GameObject[3];
        buttonText = new Text[3];

        buttons[0] = UI.transform.GetChild(3).gameObject;
        buttons[1] = UI.transform.GetChild(4).gameObject;
        buttons[2] = UI.transform.GetChild(5).gameObject;

        buttonText[0] = buttons[0].transform.GetChild(0).GetComponent<Text>();
        buttonText[1] = buttons[1].transform.GetChild(0).GetComponent<Text>();
        buttonText[2] = buttons[2].transform.GetChild(0).GetComponent<Text>();

        buttons[0].GetComponent<Button>().onClick.AddListener(But0);
        buttons[1].GetComponent<Button>().onClick.AddListener(But1);
        buttons[2].GetComponent<Button>().onClick.AddListener(But2);

        ItemPref = Resources.Load<GameObject>("Prefabs/Item");

        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
        buttons[2].SetActive(false);



        UI.SetActive(false);
    }
    private void Update()
    {
        if (dialog)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !stopReplicas)
            {
                DialogueReader();
            }
           
        }
    }
    public void StartDialogue(DialogueSettings newDialogueSettings)
    {
        settings = newDialogueSettings;
        settings.dialogueStarted = true;

        if (settings.dialogueEnded) dialog = settings.dialogEnd;
        else dialog = settings.dialogue;

        Portrate.sprite = dialog.NpcImage;
        Name.text = dialog.npcName;

        replicasIndex = 0;
        textIndex = 0;
        UI.SetActive(true);
        DialogueReader();
    }
    private void DialogueReader()
    {
        if(textIndex < dialog.replicas[replicasIndex].replicaText.Length)
        {
            OutReplicaText();
        }
        else
        {
            OnAswer();
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        OnAswer();
        Debug.Log("Replica over");
    }
    private void OutReplicaText()
    {
        ReplicasText.text = dialog.replicas[replicasIndex].replicaText[textIndex];
        textIndex++;
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
       
    }
    private void OnAswer()
    {
        for(int i = 0; i < dialog.replicas[replicasIndex].answers.Length; i++)
        {
            buttons[i].SetActive(true);
            buttonText[i].text = dialog.replicas[replicasIndex].answers[i].answerText;
        }
        stopReplicas = true;
        ReplicasText.text = "";
    }
    private void OffAnswer()
    {
        for (int i = 0; i < dialog.replicas[replicasIndex].answers.Length; i++)
        {
            buttons[i].SetActive(false);
            Debug.Log("False");
            ReplicasText.text = "";
        }
        stopReplicas = false;
    }
    private void AnswerReader(AnswerTypes type, int link)
    {
        OffAnswer();
        switch (type)
        {
            case AnswerTypes.nextReplica: 
                replicasIndex = link;
                DialogueReader();
                break;
            case AnswerTypes.exit:
                Exit(); break;

            case AnswerTypes.healhpAndFinish:
                PlayerStats.PlayerHealth = PlayerStats.PlayerMaxHealth;
                settings.dialogueEnded = true;
                Exit(); break;

            case AnswerTypes.healmpAndFinish:
                PlayerStats.PlayerMana = PlayerStats.PlayerMaxMana;
                settings.dialogueEnded = true;
                Exit(); break;

            case AnswerTypes.healAllAndFinish:
                PlayerStats.PlayerHealth = PlayerStats.PlayerMaxHealth;
                PlayerStats.PlayerMana = PlayerStats.PlayerMaxMana;
                settings.dialogueEnded = true;
                Exit();
                break;

            case AnswerTypes.giveItemAndFinish:
                if(inventory.AddItem(ItemManager.items[link], 1))
                {
                    Debug.Log("You get " + ItemManager.items[link].name);
                }
                else
                {
                    ItemSettings temp = Instantiate(ItemPref, playerController.transform.position, Quaternion.identity).GetComponent<ItemSettings>();   
                    temp.thisItem = ItemManager.items[link];
                    temp.count = 1;
                }
                Exit(); 
                break;
        }
    }
    private void Exit()
    {
        settings.dialogueStarted = false;

        replicasIndex = 0;
        textIndex = 0;
        dialog = null;
        settings = null;
        UI.SetActive(false);
        Interactive.player.dialogue = null;
    }
    private void But0()
    {
        AnswerReader(dialog.replicas[replicasIndex].answers[0].answerType, dialog.replicas[replicasIndex].answers[0].links);
    }
    private void But1()
    {
        AnswerReader(dialog.replicas[replicasIndex].answers[1].answerType, dialog.replicas[replicasIndex].answers[1].links);
    }
    private void But2()
    {
        AnswerReader(dialog.replicas[replicasIndex].answers[2].answerType, dialog.replicas[replicasIndex].answers[2].links);
    }
}
