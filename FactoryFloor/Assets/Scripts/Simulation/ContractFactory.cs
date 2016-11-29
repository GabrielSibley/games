using UnityEngine;
using System.Collections;

public class ContractFactory {

    private static float[] sizeWeights = new float[]{
        2, //size 1
		5, //size 2
		4, //size 3
		3, //size 4
	};

    private bool isDelivery;

    public ContractFactory(bool isDelivery)
    {
        this.isDelivery = isDelivery;
    }

    public Contract GetContractForShop()
    {
        Contract c;
        if(isDelivery)
        {
            c = new DeliveryContract();
        }
        else
        {
            c = new SupplyContract();
        }
        c.Crate = new Crate();
        int numFeatures = RandomEx.RandomIndexWeighted(sizeWeights) + 1;
        for(int i = 0; i < numFeatures; i++)
        {
            c.Crate.Features.Add(CrateFeature.RandomNonWild());
        }

        c.Quantity = Random.Range(1,20)*50;

        return c;
    }

}
