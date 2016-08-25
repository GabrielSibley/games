using UnityEngine;
using System.Collections;

[PrefabManager]
public class SupplyContractDisplay : MonoBehaviour {

	public FourFeatureDisplay featureDisplay;
	public TextMesh priceText, quantityText; 

	public void UpdateDisplay(Vector3 position, SupplyContract contract)
	{
		transform.position = position;
		featureDisplay.Display(contract.Supplies.Features);
		priceText.text = "$" + contract.LotPrice;
		quantityText.text = contract.Quantity.ToString ();
	}
}
