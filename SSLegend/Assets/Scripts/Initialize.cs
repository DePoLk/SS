using UnityEngine;
using UnityEngine.Events;

public class Initialize : MonoBehaviour {

	[SerializeField] private UnityEvent onStart;


	void Start(){
		this.onStart.Invoke();
	}
}
