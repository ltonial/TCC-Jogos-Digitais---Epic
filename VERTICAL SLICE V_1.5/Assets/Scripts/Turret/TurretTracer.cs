using UnityEngine;

public class TurretTracer
{
    #region Attributes
    /// <summary>
    /// Objeto transform do turret.
    /// </summary>
    public Transform _myTransform;
    /// <summary>
    /// Linha que renderiza o raycast.
    /// </summary>
    private LineRenderer _lr;
    /// <summary>
    /// O hit do raycast.
    /// </summary>
    public RaycastHit _hit;
    /// <summary>
    /// O ray.
    /// </summary>
    public Ray _ray;
    /// <summary>
    /// A origem, ou seja, posi��o inicial do ray.
    /// </summary>
    public Vector3 _rayOrigin;
    /// <summary>
    /// Quem colidir� com o ray (usado para comportamento do turret).
    /// </summary>
    public Collider _rayCollider;
    /// <summary>
    /// Se o ray tocou algo.
    /// </summary>
    public bool _hithingSomething;
    /// <summary>
    /// Dist�ncia de 'vis�o' do turret.
    /// </summary>
    private float _distanceRay;
    #endregion
    #region Constructors
    /// <summary>
    /// Inicializando o raycast do turret.
    /// </summary>
    public TurretTracer(Transform pMyTransform)
    {
        this._myTransform = pMyTransform;
        this._distanceRay = 10f;
        this._hithingSomething = false;
        this._rayOrigin = this._myTransform.position;
        this._rayCollider = new Collider();
        this._lr = this._myTransform.GetComponent<LineRenderer>();
    }
    #endregion
    #region Properties
    /// <summary>
    /// Verifica se est� fazendo um hit sobre o target.
    /// </summary>
    public bool HithingTarget
    {
        get
        {
            return this._rayCollider != null && 
                this._rayCollider.gameObject != null &&
                this._rayCollider.gameObject.tag != null && 
                this._rayCollider.gameObject.tag == "Player" 
                && this._hithingSomething;
        }
    }
    #endregion
    #region Methods
    /// <summary>
    /// Atualizando o processo de raycast.
    /// </summary>
    public void Update(bool pEnable)
    {
		this._lr.enabled = pEnable;
		
        this.ProcessRaycast();
        this.DrawTracer();
    }
    /// <summary>
    /// Faz o processamento do raycast
    /// </summary>
    private void ProcessRaycast()
    {
        // Move o ray para onde est� o objeto.
        this._rayOrigin = this._myTransform.position;

        // Criando o ray
        Vector3 auxTransformFoward = this._myTransform.forward;
        auxTransformFoward.y = 0f;
        this._ray = new Ray(this._rayOrigin, auxTransformFoward);

        if (!Physics.Raycast(this._ray, out this._hit))
        {
            this._hithingSomething = false;
            return;
        }
        else if (Physics.Raycast(this._ray, out this._hit))//, this._distanceRay))
        {
            // Guarda o objeto que fez o hit
            this._rayCollider = this._hit.collider;
            this._hithingSomething = true;
        }
    }
    /// <summary>
    /// Redesenha o tracer
    /// </summary>
    private void DrawTracer()
    {
        this._lr.SetPosition(0, this._rayOrigin);
        this._lr.SetPosition(1, this._hit.point);

        Color startColor = Color.red;
        Color endColor = Color.white;
        startColor.a = Random.Range(0f, 1f);
        endColor.a = Random.Range(0f, 1f);

        this._lr.SetColors(startColor, endColor);
    }
    #endregion
}
