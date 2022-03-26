using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{

    private PlayerController selected = null;

#region Healthbars
    // Warrior
    public Entity Warrior;
    public Image W_Healthbar;

    // Rogue
    public Entity Rogue;
    public Image R_Healthbar;

    // Mage
    public Entity Mage;
    public Image M_Healthbar;

    // Priest
    public Entity Priest;
    public Image P_Healthbar;

    // Archer
    public Entity Archer;
    public Image A_Healthbar;
#endregion

#region Abilities

    public Image A1_Icon, A1_CD;
    public Text A1_Name;
    public Image A2_Icon, A2_CD;
    public Text A2_Name;
    public Image A3_Icon, A3_CD;
    public Text A3_Name;
    public Image A4_Icon, A4_CD;
    public Text A4_Name;

#endregion

#region Room Data

    public GameObject InteractablesListBase;
    public GameObject InteractablesListParent;
    public Text InteractablesList;
    public GameObject EnemiesListBase;
    public GameObject EnemiesListParent;
    public Text EnemiesList;


#endregion


    public void Initialize()
    {
        if (Warrior != null) {
            Warrior.onDamageReceived += UpdateWarrior;
        }

        if (Rogue != null) {
            Rogue.onDamageReceived += UpdateRogue;
        }

        if (Mage != null) {
            Mage.onDamageReceived += UpdateMage;
        }

        if (Priest != null) {
            Priest.onDamageReceived += UpdatePriest;
        }

        if (Archer != null) {
            Archer.onDamageReceived += UpdateArcher;
        }
    }

    private void Update()
    {   
        // Update Ability CDs
        A1_CD.fillAmount = selected.GetCharacter().Abilities["A1"].currCD / selected.GetCharacter().Abilities["A1"].CD;
        A2_CD.fillAmount = selected.GetCharacter().Abilities["A2"].currCD / selected.GetCharacter().Abilities["A2"].CD;
        A3_CD.fillAmount = selected.GetCharacter().Abilities["A3"].currCD / selected.GetCharacter().Abilities["A3"].CD;
        A4_CD.fillAmount = selected.GetCharacter().Abilities["A4"].currCD / selected.GetCharacter().Abilities["A4"].CD;                
    }

    public void UpdateRoomData()
    {   
        // Dont want to update if we dont have a selection or a room to update.
        if (selected == null || selected.GetRoom() == null) return;

        // Update Interactables List
        if (selected.GetRoom().ObjectsInRoom.Count > 0) {
            if (!InteractablesListBase.activeInHierarchy) {
                InteractablesListBase.SetActive(true);
                InteractablesListParent.SetActive(true);
            }

            InteractablesList.text = "";
            foreach (GameObject obj in selected.GetRoom().ObjectsInRoom)
            {
                InteractablesList.text += obj.name + "\n";
            }

            Canvas.ForceUpdateCanvases();
            InteractablesListParent.GetComponent<ContentSizeFitter>().enabled = false;
            InteractablesListParent.GetComponent<ContentSizeFitter>().enabled = true;
        } else {
            InteractablesListBase.SetActive(false);
            InteractablesListParent.SetActive(false);
        }
        
        // Update Enemies List
        if (selected.GetRoom().EnemiesInRoom.Count > 0) {
            if (!EnemiesListBase.activeInHierarchy) {
                EnemiesListBase.SetActive(true);
                EnemiesListParent.SetActive(true);
            }

            EnemiesList.text = "";
            foreach (GameObject obj in selected.GetRoom().EnemiesInRoom)
            {
                EnemiesList.text += obj.name + "\n";
            }

            Canvas.ForceUpdateCanvases();
            EnemiesListParent.GetComponent<ContentSizeFitter>().enabled = false;
            EnemiesListParent.GetComponent<ContentSizeFitter>().enabled = true;
        } else {
            EnemiesListBase.SetActive(false);
            EnemiesListParent.SetActive(false);
        }
        
        
        
        
        // Clear the list
        // foreach (Transform child in EnemiesListParent)
        // {
        //     GameObject.Destroy(child.gameObject);
        // }

        // // // Populate the list
        // foreach (GameObject obj in selected.GetRoom().EnemiesInRoom)
        // {
        //     GameObject textObj = Instantiate(textPrefab, Vector3.zero, Quaternion.identity);
        //     textObj.transform.SetParent(EnemiesListParent);
        //     textObj.GetComponent<Text>().text = obj.name;
        // }
    }

    public void SelectCharacter(PlayerController pc)
    {
        selected = pc;

        // Update Ability Bar Icons
        A1_Icon.sprite = pc.GetCharacter().Icons["A1"];
        A2_Icon.sprite = pc.GetCharacter().Icons["A2"];
        A3_Icon.sprite = pc.GetCharacter().Icons["A3"];
        A4_Icon.sprite = pc.GetCharacter().Icons["A4"];

        // Update Ability Bar Names        
        A1_Name.text = pc.GetCharacter().Abilities["A1"].Name;
        A2_Name.text = pc.GetCharacter().Abilities["A2"].Name;
        A3_Name.text = pc.GetCharacter().Abilities["A3"].Name;
        A4_Name.text = pc.GetCharacter().Abilities["A4"].Name;

        // Update Ability CDs
        A1_CD.fillAmount = pc.GetCharacter().Abilities["A1"].currCD / pc.GetCharacter().Abilities["A1"].CD;
        A2_CD.fillAmount = pc.GetCharacter().Abilities["A2"].currCD / pc.GetCharacter().Abilities["A2"].CD;
        A3_CD.fillAmount = pc.GetCharacter().Abilities["A3"].currCD / pc.GetCharacter().Abilities["A3"].CD;
        A4_CD.fillAmount = pc.GetCharacter().Abilities["A4"].currCD / pc.GetCharacter().Abilities["A4"].CD;

    }

    private void UpdateWarrior()
    {
        W_Healthbar.fillAmount = Warrior.GetCurrentHealth() / Warrior.GetMaxHealth();
    }

    private void UpdateRogue()
    {
        R_Healthbar.fillAmount = Rogue.GetCurrentHealth() / Rogue.GetMaxHealth();
    }

    private void UpdateMage()
    {
        Debug.Log("Mage Health Bar Updated!");
        M_Healthbar.fillAmount = Mage.GetCurrentHealth() / Mage.GetMaxHealth();
    }

    private void UpdatePriest()
    {
        P_Healthbar.fillAmount = Priest.GetCurrentHealth() / Priest.GetMaxHealth();
    }

    private void UpdateArcher()
    {
        A_Healthbar.fillAmount = Archer.GetCurrentHealth() / Archer.GetMaxHealth();
    }

    private void UpdateAbilityBar()
    {

    }
}
