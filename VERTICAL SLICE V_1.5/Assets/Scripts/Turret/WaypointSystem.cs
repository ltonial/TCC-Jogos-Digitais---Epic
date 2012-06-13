using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class WaypointSystem
{
    #region Attributes
    /// <summary>
    /// Transform do turret.
    /// </summary>
    private Transform _myTransform;
    /// <summary>
    /// Transform do target.
    /// </summary>
    private Transform _targetTransform;
    /// <summary>
    /// Lista de waypoints.
    /// </summary>
    private List<Waypoint> _waypointsList;
    /// <summary>
    /// Lista de computadores.
    /// </summary>
    private List<Computer> _computersList;
    /// <summary>
    /// 
    /// </summary>
    private Computer _nearComputer;
    private bool _foundComputer;
    /// <summary>
    /// 
    /// </summary>
    private Waypoint _nearWaypoint;
    private bool _foundWaypoint;
    /// <summary>
    /// Últimos waypoints percorridos.
    /// </summary>
    private Queue<Waypoint> _foundWaypointsQueue;
    #endregion
    #region Constructos
    public WaypointSystem(Transform pMyTransform, Transform pTargetTransform)
    {
        this._myTransform = pMyTransform;
        this._targetTransform = pTargetTransform;
        this._waypointsList = new List<Waypoint>();
        this._foundWaypointsQueue = new Queue<Waypoint>();
        this._computersList = new List<Computer>();

        // Adicionando os waypoints.
        GameObject[] auxWaypointsObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        if (auxWaypointsObjects != null)
        {
            foreach (GameObject item in auxWaypointsObjects)
            {
                int id = item.GetComponent<ItemIdentificator>().ID;
                this._waypointsList.Add(new Waypoint(id, item, WaypointType.POINT));
            }
        }

        // Adicionando os computadores.
        GameObject[] auxComputersObjects = GameObject.FindGameObjectsWithTag("Computer");
        if (auxComputersObjects != null)
        {
            foreach (GameObject item in auxComputersObjects)
            {
                int id = item.GetComponent<ItemIdentificator>().ID;
                this._computersList.Add(new Computer(id, item, ComputerType.ALARM));
            }
        }
    }
    #endregion
    #region Properties
    public Computer NearComputer
    {
        get { return this._nearComputer; }
    }
    public Waypoint NearWaypoint
    {
        get { return this._nearWaypoint; }
    }
    #endregion
    #region Methods (Public)
    /// <summary>
    /// Persegue um waypoint.
    /// </summary>
    public GameObject Patrol()
    {
        this._foundWaypoint = Vector3.Distance(this._myTransform.position, this._nearWaypoint.GameObject.transform.position) < 1f;
        //if (this._foundWaypoint) this._nearWaypoint = this.FindNearWaypoint(this._nearWaypoint);
        if (this._foundWaypoint) this._nearWaypoint = this.FindNearWaypoint();

        return this._nearWaypoint.GameObject;
    }
    /// <summary>
    /// Persegue um waypoint ou o target.
    /// </summary>
    public GameObject Chase()
    {
        GameObject auxWaypoint = null;
        if (Physics.Linecast(this._myTransform.position, this._targetTransform.position))
            auxWaypoint = this.FindBetterWayToChase();

        if (auxWaypoint == null) auxWaypoint = GameObject.FindGameObjectWithTag("Player");
        return auxWaypoint;
    }
    /// <summary>
    /// Persegue um waypoint até um computador.
    /// </summary>
    public GameObject Alert(out bool foundComputer)
    {
        // Se chegar perto do computador, fica parado.
        this._foundComputer = Vector3.Distance(this._myTransform.position, this._nearComputer.GameObject.transform.position) < 5f;
        if (this._foundComputer)
        {
            foundComputer = true;
            return this._myTransform.gameObject;
        }
        // Senão faz uma busca pelo melhor caminho.
        else
        {
            GameObject auxWaypoint = null;
            if (Physics.Linecast(this._myTransform.position, this._nearComputer.GameObject.transform.position))
                auxWaypoint = this.FindBetterWayToAlert();

            if (auxWaypoint == null) auxWaypoint = this._nearComputer.GameObject;

            foundComputer = false;
            return auxWaypoint;
        }
    }
    /// <summary>
    /// Procura waypoint mais próximo.
    /// </summary>
    public void UpdateWaypoint()
    {
        //this._nearWaypoint = this.FindNearWaypoint(this._nearWaypoint);
        this._nearWaypoint = this.FindNearWaypoint();
        // Se a fila estiver com mais de 3 waypoints, libera os primieros incluídos.
        if (this._foundWaypointsQueue.Count > 3) this._foundWaypointsQueue.Dequeue();
    }
    /// <summary>
    /// Procura computador mais próximo.
    /// </summary>
    public void UpdateComputer()
    {
        this._nearComputer = this.FindNearComputer();
    }
    #endregion
    #region Methods (Private)
    /// <summary>
    /// Procura o melhor caminho na perseguição.
    /// </summary>
    private GameObject FindBetterWayToChase()
    {
        GameObject auxBetterWay = null;
        float auxDistanceToBetterWay = Mathf.Infinity;

        foreach (Waypoint item in this._waypointsList)
        {
            // Distância do waypoint até o turret.
            float auxDistanceToWaypoint = Vector3.Distance(item.GameObject.transform.position, this._myTransform.position);
            // Distância do waypoint até o target.
            float auxDistanceWaypointToTarget = Vector3.Distance(item.GameObject.transform.position, this._targetTransform.position);
            // Distância do target até o turret.
            float auxDistanceToTarget = Vector3.Distance(this._targetTransform.position, this._myTransform.position);
            // Verifica se há uma parede no caminho.
            bool auxWallBetween = Physics.Linecast(item.GameObject.transform.position, this._myTransform.position);

            // Se não há parede no caminho, 
            // se a distância até o waypoint for menor que a melhor distância e
            // se a distância até o target for maior que a distância do waypoint até o target.
            if (!auxWallBetween && (auxDistanceToWaypoint < auxDistanceToBetterWay) && (auxDistanceToTarget > auxDistanceWaypointToTarget))
            {
                auxDistanceToBetterWay = auxDistanceToWaypoint;
                auxBetterWay = item.GameObject;
            }
            else
            {
                bool auxWaypointToTargetCollision = Physics.Linecast(item.GameObject.transform.position, this._targetTransform.position);
                if (!auxWaypointToTargetCollision) auxBetterWay = item.GameObject;
            }
        }
        //if (auxBetterWay == null) auxBetterWay = this._nearWaypoint.GameObject;
        return auxBetterWay;
    }
    /// <summary>
    /// Procura o melhor caminho para o alerta.
    /// </summary>
    private GameObject FindBetterWayToAlert()
    {
        GameObject auxBetterWay = null;
        float auxDistanceToBetterWay = Mathf.Infinity;
        foreach (Waypoint item in this._waypointsList)
        {
            // Distância do waypoint até o turret.
            float auxDistanceToWaypoint = Vector3.Distance(item.GameObject.transform.position, this._myTransform.position);
            // Distância do waypoint até o computador.
            float auxDistanceWaypointToComputer = Vector3.Distance(item.GameObject.transform.position, this._nearComputer.GameObject.transform.position);
            // Distância do computador até o turret.
            float auxDistanceToComputer = Vector3.Distance(this._nearComputer.GameObject.transform.position, this._myTransform.position);
            // Verifica se há uma parede no caminho.
            //LayerMask mask = 15;
            //Debug.Log("LayerMask: " + LayerMask.LayerToName(mask.value));
            bool auxWallBetween = Physics.Linecast(item.GameObject.transform.position, this._myTransform.position);//, mask);

            // Se não há parede no caminho, 
            // se a distância até o waypoint for menor que a melhor distância e
            // se a distância até o computador for maior que a distância do waypoint até o target.
            if (!auxWallBetween && (auxDistanceToWaypoint < auxDistanceToBetterWay) && (auxDistanceToComputer > auxDistanceWaypointToComputer))
            {
                auxDistanceToBetterWay = auxDistanceToWaypoint;
                auxBetterWay = item.GameObject;
            }
            else
            {
                bool auxWaypointToTargetCollision = Physics.Linecast(item.GameObject.transform.position, this._nearComputer.GameObject.transform.position);
                if (!auxWaypointToTargetCollision) auxBetterWay = item.GameObject;
            }
        }
        return auxBetterWay;
    }
    /// <summary>
    /// Finds the near waypoint.
    /// </summary>
    //private Waypoint FindNearWaypoint(Waypoint pWaypoint = null)
    private Waypoint FindNearWaypoint()
    {
        Waypoint auxNearWaypoint = null;
        if (this._waypointsList.Count > 0)
        {
            //List<Waypoint> auxList = new List<Waypoint>(this._waypointsList);
            //if (pWaypoint != null) auxList.Remove(pWaypoint);
            List<Waypoint> auxList = this.GetAvailableWaypoints();

            auxNearWaypoint = auxList[0];
            float auxDistanceToBetterWay = Mathf.Infinity;
            foreach (Waypoint item in auxList)
            {
                // Distância do waypoint até o turret.
                float auxDistanceToWaypoint = Vector3.Distance(item.GameObject.transform.position, this._myTransform.position);

                // Se a distância até o waypoint for menor que a melhor distância
                if (auxDistanceToWaypoint < auxDistanceToBetterWay)
                {
                    auxDistanceToBetterWay = auxDistanceToWaypoint;
                    auxNearWaypoint = item;
                }
            }
        }
        else Debug.LogError("NÃO HÁ WAYPOINTS NO CENÁRIO!");

        this._foundWaypoint = false;
        this._foundWaypointsQueue.Enqueue(auxNearWaypoint);
        return auxNearWaypoint;
    }
    /// <summary>
    /// Finds the near computer.
    /// </summary>
    //private Computer FindNearComputer(Computer pComputer = null)
    private Computer FindNearComputer()
    {
        Computer auxNearComputer = null;
        if (this._computersList.Count > 0)
        {
            //List<Computer> auxList = new List<Computer>(this._computersList);
            //if (pComputer != null) auxList.Remove(pComputer);
            List<Computer> auxList = this._computersList;

            auxNearComputer = auxList[0];
            float auxDistanceToBetterWay = Mathf.Infinity;
            foreach (Computer item in auxList)
            {
                // Distância do waypoint até o turret.
                float auxDistanceToComputer = Vector3.Distance(item.GameObject.transform.position, this._myTransform.position);

                // Se não há parede no caminho, 
                // se a distância até o waypoint for menor que a melhor distância
                if (auxDistanceToComputer < auxDistanceToBetterWay)
                {
                    auxDistanceToBetterWay = auxDistanceToComputer;
                    auxNearComputer = item;
                }
            }
        }
        else Debug.LogError("NÃO HÁ COMPUTADORES NO CENÁRIO!");

        this._foundComputer = false;
        return auxNearComputer;
    }
    /// <summary>
    /// Recupera os waypoints que não estahem an fila.
    /// </summary>
    /// <returns></returns>
    private List<Waypoint> GetAvailableWaypoints()
    {
        return this._waypointsList.FindAll(delegate(Waypoint way) { return !this._foundWaypointsQueue.Contains(way); });
    }
    #endregion
}
