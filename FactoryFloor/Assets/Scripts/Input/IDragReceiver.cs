using UnityEngine;
using System.Collections;

public interface IDragReceiver {

	bool Drop(IDraggable dropped);
}
