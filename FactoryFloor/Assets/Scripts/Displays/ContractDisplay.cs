using UnityEngine;
using System.Collections;

[PrefabManager]
public class ContractDisplay : MonoBehaviour {

    public FourFeatureDisplay featureDisplay;
    public TextMesh quantityText;
    public SpriteRenderer typeDisplay;
    public Sprite supplyIcon;
    public Sprite deliveryIcon;


    public void Display(Contract contract)
    {
        featureDisplay.Display(contract.Crate.Features);
        quantityText.text = contract.Quantity.ToString();
        if(contract is SupplyContract)
        {
            typeDisplay.sprite = supplyIcon;
        }
        else
        {
            typeDisplay.sprite = deliveryIcon;
        }
    }

}
