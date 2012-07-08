using UnityEngine;
using System.Collections;

public class SwitchCameras : MonoBehaviour
{

    #region Attributes
    private FadeCamera _scriptFade;
    private GameObject _cameraPit;
    private GameObject _cameraStairsLeft;
    private GameObject _cameraStairsRight;
    private GameObject _cameraRing;
    public static bool _activeCameraPit = false;
    public static bool _activeCameraStairsLeft = false;
    public static bool _activeCameraStairsRight = false;
    public static bool _activeCameraRing = false;
    private bool _canFade;
    private float _timeToShowCamera;
    private const float TIMETOSHOWCAMERA = 10.0f;
    private const float TIME_FADE = 2.0f;
    private bool _cameraPitStoppedGame;
    private bool _cameraStairsLeftStoppedGame;
    private bool _cameraStairsRightStoppedGame;
    private bool _cameraRingStoppedGame;
    private GameObject _player;
    private GameObject[] _turrets;
    private GameObject[] _humanoids;

    #endregion

    #region Methods(Inherit)
    void Start()
    {
        this._player = GameObject.FindWithTag("Player");
        this._scriptFade = GetComponent<FadeCamera>();
        this._cameraPit = GameObject.Find("Camera-Pit");
        this._cameraStairsLeft = GameObject.Find("Camera-Stairs-Left");
        this._cameraStairsRight = GameObject.Find("Camera-Stairs-Right");
        this._cameraRing = GameObject.Find("Camera-Ring");
        this._timeToShowCamera = TIMETOSHOWCAMERA;
        this._cameraPit.camera.enabled = false;
        this._cameraStairsLeft.camera.enabled = false;
        this._cameraStairsRight.camera.enabled = false;
        this._cameraRing.camera.enabled = false;
        this._canFade = false;
        this._cameraPitStoppedGame = false;
        this._cameraStairsLeftStoppedGame = false;
        this._cameraStairsRightStoppedGame = false;
    }

    void Update()
    {
        this._turrets = GameObject.FindGameObjectsWithTag("Turret");
        this._humanoids = GameObject.FindGameObjectsWithTag("Humanoid");
        this._timeToShowCamera -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F10)) _activeCameraPit = true;
        if (Input.GetKeyDown(KeyCode.F11)) _activeCameraStairsLeft = true;
        if (Input.GetKeyDown(KeyCode.F12)) _activeCameraRing = true;

        StartCoroutine(ChangeToCameraPit());
        StartCoroutine(ChangeToCameraStairsLeft());
        StartCoroutine(ChangeToCameraStairsRight());
        StartCoroutine(ChangeToCameraRing());
    }

    #endregion

    #region Methods(Classes)
    private IEnumerator ChangeToCameraPit()
    {
        if (_activeCameraPit)
        {
            if (!_cameraPitStoppedGame)
            {
                EnabledDisableScripts();
                this._cameraPitStoppedGame = true;
            }
            this._scriptFade.StartCoroutine(_scriptFade.FadeInOut(TIME_FADE));
            yield return new WaitForSeconds(TIME_FADE);
            this.camera.enabled = false;
            this._cameraPit.camera.enabled = true;
            _activeCameraPit = false;
            this._canFade = true;
        }
        else if (_timeToShowCamera < 0.0f)
        {
            if (_canFade)           //Senão ele fica dando fade a cada 5 seg
            {
                this._scriptFade.StartCoroutine(_scriptFade.FadeInOut(TIME_FADE));
                _canFade = false;
            }
            yield return new WaitForSeconds(TIME_FADE);
            if (_cameraPitStoppedGame)
            {
                this._cameraPit.camera.enabled = false;
                this.camera.enabled = true;
                this._cameraPitStoppedGame = false;
                EnabledDisableScripts();
            }
            _timeToShowCamera = TIMETOSHOWCAMERA;
        }

    }

    private IEnumerator ChangeToCameraStairsLeft()
    {
        if (_activeCameraStairsLeft)
        {
            if (!_cameraStairsLeftStoppedGame)
            {
                EnabledDisableScripts();
                this._cameraStairsLeftStoppedGame = true;
            }
            this._scriptFade.StartCoroutine(_scriptFade.FadeInOut(TIME_FADE));
            yield return new WaitForSeconds(TIME_FADE);
            this.camera.enabled = false;
            this._cameraStairsLeft.camera.enabled = true;
            _activeCameraStairsLeft = false;
            this._canFade = true;
        }
        else if (_timeToShowCamera < 0.0f)
        {
            if (_canFade)           //Senão ele fica dando fade a cada 5 seg
            {
                this._scriptFade.StartCoroutine(_scriptFade.FadeInOut(TIME_FADE));
                _canFade = false;
            }
            yield return new WaitForSeconds(TIME_FADE);
            if (_cameraStairsLeftStoppedGame)
            {
                this._cameraStairsLeft.camera.enabled = false;
                //Debug.Log("Camera LEft");
                _activeCameraStairsRight = true;
                this._cameraStairsLeftStoppedGame = false;
                EnabledDisableScripts();
                
            }
            _timeToShowCamera = TIMETOSHOWCAMERA;
        }

    }

    private IEnumerator ChangeToCameraStairsRight()
    {
        if (_activeCameraStairsRight)
        {
            if (!_cameraStairsRightStoppedGame)
            {
                EnabledDisableScripts();
                this._cameraStairsRightStoppedGame = true;
            }
            this._scriptFade.StartCoroutine(_scriptFade.FadeInOut(TIME_FADE));
            yield return new WaitForSeconds(TIME_FADE);
            this.camera.enabled = false;
            this._cameraStairsRight.camera.enabled = true;
            _activeCameraStairsRight = false;
            this._canFade = true;
        }
        else if (_timeToShowCamera < 0.0f)
        {
            if (_canFade)           //Senão ele fica dando fade a cada 5 seg
            {
                this._scriptFade.StartCoroutine(_scriptFade.FadeInOut(TIME_FADE));
                _canFade = false;
            }
            yield return new WaitForSeconds(TIME_FADE);
            if (_cameraStairsRightStoppedGame)
            {
                this._cameraStairsRight.camera.enabled = false;
                this.camera.enabled = true;
                //Debug.Log("Camera Right");
                this._cameraStairsRightStoppedGame = false;
                EnabledDisableScripts();
            }
            _timeToShowCamera = TIMETOSHOWCAMERA;
        }

    }

    private IEnumerator ChangeToCameraRing()
    {
        if (_activeCameraRing)
        {
            if (!_cameraRingStoppedGame)
            {
                EnabledDisableScripts();
                this._cameraRingStoppedGame = true;
            }
            this._scriptFade.StartCoroutine(_scriptFade.FadeInOut(TIME_FADE));
            yield return new WaitForSeconds(TIME_FADE);
            this.camera.enabled = false;
            this._cameraRing.camera.enabled = true;
            _activeCameraRing = false;
            this._canFade = true;
        }
        else if (_timeToShowCamera < 0.0f)
        {
            if (_canFade)           //Senão ele fica dando fade a cada 5 seg
            {
                this._scriptFade.StartCoroutine(_scriptFade.FadeInOut(TIME_FADE));
                _canFade = false;
            }
            yield return new WaitForSeconds(TIME_FADE);
            if (_cameraRingStoppedGame)
            {
                this._cameraRing.camera.enabled = false;
                this.camera.enabled = true;
                this._cameraRingStoppedGame = false;
                EnabledDisableScripts();
            }
            _timeToShowCamera = TIMETOSHOWCAMERA;
        }

    }

    private void EnabledDisableScripts()
    {
        gameObject.GetComponent<CameraAction>().enabled = !gameObject.GetComponent<CameraAction>().enabled;
        MonoBehaviour[] componentsPlayer = _player.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in componentsPlayer)
        {
            component.enabled = !component.enabled;
        }

        foreach (GameObject turret in _turrets)
        {
            MonoBehaviour[] componentsTurret = turret.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in componentsTurret)
            {
                component.enabled = !component.enabled;
            }
        }

        foreach (GameObject humanoid in _humanoids)
        {
            MonoBehaviour[] componentsHumanoid = humanoid.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in componentsHumanoid)
            {
                component.enabled = !component.enabled;
            }
        }
    }
    #endregion
}
