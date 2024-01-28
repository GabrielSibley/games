using UnityEngine;
using System.Collections;

public class AIState<T> {

	public virtual void Update(T entity){
	}
	public virtual void OnStateEntered(T entity){		
	}
	public virtual void OnStateExited(T entity){		
	}
}
