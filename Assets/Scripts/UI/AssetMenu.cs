using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetMenu : MonoBehaviour
{
    public GameObject buildingholder;
    public GameObject buildingholderSizes;
    public GameObject buildingholderOpenButton;
    public GameObject buildingholderCloseButton;

    // Start is called before the first frame update
    void Start()
    {
        buildingholderOpenButton.GetComponent<Button>().onClick.AddListener(BuildingClicked);
        buildingholderCloseButton.GetComponent<Button>().onClick.AddListener(CloseBuildingHolderSizes);
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    // When a building in building menu is clicked, inactivate buildingholder and active buildingholderSizes
    private void BuildingClicked()
    {
        buildingholderSizes.SetActive(true);
        buildingholder.SetActive(false);
    }
     
    // when the closebutton in buildingholdersizes is clicked, inactivate buildingholdersizes and active buildingholder
    public void CloseBuildingHolderSizes()
    {
        buildingholderSizes.SetActive(false);
        buildingholder.SetActive(true);
    }
}