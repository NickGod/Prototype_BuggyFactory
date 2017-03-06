using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class module : MonoBehaviour {
    public enum ModuleType {
        Tire,
        Door,
        Body
    };
    public enum SpotType {
        Module,
        SingleTest,
        FlowTest
    }
    public float _myrate;

    private Transform FlowButton;
    private Transform SingleTestButton;

    Vector3 originSize = Vector3.one * 8;

    public ModuleType _myType;
    public SpotType _mySpot = SpotType.Module;
    public Transform singleTest;
    public Transform[] flowTest = new Transform[3];
	// Use this for initialization
	void Start () {
        FlowButton = GameObject.FindWithTag("Button").transform;
        SingleTestButton = GameObject.FindWithTag("SingleTestButton").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == Tags.AssembleSpot) {
            FlowButton.GetComponent<button>().ReachAssemblySpot(transform);
        }
    }

    public Transform OnGrab() {
        // clone this game object so you can actually grab
        Transform me_clone = Instantiate(gameObject).transform;
        me_clone.parent = null;
        me_clone.position = transform.position;
        me_clone.rotation = transform.rotation;
        Transform _parent = transform;
        //multiply parents' scale
        while (_parent.parent != null) {
            me_clone.localScale *= _parent.parent.localScale.x;
            _parent = _parent.parent;
        }
        //initialize attribute value
        me_clone.GetComponent<module>()._myType = _myType;
        me_clone.GetComponent<module>()._myrate = _myrate;
        return me_clone;
    }

    public Transform OnRelease(hand _playerHand) {
        transform.localScale = originSize;
        transform.rotation = Quaternion.identity;
        if (transform.position.z < 0.0f) {
            // delete from hand
            _playerHand.GetOutOfList(gameObject.transform);
            // attach to single test position
            transform.position = singleTest.position;
            _mySpot = SpotType.SingleTest;

            return transform;
        } else {
            _playerHand.GetOutOfList(gameObject.transform);
            Debug.Log("Goes to flow test.");
            switch(_myType) {
                case ModuleType.Tire:
                    transform.position = flowTest[0].position;
                    break;
                case ModuleType.Door:
                    transform.position = flowTest[1].position;
                    break;
                case ModuleType.Body:
                    transform.position = flowTest[2].position;
                    break;
                default:
                    Debug.Log("Weird, shoundn't be default");
                    break;
            }
            _mySpot = SpotType.FlowTest;
            return transform;
        }
    }

    public void SetMyPos(Transform trf) {
        ModuleType inType = trf.GetComponent<module>()._myType;
        if (inType == ModuleType.Tire) {
            trf.position = flowTest[0].position;
        } else if (inType == ModuleType.Door) {
            trf.position = flowTest[1].position;
        } else if (inType == ModuleType.Body) {
            trf.position = flowTest[2].position;
        }
    }

    public void move_towards(Vector3 taret_pos) {
        transform.position += 0.05f * (taret_pos - transform.position);
    }

    public float GetRate() {
        return _myrate;
    }
}
