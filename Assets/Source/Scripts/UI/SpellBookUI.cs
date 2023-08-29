using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SpellBookUI : MonoBehaviour
{
    public BookScript[] books;
    private Transform bookPanel;


    public Spell selectedSpell;
    public BookScript cursorBook;
    public BookScript selectedBook;
    public BookScript previousBook;
    //InfoSettings
    private Transform infoPanel;
    private Image infoImage;
    private Text infoName;
    private Text infoDesc;
    private Text infoEffect;
    //EquipSettings
    private Transform equipPanel;
    private EquipBookScript[] equipBooks;
    private GameObject[] equipButtons;

    //Colors;
    [Header("SpellBookColors")]
    public Color cursorColor;
    public Color myColor;
    public Color selectColor;
    public Color equipColor;

    private bool isInitialized;
    [Inject] private SpellBook spell;
    public void Access()
    {
        bookPanel = transform.GetChild(0);
        infoPanel = transform.GetChild(1);  
        equipPanel = transform.GetChild(2);

        //Inventory
        books = new BookScript[11];
        for(int i = 0; i < books.Length; i++)
        {
            books[i] = bookPanel.GetChild(i).GetComponent<BookScript>().GetLinkSetSettings(i, this);
        }
        //Info
        infoImage = infoPanel.GetChild(0).GetChild(0).GetComponent<Image>();
        infoName = infoPanel.GetChild(1).GetComponent<Text>();
        infoDesc = infoPanel.GetChild(2).GetComponent<Text>();
        infoEffect = infoPanel.GetChild(3).GetComponent<Text>();

        //Equip To Write
        equipBooks = new EquipBookScript[2];
        equipButtons = new GameObject[2];
        for (int i = 0; i < equipBooks.Length; i++)
        {
            equipBooks[i] = equipPanel.GetChild(i).GetComponent<EquipBookScript>();
            equipButtons[i] = equipPanel.GetChild(i + 2).gameObject;
        }
        isInitialized = true;
    }
    public void OnButtonClick(int index)
    {
        spell.SetEquip(index, selectedBook.BookID);
        selectedBook = null;
        RefreshAll();
    }
    private void Update()
    {
        if (cursorBook)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectBookSwitch();
            }
        }
    }
    private void ButtonSwitcher()
    {
        equipButtons[0].SetActive(selectedBook != null);
        equipButtons[1].SetActive(selectedBook != null);
    }
    public void CursorBookSwitch(BookScript newBook)
    {
        if (!cursorBook)
        {
            cursorBook = newBook;
        }
        else cursorBook = null;

        RefreshAll();
    }
    private void SelectBookSwitch()
    {
        if (!selectedBook) selectedBook = cursorBook;
        else
        {
            if (selectedBook == cursorBook) selectedBook = null;
            else selectedBook = cursorBook;
        }

        RefreshAll();
    }
    private void InfoChange(Spell SpellInfo)
    {
        if (SpellInfo)
        {
            infoImage.enabled = true;
            infoImage.sprite = SpellInfo.spellIcon;

            infoName.text = SpellInfo.spellName;
            infoDesc.text = SpellInfo.spellDescription;
            infoEffect.text = SpellInfo.spellEffect;    
        }
        else
        {
            infoImage.enabled =false;
            infoName.text = " ";
            infoDesc.text = " ";
            infoEffect.text = " ";
        }
    }
    public void RefreshAll()
    {
        for(int i = 0; i<books.Length; i++)
        {
            books[i].isEquip = false;
            if(spell.spells[books[i].BookID] != null)
            {
                if (spell.equipment[0] == spell.equipment[i]) books[i].isEquip = true;
                if (spell.equipment[1] == spell.equipment[i]) books[i].isEquip = true;
            }
            books[i].Refresh();

            if (books[i].isEquip) books[i].SetColor(cursorColor);
            else books[i].SetColor(myColor);
        }
        if (cursorBook && !cursorBook.isEquip) cursorBook.SetColor(cursorColor);

        if (selectedBook)
        {
            if (!selectedBook.isEquip) selectedBook.SetColor(selectColor);
            InfoChange(spell.spells[selectedBook.BookID]);
        }
        else InfoChange(null);


        for(int i = 0; i < equipBooks.Length ; i++)
        {
            equipBooks[i].Refresh();
        }
        ButtonSwitcher();
    }
    public void Cleaner()
    {
        cursorBook = null;
        selectedBook = null;
        RefreshAll();
    }
}
