using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class module : MonoBehaviour {
    public enum ModuleType {
        type1,
        type2,
        type3
    };
    public enum SpotType {
        Module,
        SingleTest,
        FlowTest
    }
    public ModuleType _myType;
    public SpotType _mySpot = SpotType.Module;
    public Transform singleTest;
    public Transform[] flowTest = new Transform[3];
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform OnGrab() {
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
        return me_clone;
    }

    public Transform OnRelease(hand _playerHand) {
        transform.localScale = Vector3.one;
        if (transform.position.z < -0.2f) {
            _playerHand.GetOutOfList(gameObject.transform);
            Debug.Log("Goes to single test.");
            transform.position = singleTest.position;
            _mySpot = SpotType.SingleTest;
            return transform;
        } else {
            _playerHand.GetOutOfList(gameObject.transform);
            Debug.Log("Goes to flow test.");
            switch(_myType) {
                case ModuleType.type1:
                    transform.position = flowTest[0].position;
                    break;
                case ModuleType.type2:
                    transform.position = flowTest[1].position;
                    break;
                case ModuleType.type3:
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

    public void move_towards(Vector3 taret_pos) {
        transform.position += 0.05f * (taret_pos - transform.position);
    }
}
