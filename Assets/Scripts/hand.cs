using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hand : MonoBehaviour {
    //public Transform _rightHand;
    //static Transform _rightIndex;
    // accept three elements, first is inventory slot
    // second is demonstrate spot
    // third is orbitting slot


    static float _inventoryTimer = 0;
    static float _inventoryTime = 0.2f;
    static bool _isInventory = false;

    List<Transform> trfList = new List<Transform>();

    Transform _grabbedParent;
    Transform _grabbed;
        
    static Transform _singleTestTrf = null;
    static Transform[] _flowTestTrfs = new Transform[3];  

    
    //aiming and grabbing/pointing
    bool _isFist = false;
    bool _isGrabbing = false;
    
    //resizing
    //static float _originDistance = 0.0f;
    //static bool _onResizing = false;

    public bool isRightHand;
    public Transform _singleTestButton;
    public Transform _flowButton;
    // Update is called once per frame


    public Transform DefaultTire;
    public Transform DefaultDoor;
    public Transform DefaultBody;
    //right index finger
    //static bool isIndexFound = false;
    //LineRenderer lineRender;

    private bool TestButtonClicked = false;
    private bool SingleTestButtonClicked = false;
    void Start() {
        if (isRightHand) {
            _flowTestTrfs[0] = Instantiate(DefaultTire.gameObject).transform;
            _flowTestTrfs[1] = Instantiate(DefaultDoor.gameObject).transform;
            _flowTestTrfs[2] = Instantiate(DefaultBody.gameObject).transform;
            _flowTestTrfs[0].gameObject.SetActive(true);
            _flowTestTrfs[1].gameObject.SetActive(true);
            _flowTestTrfs[2].gameObject.SetActive(true);
            _flowTestTrfs[0].GetComponent<module>().SetMyPos(_flowTestTrfs[0]);
            _flowTestTrfs[1].GetComponent<module>().SetMyPos(_flowTestTrfs[1]);
            _flowTestTrfs[2].GetComponent<module>().SetMyPos(_flowTestTrfs[2]);
        }
    }

    void Update() {
        if (!isRightHand) {
            ////for left hand


            //transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            //transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);


            //calling inventory based on left hand index
            if (IsInventoryUp()) {
                _inventoryTimer += Time.deltaTime;
            } else {
                _inventoryTimer = 0.0f;
                if (_isInventory) {
                    //TODO: inventory off animation and related mehanics here
                    transform.GetChild(0).gameObject.SetActive(false);
                }
                _isInventory = false;
            }
            if (_inventoryTimer >= _inventoryTime && !_isInventory) {
                // this function only call once for each index up
                // there is up time for inventory to appear
                //TODO: inventory on animation and related mechanics here
                _isInventory = true;
                transform.GetChild(0).gameObject.SetActive(true);
            }
        } else {
            //for right hand
            //grabbing
            _isGrabbing = false;
            if (IsFist() && !_isFist) {
                _isFist = true;
                _isGrabbing = true;
            }
            if (_isGrabbing) {
                if (_grabbed == null) {
                    int count = trfList.Count;
                    for (int i = count - 1; i >= 0; i--) {
                        Transform trf = trfList[i];
                        if (trf == null || !trf.gameObject.activeSelf) {
                            trfList.Remove(trf);
                        }
                    }
                    Transform target = GetClosest();
                    if (target) {
                        _grabbed = target.GetComponent<module>().OnGrab();
                        if (!trfList.Contains(_grabbed)) {
                            trfList.Add(_grabbed);
                        }
                        if (_grabbed) {
                            _grabbedParent = null;
                            _grabbed.parent = transform;
                        }
                    }
                }
            }

            //grabbing release
            if (!IsFist()) {
                if (_grabbed) {
                    _grabbed.parent = _grabbedParent;
                    LoseControl();
                }
                _isFist = false;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == Tags.Grabbable && !trfList.Contains(other.transform)) {
            trfList.Add(other.transform);
        } else if (other.gameObject.tag == Tags.Button) {
            //TODO: button down anim
            Debug.Log("Click the button");
            other.gameObject.GetComponent<button>().PressButton();
            other.gameObject.GetComponent<button>().SetFlowTestTrfs(_flowTestTrfs);
            other.gameObject.GetComponent<button>().SetFlowTest(true);
        } else if (other.gameObject.tag == Tags.SingleTestButton) {
            Debug.Log("Click the single test button");
            other.gameObject.GetComponent<button>().PressButton();
            other.gameObject.GetComponent<button>().ShowSingleModuleRate(_singleTestTrf);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == Tags.Grabbable) {
            trfList.Remove(other.transform);
        } else if (other.gameObject.tag == Tags.Button || other.gameObject.tag == Tags.SingleTestButton) {
            other.gameObject.GetComponent<button>().ReleaseButton();
        }
    }

    Transform GetClosest() {
        Transform select = null;
        float _minDistance = float.MaxValue;
        foreach (var trf in trfList) {
            float dis = Vector3.Distance(trf.position, transform.position);
            if (dis < _minDistance) {
                _minDistance = dis;
                select = trf;
            }
        }
        return select;
    }

    bool IsInventoryUp() {
        if (!isRightHand) {
            return !(OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger) ||
                OVRInput.Get(OVRInput.Touch.PrimaryThumbRest) || OVRInput.Get(OVRInput.Touch.PrimaryThumbstick));
        } else {
            return false;
        }
    }

    bool IsFist() {
        OVRInput.Button indexFinger;
        OVRInput.Button middleFinger;

        bool thumb;
        if (isRightHand) {
            indexFinger = OVRInput.Button.SecondaryIndexTrigger;
            middleFinger = OVRInput.Button.SecondaryHandTrigger;
            thumb = OVRInput.Get(OVRInput.Touch.SecondaryThumbRest) || OVRInput.Get(OVRInput.Touch.One) || OVRInput.Get(OVRInput.Touch.Two);
        } else {
            indexFinger = OVRInput.Button.PrimaryIndexTrigger;
            middleFinger = OVRInput.Button.PrimaryHandTrigger;
            thumb = OVRInput.Get(OVRInput.Touch.PrimaryThumbRest) || OVRInput.Get(OVRInput.Touch.Three) || OVRInput.Get(OVRInput.Touch.Four);
        }
        bool index = OVRInput.Get(indexFinger);
        bool middle = OVRInput.Get(middleFinger);

        return index && middle && thumb;
    }

    public Transform GetGrabbedParent() {
        return _grabbedParent;
    }

    public void LoseControl() {
        if (_grabbed) {
            Transform _result = _grabbed.GetComponent<module>().OnRelease(this);
            //TODO: _singleTestTrf = result or  _flowTestTrfs[i] = result
            //TODO: destroy the stuff existing previously
            module _resultModule = _result.GetComponent<module>();
            if (_resultModule._mySpot == module.SpotType.SingleTest) {
                if (_singleTestTrf) {
                    Destroy(_singleTestTrf.gameObject);
                    _singleTestTrf = null;
                }
                _singleTestTrf = _result;
                //_singleTestButton.GetComponent<button>().ShowSingleModuleRate(_singleTestTrf);
                //TODO: update single test success rate on something
            } else if (_resultModule._mySpot == module.SpotType.FlowTest) {
                _flowButton.GetComponent<button>().SetFlowTest(false);
                switch(_resultModule._myType) {
                    case module.ModuleType.Tire:
                        if (_flowTestTrfs[0]) {
                            Destroy(_flowTestTrfs[0].gameObject);
                            _flowTestTrfs[0] = null;
                        }
                        _flowTestTrfs[0] = _result;
                        break;
                    case module.ModuleType.Door:
                        if (_flowTestTrfs[1]) {
                            Destroy(_flowTestTrfs[1].gameObject);
                            _flowTestTrfs[1] = null;
                        }
                        _flowTestTrfs[1] = _result;
                        break;
                    case module.ModuleType.Body:
                        if (_flowTestTrfs[2]) {
                            Destroy(_flowTestTrfs[2].gameObject);
                            _flowTestTrfs[2] = null;
                        }
                        _flowTestTrfs[2] = _result;
                        break;
                    default:
                        break;
                }
            }
            _grabbed = null;
            _grabbedParent = null;
        }
    }

    public void GetOutOfList(Transform trf) {
        if (trfList.Contains(trf)) {
            trfList.Remove(trf);
        }
    }
}
