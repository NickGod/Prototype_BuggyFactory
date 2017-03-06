using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button : MonoBehaviour {

    Transform[] _flowTestTrfs = new Transform[3];

    bool _isFlowTest = false;

    bool _isAssembed = false;
    bool _isFlowTestDone = false;
    public Transform _assembledModule;
    private Transform _singleTestModule;
    [SerializeField] private Text m_CurrentSuccessRate;
    [SerializeField] private Text m_SingleTestSuccessRate;

    Transform _assembledModuleCopy;
    float _assembledRate = 0;
    int _reachCount = 0;
    // Update is called once per frame
    void Update() {

        if (_isFlowTest) {
            if (!_isAssembed) {
                foreach (Transform trf in _flowTestTrfs) {
                    if (trf) {
                        trf.position += Vector3.forward * Time.deltaTime / 3.0f;
                    }
                }
                if (_reachCount == 3) {
                    Transform me_clone = Instantiate(_assembledModule.gameObject).transform;
                    me_clone.parent = null;
                    me_clone.position = _assembledModule.position;
                    me_clone.rotation = _assembledModule.rotation;
                    me_clone.localScale = _assembledModule.localScale;
                    me_clone.GetComponent<module>()._myType = _assembledModule.gameObject.GetComponent<module>()._myType;
                    me_clone.GetComponent<module>()._myrate = _assembledRate / 3.0f;
                    me_clone.gameObject.SetActive(true);
                    _assembledModuleCopy = me_clone;

                    _isAssembed = true;
                }
            } else if (!_isFlowTestDone) {
                _assembledModuleCopy.position += Vector3.right * Time.deltaTime / 3.0f;
            }
        }
    }

    public void SetFlowTest(bool tof) {
        _isFlowTest = tof;
        _assembledRate = 0.0f;
        if (!tof) {
            _isFlowTest = false;
            _isAssembed = false;
            _isFlowTestDone = false;
            _assembledRate = 0.0f;
            _reachCount = 0;
            for (int i = 0; i < 3; i++) {
                if (_flowTestTrfs[i]) {
                    Destroy(_flowTestTrfs[i].gameObject);
                    _flowTestTrfs[i] = null;
                }
            }
            if (_assembledModuleCopy) {
                Destroy(_assembledModuleCopy.gameObject);
                _assembledModuleCopy = null;
            }
            //TODO: erase the rate value on somethign to zero
        }
    }


    public void SetFlowTestTrfs(Transform[] trfs) {
        if (!_isFlowTest) {
            _isFlowTestDone = false;
            _assembledRate = 0.0f;
            int i = 0;
            foreach (Transform trf in trfs) {
                Transform trfson = trf.GetChild(0);
                Transform me_clone = Instantiate(trfson.gameObject).transform;
                me_clone.parent = null;
                me_clone.position = trfson.position;
                me_clone.rotation = trfson.rotation;
                me_clone.localScale = trfson.localScale;
                me_clone.GetComponent<module>()._myType = trf.gameObject.GetComponent<module>()._myType;
                me_clone.GetComponent<module>()._myrate = trf.gameObject.GetComponent<module>()._myrate;
                Transform _parent = trfson;
                while (_parent.parent != null) {
                    me_clone.localScale *= _parent.parent.localScale.x;
                    _parent = _parent.parent;
                }
                me_clone.gameObject.SetActive(true);
                if (_flowTestTrfs[i]) {
                    Destroy(_flowTestTrfs[i].gameObject);
                    _flowTestTrfs[i] = null;
                }
                if (_assembledModuleCopy) {
                    Destroy(_assembledModuleCopy.gameObject);
                    _assembledModuleCopy = null;
                }
                _flowTestTrfs[i++] = me_clone;
            }
        }
    }

    public void ReachAssemblySpot(Transform _reachedTrf) {
        if (_reachedTrf == _assembledModuleCopy) {
            if (_assembledModuleCopy) {
                Destroy(_assembledModuleCopy.gameObject);
                _assembledModuleCopy = null;
            }
            //TODO: show rate on something
            //rate = _assembledRate / 3.0f;
            float rate = _assembledRate / 3.0f;
            rate *= 100;
            // update the UI;
            m_CurrentSuccessRate.text = "Success Rate: " + rate.ToString() + "%";
            Debug.Log("Test: " + _assembledRate / 3.0f);
            _isFlowTestDone = true;
            _isFlowTest = false;
            _isAssembed = false;
            _reachCount = 0;
            for (int i = 0; i < 3; i++) {
                if (_flowTestTrfs[i]) {
                    Destroy(_flowTestTrfs[i].gameObject);
                    _flowTestTrfs[i] = null;
                }
            }
            if (_assembledModuleCopy) {
                Destroy(_assembledModuleCopy.gameObject);
                _assembledModuleCopy = null;
            }
        } else {
            for (int i = 0; i < 3; i++) {
                if (_flowTestTrfs[i] == _reachedTrf) {
                    _flowTestTrfs[i] = null;
                }
            }
            _assembledRate += _reachedTrf.GetComponent<module>()._myrate;
            Destroy(_reachedTrf.gameObject);
            _reachCount++;
        }
    }

    public void ShowSingleModuleRate(Transform _singleModule) {
        if (_singleModule == null) {
            m_SingleTestSuccessRate.text = "Module Test Failed... Try place an module onto testing field";
            return;
        }
        _singleTestModule = _singleModule;
        float rate = _singleModule.gameObject.GetComponent<module>().GetRate();
        if (rate > 0.0f) {
            //update single test UI accordingly...
            rate *= 100;
            // update the UI;
            m_SingleTestSuccessRate.text = "Module Success Rate: " + rate.ToString() + "%";
            Debug.Log("Single Test: " + rate);
        } else {
            m_SingleTestSuccessRate.text = "Module Test Failed...";
        }
    }

    public void PressButton() {
        gameObject.transform.position -= new Vector3(0, 0.015f, 0);
    }

    public void ReleaseButton() {
        gameObject.transform.position += new Vector3(0, 0.015f, 0);

    }
 }
