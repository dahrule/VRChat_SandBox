
using UnityEngine;


public class HandleController : MonoBehaviour
{
    [SerializeField] Transform target;

    [Tooltip("The rotation Point")]
    [SerializeField] Transform handlePivot;

    private Transform hand;

    // Start is called before the first frame update
    void Start()
    {
        //align target with handle
        //target.parent = handlePivot;
    }

    // Update is called once per frame
    void Update()
    {
        if(hand) 
        {

            target.position = hand.position;
            target.localPosition = new Vector3(target.localPosition.x,0f, target.localPosition.z);

            Vector3 direction = target.position - handlePivot.position;
            Quaternion newRotation = Quaternion.LookRotation(direction,handlePivot.up);
            handlePivot.rotation = newRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hand = other.transform;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hand = null;
        }
    }
}
