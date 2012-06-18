using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ComputerManager : MonoBehaviour {
	
	#region Constants
	private const int LIMITTOSPAWN = 2;
	private const float SPAWNTIME = 30f;
	#endregion
	#region Attributes
	private bool _wasHacked;
	private bool _wasActivatedSpawn;
	private float _timeToSpawn;
	private int _totalSpawn;
	private GameObject _enemyObject;
	private Transform _spawnTransform;
	private List<HumanoidManager> _humanoidSpawnedList = new List<HumanoidManager>();
	#endregion
	#region Properties
	public int TotalSpawn 
	{
		get { return this._totalSpawn; }
		set { this._totalSpawn = value; }
	}
	public bool WasHacked 
	{
		get { return this._wasHacked; }
	}
	public bool WasActivatedSpawn 
	{
		get { return this._wasActivatedSpawn;}
		set { this._wasActivatedSpawn = value; } 
	}
	#endregion
	#region Methods (Inherit)
	void Start () 
	{
		this._wasHacked = false;
		this._wasActivatedSpawn = false;
		
		this._timeToSpawn = SPAWNTIME;
		this._totalSpawn = 0;
		
		this._enemyObject = (GameObject)Resources.Load("Enemies/HumanoidRed");
		this._spawnTransform = transform.FindChild("Spawn");
		
		//TODO: Somente p/ visualização.
		transform.parent.parent.renderer.material.color = Color.gray;
	}
	void Update () 
	{
		if (this._wasActivatedSpawn)
		{
			this._timeToSpawn -= Time.deltaTime;
			if (this._timeToSpawn <= 0f)
			{
				this._timeToSpawn = SPAWNTIME;
				InstantiateEnemy();
			}
		}
	}
	void OnTriggerEnter(Collider collider)
	{
		if (!this._wasActivatedSpawn)
		{
			if (collider.tag == "Turret")
			{
				this._wasActivatedSpawn = true;
				this._wasHacked = false;
				
				collider.gameObject.GetComponent<TurretManager>().SetTerminalSpawn(this);
				InstantiateEnemy();
				
				//TODO: Somente p/ visualização.
				transform.parent.parent.renderer.material.color = Color.red;
			}
			else if (collider.tag == "Player")
			{
				this._wasHacked = true;
				
				//TODO: Somente p/ visualização.
				transform.parent.parent.renderer.material.color = Color.green;
			}
		}
	}
	#endregion
	#region Methods (Class)
	private void InstantiateEnemy()
	{
		if (this._totalSpawn > LIMITTOSPAWN)
		{
			this._wasActivatedSpawn = false;
			
			this._totalSpawn = 0;
			this._humanoidSpawnedList.ForEach(h => h.RemoveTerminal());
			this._humanoidSpawnedList.Clear();
			
			//TODO: Somente p/ visualização.
			transform.parent.parent.renderer.material.color = Color.gray;
		}
		else
		{
			this._totalSpawn++;	
			GameObject auxSpawn = (GameObject)Instantiate(this._enemyObject, this._spawnTransform.position, Quaternion.identity);
			
			HumanoidManager auxHumanoid = auxSpawn.GetComponent<HumanoidManager>();
			auxHumanoid.SetComputer(this);
			auxHumanoid.BehaviourType = BehaviourType.GROUP;
			
			this._humanoidSpawnedList.Add(auxHumanoid);
		}
	}
	#endregion
}
